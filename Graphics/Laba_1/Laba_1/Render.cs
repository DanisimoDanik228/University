using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Laba_1;

public class Render
{
    private static float[] _zBuffer;
    private static readonly int _bgColor = Color.FromArgb(255, 30, 30, 35).ToArgb();

    // Настройки освещения
    private const float K_AMBIENT = 0.15f;
    private const float SHININESS = 60f;

    public static unsafe void Rendering(Bitmap bmp, ObjModel model, Camera camera, Vector lightDir,
        Bitmap diffuseMap, Bitmap normalMap, Bitmap specularMap)
    {
        int width = bmp.Width;
        int height = bmp.Height;

        if (_zBuffer == null || _zBuffer.Length != width * height)
            _zBuffer = new float[width * height];

        BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);

        // Блокируем все текстуры
        BitmapData diffData = diffuseMap.LockBits(new Rectangle(0, 0, diffuseMap.Width, diffuseMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        BitmapData normData = normalMap.LockBits(new Rectangle(0, 0, normalMap.Width, normalMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        BitmapData specData = specularMap.LockBits(new Rectangle(0, 0, specularMap.Width, specularMap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        try
        {
            int* pBase = (int*)data.Scan0;
            for (int i = 0; i < width * height; i++) { pBase[i] = _bgColor; _zBuffer[i] = float.MaxValue; }

            var modelM = Matrix.CreateTranslation(model.X, model.Y, model.Z) *
                         Matrix.CreateScale(model.Scale, model.Scale, model.Scale) *
                         Matrix.CreateRotation(model.AngleX, model.AngleY, model.AngleZ);
            var viewM = Matrix.View(camera.eye, new Vector(0, 0, 0), new Vector(0, 1, 0));
            var projM = Matrix.Perspective(camera.fov, (float)width / height, camera.near, camera.far);
            var combinedM = projM * viewM * modelM;

            Vector L = (lightDir * -1.0f).Normalize();

            foreach (var face in model.Faces)
            {
                // Мировые координаты и нормали
                Vector v1_w = modelM * model.Vertices[face.VertexIndices[0]];
                Vector v2_w = modelM * model.Vertices[face.VertexIndices[1]];
                Vector v3_w = modelM * model.Vertices[face.VertexIndices[2]];

                var rotM = Matrix.CreateRotation(model.AngleX, model.AngleY, model.AngleZ);
                Vector n1 = (rotM * model.Normals[face.NormalIndices[0]]).Normalize();
                Vector n2 = (rotM * model.Normals[face.NormalIndices[1]]).Normalize();
                Vector n3 = (rotM * model.Normals[face.NormalIndices[2]]).Normalize();

                Vector uv1 = model.UVs[face.UVIndices[0]];
                Vector uv2 = model.UVs[face.UVIndices[1]];
                Vector uv3 = model.UVs[face.UVIndices[2]];

                // Расчет Тангента для Normal Mapping (упрощенно для треугольника)
                Vector edge1 = v2_w - v1_w;
                Vector edge2 = v3_w - v1_w;
                float du1 = uv2.X - uv1.X;
                float dv1 = uv2.Y - uv1.Y;
                float du2 = uv3.X - uv1.X;
                float dv2 = uv3.Y - uv1.Y;
                float f = 1.0f / (du1 * dv2 - du2 * dv1);
                Vector tangent = new Vector(
                    f * (dv2 * edge1.X - dv1 * edge2.X),
                    f * (dv2 * edge1.Y - dv1 * edge2.Y),
                    f * (dv2 * edge1.Z - dv1 * edge2.Z)
                ).Normalize();

                // Проекция (сохраняем W для перспективной коррекции)
                Vector p1_s = Project(combinedM, model.Vertices[face.VertexIndices[0]], width, height);
                Vector p2_s = Project(combinedM, model.Vertices[face.VertexIndices[1]], width, height);
                Vector p3_s = Project(combinedM, model.Vertices[face.VertexIndices[2]], width, height);

                if (Vector.Dot(n1, (v1_w - camera.eye).Normalize()) > 0) continue;

                DrawTriangleFull(pBase, width, height, data.Stride,
                    p1_s, p2_s, p3_s, v1_w, v2_w, v3_w, n1, n2, n3, tangent, uv1, uv2, uv3,
                    camera.eye, L, diffData, normData, specData);
            }
        }
        finally
        {
            bmp.UnlockBits(data);
            diffuseMap.UnlockBits(diffData);
            normalMap.UnlockBits(normData);
            specularMap.UnlockBits(specData);
        }
    }

    private static unsafe void DrawTriangleFull(int* pBase, int width, int height, int stride,
        Vector p1, Vector p2, Vector p3, Vector v1w, Vector v2w, Vector v3w,
        Vector n1, Vector n2, Vector n3, Vector tangent, Vector uv1, Vector uv2, Vector uv3,
        Vector cameraPos, Vector L, BitmapData diffD, BitmapData normD, BitmapData specD)
    {
        int minX = (int)Math.Max(0, Math.Min(p1.X, Math.Min(p2.X, p3.X)));
        int maxX = (int)Math.Min(width - 1, Math.Max(p1.X, Math.Max(p2.X, p3.X)));
        int minY = (int)Math.Max(0, Math.Min(p1.Y, Math.Min(p2.Y, p3.Y)));
        int maxY = (int)Math.Min(height - 1, Math.Max(p1.Y, Math.Max(p2.Y, p3.Y)));

        // Для перспективной коррекции: атрибуты делим на W (W в Project — это глубина)
        float w1_inv = 1f / p1.W;
        float w2_inv = 1f / p2.W;
        float w3_inv = 1f / p3.W;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Barycentric(p1, p2, p3, x, y, out float b1, out float b2, out float b3);

                if (b1 >= -0.001f && b2 >= -0.001f && b3 >= -0.001f)
                {
                    float z = b1 * p1.Z + b2 * p2.Z + b3 * p3.Z;
                    int idx = y * width + x;

                    if (z < _zBuffer[idx])
                    {
                        _zBuffer[idx] = z;

                        // 1. ПЕРСПЕКТИВНО-КОРРЕКТНАЯ ИНТЕРПОЛЯЦИЯ
                        float interpW = 1.0f / (b1 * w1_inv + b2 * w2_inv + b3 * w3_inv);
                        float u = (b1 * uv1.X * w1_inv + b2 * uv2.X * w2_inv + b3 * uv3.X * w3_inv) * interpW;
                        float v = (b1 * uv1.Y * w1_inv + b2 * uv2.Y * w2_inv + b3 * uv3.Y * w3_inv) * interpW;

                        // 2. ДИФФУЗНАЯ КАРТА
                        Color texColor = SampleTexture(diffD, u, v);

                        // 3. КАРТА НОРМАЛЕЙ (NORMAL MAPPING)
                        Vector interpolatedNormal = (n1 * b1 + n2 * b2 + n3 * b3).Normalize();
                        Vector T = tangent.Normalize();
                        Vector B = Vector.Cross(interpolatedNormal, T).Normalize();
                        Matrix TBN = new Matrix(T, B, interpolatedNormal); // Касательное -> Мировое

                        Color normSample = SampleTexture(normD, u, v);
                        Vector normalFromMap = new Vector(
                            (normSample.R / 255f) * 2f - 1f,
                            (normSample.G / 255f) * 2f - 1f,
                            (normSample.B / 255f) * 2f - 1f
                        ).Normalize();
                        Vector N = (TBN * normalFromMap).Normalize();

                        // 4. ЗЕРКАЛЬНАЯ КАРТА (SPECULAR MAPPING)
                        Color specSample = SampleTexture(specD, u, v);
                        float specIntensity = specSample.R / 255f; // Берем красный канал как силу блеска

                        // 5. ОСВЕЩЕНИЕ (PHONG)
                        Vector P = v1w * b1 + v2w * b2 + v3w * b3;
                        Vector V = (cameraPos - P).Normalize();

                        float dotNL = Math.Max(0f, Vector.Dot(N, L));
                        float diffuse = dotNL;

                        float specular = 0;
                        if (dotNL > 0)
                        {
                            Vector R = (N * (2.0f * dotNL) - L).Normalize();
                            specular = (float)Math.Pow(Math.Max(0f, Vector.Dot(R, V)), SHININESS) * specIntensity;
                        }

                        byte* ptr = (byte*)pBase + (y * stride) + (x * 4);
                        *(int*)ptr = ApplyLighting(texColor, K_AMBIENT, diffuse, specular);
                    }
                }
            }
        }
    }

    private static unsafe Color SampleTexture(BitmapData data, float u, float v)
    {
        u = u - (float)Math.Floor(u);
        v = v - (float)Math.Floor(v);
        int tx = Math.Clamp((int)(u * (data.Width - 1)), 0, data.Width - 1);
        int ty = Math.Clamp((int)((1 - v) * (data.Height - 1)), 0, data.Height - 1);

        byte* pTex = (byte*)data.Scan0 + (ty * data.Stride) + (tx * 4);
        return Color.FromArgb(pTex[3], pTex[2], pTex[1], pTex[0]);
    }

    private static int ApplyLighting(Color texCol, float amb, float diff, float spec)
    {
        float r = texCol.R * (amb + diff) + spec * 255;
        float g = texCol.G * (amb + diff) + spec * 255;
        float b = texCol.B * (amb + diff) + spec * 255;

        return (255 << 24) | ((byte)Math.Clamp(r, 0, 255) << 16) |
                             ((byte)Math.Clamp(g, 0, 255) << 8) |
                              (byte)Math.Clamp(b, 0, 255);
    }

    private static Vector Project(Matrix combined, Vector vRaw, int width, int height)
    {
        Vector v = combined * vRaw;
        float w = v.W; // Сохраняем оригинальный W для коррекции
        if (w != 0) { v.X /= w; v.Y /= w; v.Z /= w; }

        float screenX = (v.X + 1) * width / 2f;
        float screenY = (1 - v.Y) * height / 2f;
        return new Vector(screenX, screenY, v.Z, w);
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