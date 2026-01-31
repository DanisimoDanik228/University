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
    
    public static Matrix CreateRotation(float xAngle, float yAngle, float zAngle)
    {
        Matrix rotX = CreateRotationX(xAngle);
        Matrix rotY = CreateRotationY(yAngle);
        Matrix rotZ = CreateRotationZ(zAngle);
        
        return Multiply(Multiply(rotX, rotY), rotZ);
    }
    
    public static Matrix Multiply(Matrix a, Matrix b)
    {
        Matrix result = new Matrix();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < 4; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        
        return result;
    }
    
    public static Vector4 Multiply(Matrix matrix, Vector4 vector)
    {
        float x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + 
                  matrix[0, 2] * vector.Z + matrix[0, 3] * vector.W;
        float y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + 
                  matrix[1, 2] * vector.Z + matrix[1, 3] * vector.W;
        float z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + 
                  matrix[2, 2] * vector.Z + matrix[2, 3] * vector.W;
        float w = matrix[3, 0] * vector.X + matrix[3, 1] * vector.Y + 
                  matrix[3, 2] * vector.Z + matrix[3, 3] * vector.W;
        
        return new Vector4(x, y, z, w);
    }
}