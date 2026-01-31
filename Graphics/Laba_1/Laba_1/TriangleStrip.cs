using System.Numerics;

namespace Laba_1;

public class TriangleStrip
{
    public List<Vector4> Vertices { get; private set; }
    public Vector4 Center { get; private set; }
    
    public TriangleStrip(params Vector4[] vertices)
    {
        Vertices = new List<Vector4>(vertices);
        UpdateCenter();
    }
    
    public TriangleStrip(IEnumerable<Vector4> vertices)
    {
        Vertices = new List<Vector4>(vertices);
        UpdateCenter();
    }
    
    private void UpdateCenter()
    {
        float cx = 0;//(Vertices[0].X + Vertices[1].X + Vertices[2].X) / 3f;
        float cy = 0;//(Vertices[0].Y + Vertices[1].Y + Vertices[2].Y) / 3f;
        float cz = 0;//(Vertices[0].Z + Vertices[1].Z + Vertices[2].Z) / 3f;

        foreach (var v in Vertices)
        {
            cx += v.X;
            cy += v.Y;
            cz += v.Z;
        }
        
        cx /= Vertices.Count;
        cy /= Vertices.Count;
        cz /= Vertices.Count;

        Center = new Vector4(cx, cy, cz, 1.0f);
    }
    
    public void Rotate(float angleX, float angleY, float angleZ)
    {
        float radX = angleX * (float)Math.PI / 180f;
        float radY = angleY * (float)Math.PI / 180f;
        float radZ = angleZ * (float)Math.PI / 180f;
        
        Matrix rotation = Matrix.CreateRotation(radX, radY, radZ);
        
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector4 relative = new Vector4(
                Vertices[i].X - Center.X,
                Vertices[i].Y - Center.Y,
                Vertices[i].Z - Center.Z,
                1.0f
            );
            
            Vector4 rotated = Matrix.Multiply(rotation, relative);
            
            Vertices[i] = new Vector4(
                rotated.X + Center.X,
                rotated.Y + Center.Y,
                rotated.Z + Center.Z,
                Vertices[i].W
            );
        }
    }
    
    public void DrawTriangle(Graphics graphics,Pen pen)
    {
        var array2D = Vertices.Select(p => new PointF(p.X, p.Y)).ToArray();

        graphics.DrawLine(pen,array2D[0], array2D[1]);
        
        for (int i = 2; i < array2D.Length; i++)
        {
            int j = (i - 1);
            int k = (i - 2);
            
            graphics.DrawLine(pen,array2D[i], array2D[k]);
            graphics.DrawLine(pen,array2D[i], array2D[j]);
        }

    }
    
    public void Scale(float precent)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            var v = Vertices[i];
            v.X = Center.X + (Vertices[i].X - Center.X) * precent / 100;
            v.Y = Center.Y + (Vertices[i].Y - Center.Y) * precent / 100;
            v.Z = Center.Z + (Vertices[i].Z - Center.Z) * precent / 100;
            Vertices[i] = v;
        }    
    }

    public void Move(float x, float y, float z)
    {
        Center = new Vector4(Center.X + x, Center.Y + y, Center.Z + z, Center.W);
        
        for (int i = 0; i < Vertices.Count; i++)
        {
            var v = Vertices[i];
            v.X += x;
            v.Y += y;
            v.Z += z;
            Vertices[i] = v;
        }  
    }

    public void Swap(bool x, bool y, bool z)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            var v = Vertices[i];
            if (x)
            {
                v.X = Center.X - (v.X - Center.X);
            }
            if (y)
            {
                v.Y = Center.Y - (v.Y - Center.Y);
            }
            if (z)
            {
                v.Z = Center.Z - (v.Z - Center.Z);
            }
            Vertices[i] = v;
        }  
    }
}