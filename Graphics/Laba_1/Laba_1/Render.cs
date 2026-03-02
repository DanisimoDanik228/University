using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Laba_1;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

public class Render
{
    private static readonly int _bgColor = Color.White.ToArgb();
    private static float[] _zBuffer; 

    public static unsafe void Rendering(Bitmap bmp, ObjModel model, Camera camera,Vector lightDir)
    {
        int width = bmp.Width;
        int height = bmp.Height;

        if (_zBuffer == null || _zBuffer.Length != width * height)
            _zBuffer = new float[width * height];

        BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);

        try
        {
            int* pBase = (int*)data.Scan0;
            int stride = data.Stride;

            for (int i = 0; i < width * height; i++)
            {
                pBase[i] = _bgColor;
                _zBuffer[i] = float.MaxValue;
            }

            var translation = Matrix.CreateTranslation(model.X, model.Y, model.Z);
            var scale =  Matrix.CreateScale(model.Scale, model.Scale, model.Scale);
            var rotation = Matrix.CreateRotation(model.AngleX, model.AngleY, model.AngleZ);
            var projM = Matrix.Perspective(camera.fov, camera.aspect, camera.near, camera.far);
            var viewM = Matrix.View(camera.eye, new Vector(0, 0, 0), new Vector(0, 1, 0));

            var modelM = translation * scale * rotation;
            var combined = projM * viewM * modelM;

            foreach (var face in model.Faces)
            {
                Vector n1 = model.Normals[face.NormalIndices[0]];
                Vector n2 = model.Normals[face.NormalIndices[1]];
                Vector n3 = model.Normals[face.NormalIndices[2]];

                Vector avgNormal = ((n1 + n2 + n3) * (1f/ 3f)).Normalize();
                Vector worldNormal = (rotation * avgNormal).Normalize();

                Vector v0_world = modelM * model.Vertices[face.VertexIndices[0]];
                Vector viewDir = (v0_world - camera.eye).Normalize();
                if (Vector.Dot(worldNormal, viewDir) > 0) 
                    continue;

                float intensity = Vector.Dot(worldNormal, lightDir * -1.0f);
                intensity = Math.Max(0.1f, intensity); 

                int triangleColor = ApplyIntensity(Color.Gray, intensity);

                Vector p1 = Project(combined, model.Vertices[face.VertexIndices[0]], width, height);
                Vector p2 = Project(combined, model.Vertices[face.VertexIndices[1]], width, height);
                Vector p3 = Project(combined, model.Vertices[face.VertexIndices[2]], width, height);

                DrawTriangle(pBase, width, height, stride, p1, p2, p3, triangleColor);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }
    private static Vector Project(Matrix combined, Vector vRaw, int width, int height)
    {
        Vector v = combined * vRaw;
        if (v.W != 0)
        {
            v.X /= v.W;
            v.Y /= v.W;
            v.Z /= v.W;
        }

        float screenX = (v.X * width / 2f);
        float screenY = (-v.Y * height / 2f);

        return new Vector(screenX, screenY, v.Z);
    }

    private static unsafe void DrawTriangle(int* pBase, int width, int height, int stride, Vector p1, Vector p2, Vector p3, int color)
    {
        int minX = (int)Math.Max(0, Math.Min(p1.X, Math.Min(p2.X, p3.X)));
        int maxX = (int)Math.Min(width - 1, Math.Max(p1.X, Math.Max(p2.X, p3.X)));
        int minY = (int)Math.Max(0, Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
        int maxY = (int)Math.Min(height - 1, Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                float w1, w2, w3;
                Barycentric(p1, p2, p3, x, y, out w1, out w2, out w3);

                if (w1 >= 0 && w2 >= 0 && w3 >= 0)
                {
                    float pixelZ = w1 * p1.Z + w2 * p2.Z + w3 * p3.Z;

                    int index = y * width + x;
                    if (pixelZ < _zBuffer[index])
                    {
                        _zBuffer[index] = pixelZ;
                        byte* ptr = (byte*)pBase + (y * stride) + (x * 4);
                        *(int*)ptr = color;
                    }
                }
            }
        }
    }

    private static void Barycentric(Vector a, Vector b, Vector c, float px, float py, out float w1, out float w2, out float w3)
    {
        float det = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
        w1 = ((b.Y - c.Y) * (px - c.X) + (c.X - b.X) * (py - c.Y)) / det;
        w2 = ((c.Y - a.Y) * (px - c.X) + (a.X - c.X) * (py - c.Y)) / det;
        w3 = 1.0f - w1 - w2;
    }

    private static void UpdateZBufferBlock(int width, int height, int startX, int startY, int size, float z)
    {
        for (int offsetY = 0; offsetY < size; offsetY++)
        {
            int y = startY + offsetY;
            if (y >= height) break;

            for (int offsetX = 0; offsetX < size; offsetX++)
            {
                int x = startX + offsetX;
                if (x >= width) break;

                _zBuffer[y * width + x] = z;
            }
        }
    }

    private static bool GetBarycentric(float px, float py, Vector a, Vector b, Vector c, out float w1, out float w2, out float w3)
    {
        float det = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
        w1 = ((b.Y - c.Y) * (px - c.X) + (c.X - b.X) * (py - c.Y)) / det;
        w2 = ((c.Y - a.Y) * (px - c.X) + (a.X - c.X) * (py - c.Y)) / det;
        w3 = 1.0f - w1 - w2;

        return w1 >= 0 && w2 >= 0 && w3 >= 0;
    }

    private static bool IsPointInTriangle(int px, int py, Vector a, Vector b, Vector c)
    {
        float s1 = (a.X - px) * (b.Y - a.Y) - (b.X - a.X) * (a.Y - py);
        float s2 = (b.X - px) * (c.Y - b.Y) - (c.X - b.X) * (b.Y - py);
        float s3 = (c.X - px) * (a.Y - c.Y) - (a.X - c.X) * (c.Y - py);

        return (s1 >= 0 && s2 >= 0 && s3 >= 0) || (s1 <= 0 && s2 <= 0 && s3 <= 0);
    }

    private static int ApplyIntensity(Color color, float intensity)
    {
        int r = (int)(color.R * intensity) % 256;
        int g = (int)(color.G * intensity) % 256;
        int b = (int)(color.B * intensity) % 256;
        return Color.FromArgb(255, r, g, b).ToArgb();
    }
}