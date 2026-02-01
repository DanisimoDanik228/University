namespace Laba_1;

public class Render
{
    private static readonly Pen _pen = new Pen(Color.Black){Width = 1};
    private static readonly float midW = 500;
    private static readonly float midH = 500;

    
    public static void Rendering(Bitmap bmp, ObjModel model,Camera camera)
    {
        using Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);

        Matrix modelM = Matrix.CreateTranslation(model.X,model.Y,model.Z) * Matrix.CreateRotation(model.AngleX,model.AngleY,model.AngleZ) * Matrix.CreateScale(model.Scale,model.Scale,model.Scale);
        Matrix viewM = Matrix.View(camera.eye, new Vector(0, 0, 0), new Vector(0, 1, 0));
        Matrix projM = Matrix.Perspective(camera.fov,camera.aspect,camera.near,camera.far);
        
        Matrix combined = projM * viewM * modelM;

        foreach (var face in model.Faces) {
            for (int i = 0; i < face.Length; i++) {
                Vector v1 = model.Vertices[face[i]];
                Vector v2 = model.Vertices[face[(i + 1) % face.Length]];

                v1 = combined * v1;
                v2 = combined * v2;

                if (v1.W != 0) { v1.X /= v1.W; v1.Y /= v1.W; }
                if (v2.W != 0) { v2.X /= v2.W; v2.Y /= v2.W; }

                int screenX1 = (int)(v1.X * midW + midW);
                int screenY1 = (int)(-v1.Y * midH + midH);
                int screenX2 = (int)(v2.X * midW + midW);
                int screenY2 = (int)(-v2.Y * midH + midH);

                
                g.DrawLine(_pen, screenX1, screenY1, screenX2, screenY2);
            }
        }
    }
}
