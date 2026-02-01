namespace Laba_1;

public class Vector
{
    public float X, Y, Z, W;
    
    public Vector(float x, float y, float z, float w = 1) 
    {
        X = x; Y = y; Z = z; W = w;
    }

    public static Vector operator -(Vector a, Vector b)
    {
        return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector Cross(Vector a, Vector b)
    {
        return new Vector(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
    }

    public static float Dot(Vector a, Vector b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    }
    
    public Vector Normalize() 
    {
        float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        return new Vector(X / length, Y / length, Z / length);
    }
}