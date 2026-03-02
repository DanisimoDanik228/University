using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laba_1;

public class Render
{
    private static readonly int _bgColor = Color.FromArgb(255, 20, 20, 25).ToArgb();
    private static float[] _zBuffer;

    private static readonly Vector lightColor = new Vector(1, 1, 1); 
    private static readonly float k_ambient = 0.1f;  
    private static readonly float k_diffuse = 0.7f;    
    private static readonly float k_specular = 0.5f;   
    private static readonly float shininess = 32.0f;   

    public static unsafe void Rendering(Bitmap bmp, ObjModel model, Camera camera, Vector lightDir)
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

            var modelM = Matrix.CreateTranslation(model.X, model.Y, model.Z) *
                         Matrix.CreateScale(model.Scale, model.Scale, model.Scale) *
                         Matrix.CreateRotation(model.AngleX, model.AngleY, model.AngleZ);

            var viewM = Matrix.View(camera.eye, new Vector(0, 0, 0), new Vector(0, 1, 0));
            var projM = Matrix.Perspective(camera.fov, camera.aspect, camera.near, camera.far);
            var combined = projM * viewM * modelM;

            Vector L = (lightDir * -1.0f).Normalize();

            foreach (var face in model.Faces)
            {
                Vector v1_w = modelM * model.Vertices[face.VertexIndices[0]];
                Vector v2_w = modelM * model.Vertices[face.VertexIndices[1]];
                Vector v3_w = modelM * model.Vertices[face.VertexIndices[2]];

                var rotM = Matrix.CreateRotation(model.AngleX, model.AngleY, model.AngleZ);
                Vector n1_w = (rotM * model.Normals[face.NormalIndices[0]]).Normalize();
                Vector n2_w = (rotM * model.Normals[face.NormalIndices[1]]).Normalize();
                Vector n3_w = (rotM * model.Normals[face.NormalIndices[2]]).Normalize();

                Vector faceCenter = (v1_w + v2_w + v3_w) * (1f / 3f);
                Vector viewToFace = (faceCenter - camera.eye).Normalize();
                Vector avgNormal = (n1_w + n2_w + n3_w).Normalize();
                if (Vector.Dot(avgNormal, viewToFace) > 0) continue;

                Vector p1_s = Project(combined, model.Vertices[face.VertexIndices[0]], width, height);
                Vector p2_s = Project(combined, model.Vertices[face.VertexIndices[1]], width, height);
                Vector p3_s = Project(combined, model.Vertices[face.VertexIndices[2]], width, height);

                DrawTrianglePhong(pBase, width, height, stride,
                    p1_s, p2_s, p3_s,
                    v1_w, v2_w, v3_w,
                    n1_w, n2_w, n3_w,
                    camera.eye, L);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    private static unsafe void DrawTrianglePhong(int* pBase, int width, int height, int stride,
        Vector p1, Vector p2, Vector p3,      
        Vector v1w, Vector v2w, Vector v3w, 
        Vector n1, Vector n2, Vector n3,       
        Vector cameraPos, Vector L)            
    {
        int minX = (int)Math.Max(0, Math.Min(p1.X, Math.Min(p2.X, p3.X)));
        int maxX = (int)Math.Min(width - 1, Math.Max(p1.X, Math.Max(p2.X, p3.X)));
        int minY = (int)Math.Max(0, Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
        int maxY = (int)Math.Min(height - 1, Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

        Color objectColor = Color.Gray;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                float w1, w2, w3;
                Barycentric(p1, p2, p3, x, y, out w1, out w2, out w3);

                if (w1 >= 0 && w2 >= 0 && w3 >= 0)
                {
                    float z = w1 * p1.Z + w2 * p2.Z + w3 * p3.Z;
                    int idx = y * width + x;

                    if (z < _zBuffer[idx])
                    {
                        _zBuffer[idx] = z;

                        Vector interpolatedPos = v1w * w1 + v2w * w2 + v3w * w3;
                        Vector interpolatedNormal = (n1 * w1 + n2 * w2 + n3 * w3).Normalize();

                        Vector V = (cameraPos - interpolatedPos).Normalize();

                        float dotNL = Vector.Dot(interpolatedNormal, L);
                        float diffuseInten = Math.Max(0, dotNL);
                        Vector R = (interpolatedNormal * (2.0f * dotNL) - L).Normalize();

                        float ambient = k_ambient;

                        float diffuse = k_diffuse * diffuseInten;

                        float specInten = (float)Math.Pow(Math.Max(0, Vector.Dot(R, V)), shininess);
                        float specular = k_specular * specInten;

                        byte* ptr = (byte*)pBase + (y * stride) + (x * 4);
                        *(int*)ptr = CalculatePhongPixel(objectColor, ambient, diffuse, specular);
                    }
                }
            }
        }
    }

    private static int CalculatePhongPixel(Color baseCol, float amb, float diff, float spec)
    {
        int r = (int)Math.Clamp((baseCol.R * (amb + diff)) + (spec * 255), 0, 255);
        int g = (int)Math.Clamp((baseCol.G * (amb + diff)) + (spec * 255), 0, 255);
        int b = (int)Math.Clamp((baseCol.B * (amb + diff)) + (spec * 255), 0, 255);

        return Color.FromArgb(255, r, g, b).ToArgb();
    }

    private static Vector Project(Matrix combined, Vector vRaw, int width, int height)
    {
        Vector v = combined * vRaw;
        if (v.W != 0) { v.X /= v.W; v.Y /= v.W; v.Z /= v.W; }

        float screenX = (v.X + 1) * width / 2f;
        float screenY = (1 - v.Y) * height / 2f;
        return new Vector(screenX, screenY, v.Z);
    }

    private static void Barycentric(Vector a, Vector b, Vector c, float px, float py, out float w1, out float w2, out float w3)
    {
        float det = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
        if (Math.Abs(det) < 1e-10) { w1 = w2 = w3 = -1; return; }
        w1 = ((b.Y - c.Y) * (px - c.X) + (c.X - b.X) * (py - c.Y)) / det;
        w2 = ((c.Y - a.Y) * (px - c.X) + (a.X - c.X) * (py - c.Y)) / det;
        w3 = 1.0f - w1 - w2;
    }
}