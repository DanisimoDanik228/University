namespace Laba_1;

public class Camera
{
    public float fov = MathF.PI / 4;
    public float aspect = 1;
    public float near = 0.1f;
    public float far = 1000f;

    public Vector eye = new Vector(0,0,100);
}