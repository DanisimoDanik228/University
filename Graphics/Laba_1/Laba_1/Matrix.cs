using System.Numerics;

namespace Laba_1;

public class Matrix
{
    private float[,] m = new float[4, 4];
    
    public float this[int row, int col]
    {
        get => m[row, col];
        set => m[row, col] = value;
    }
    
    public static Matrix Identity()
    {
        Matrix matrix = new Matrix();
        for (int i = 0; i < 4; i++)
            matrix[i, i] = 1.0f;
        return matrix;
    }
    
    public static Matrix CreateRotationX(float angle)
    {
        Matrix matrix = Identity();
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        
        matrix[1, 1] = cos;
        matrix[1, 2] = -sin;
        matrix[2, 1] = sin;
        matrix[2, 2] = cos;
        
        return matrix;
    }
    
    public static Matrix CreateRotationY(float angle)
    {
        Matrix matrix = Identity();
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        
        matrix[0, 0] = cos;
        matrix[0, 2] = sin;
        matrix[2, 0] = -sin;
        matrix[2, 2] = cos;
        
        return matrix;
    }
    
    public static Matrix CreateRotationZ(float angle)
    {
        Matrix matrix = Identity();
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        
        matrix[0, 0] = cos;
        matrix[0, 1] = -sin;
        matrix[1, 0] = sin;
        matrix[1, 1] = cos;
        
        return matrix;
    }

    public static Matrix CreateTranslation(float xMove, float yMove, float zMove)
    {
        Matrix matrix = Matrix.Identity();

        matrix[0,3] =  xMove;
        matrix[1,3] =  yMove;
        matrix[2,3] =  zMove;
        
        return matrix;
    }
    
    public static Matrix CreateScale(float xScale, float yScale, float zScale)
    {
        Matrix matrix = new Matrix();
        
        matrix[0, 0] = xScale;
        matrix[1, 1] = yScale;
        matrix[2, 2] = zScale;
        matrix[3, 3] = 1;

        return matrix;
    }

    public static Matrix CreateRotation(float xAngle, float yAngle, float zAngle)
    {
        Matrix rotX = CreateRotationX(xAngle);
        Matrix rotY = CreateRotationY(yAngle);
        Matrix rotZ = CreateRotationZ(zAngle);
        
        return rotX * rotY * rotZ;
    }
    
    public static Matrix operator *(Matrix a, Matrix b)
    {
        Matrix result = new Matrix();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        
        return result;
    }
    
    public static Vector operator *(Matrix a, Vector b)
    {
        float x = a[0, 0] * b.X + a[0, 1] * b.Y + 
                  a[0, 2] * b.Z + a[0, 3] * b.W;
        float y = a[1, 0] * b.X + a[1, 1] * b.Y + 
                  a[1, 2] * b.Z + a[1, 3] * b.W;
        float z = a[2, 0] * b.X + a[2, 1] * b.Y + 
                  a[2, 2] * b.Z + a[2, 3] * b.W;
        float w = a[3, 0] * b.X + a[3, 1] * b.Y + 
                  a[3, 2] * b.Z + a[3, 3] * b.W;
        
        return new Vector(x, y, z, w);
    }
    
    public static Matrix Perspective(float fov, float aspect, float znear, float zfar) {
        var res = new Matrix();
        
        float h = 1.0f / (float)Math.Tan(fov / 2);
        res[0, 0] = h / aspect;
        res[1, 1] = h;
        res[2, 2] = zfar / (znear - zfar);
        res[2, 3] = (znear * zfar) / (znear - zfar);
        res[3, 2] = -1;
        
        return res;
    }
    
    public static Matrix View(Vector eye, Vector target, Vector up)
    {
        Vector zAxis = (eye - target).Normalize();
        Vector xAxis = Vector.Cross(up , zAxis).Normalize();
        Vector yAxis = Vector.Cross(zAxis , xAxis).Normalize();

        Matrix viewMatrix = new Matrix();
        viewMatrix[0, 0] = xAxis.X;
        viewMatrix[0, 1] = xAxis.Y;
        viewMatrix[0, 2] = xAxis.Z;
        viewMatrix[0, 3] = -Vector.Dot(xAxis, eye);
        
        viewMatrix[1, 0] = yAxis.X;
        viewMatrix[1, 1] = yAxis.Y;
        viewMatrix[1, 2] = yAxis.Z;
        viewMatrix[1, 3] = -Vector.Dot(yAxis, eye);
        
        viewMatrix[2, 0] = zAxis.X;
        viewMatrix[2, 1] = zAxis.Y;
        viewMatrix[2, 2] = zAxis.Z;
        viewMatrix[2, 3] = -Vector.Dot(zAxis, eye);
        
        viewMatrix[3, 0] = 0;
        viewMatrix[3, 1] = 0;
        viewMatrix[3, 2] = 0;
        viewMatrix[3, 3] = 1;
        
        return viewMatrix;
    }
}