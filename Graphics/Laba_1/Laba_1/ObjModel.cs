using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Laba_1;

public class ObjModel
{
    public float AngleX = 0, AngleY = 0, AngleZ = 0;
    public float X = 0, Y = 0, Z = 0;
    public float Scale = 1.0f; // По умолчанию масштаб должен быть 1

    public List<Vector> Vertices = new List<Vector>();
    public List<Vector> Normals = new List<Vector>();
    public List<Vector> UVs = new List<Vector>(); // СПИСОК ТЕКСТУРНЫХ КООРДИНАТ
    public List<Face> Faces = new List<Face>();

    public class Face
    {
        public int[] VertexIndices;
        public int[] NormalIndices;
        public int[] UVIndices; // ИНДЕКСЫ ТЕКСТУРНЫХ КООРДИНАТ
    }

    public void Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            MessageBox.Show("Файл не найден!");
            return;
        }

        Vertices.Clear();
        Normals.Clear();
        UVs.Clear();
        Faces.Clear();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v": // Вершины
                    Vertices.Add(new Vector(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)
                    ));
                    break;

                case "vt": // Текстурные координаты (UV)
                    UVs.Add(new Vector(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        parts.Length > 3 ? float.Parse(parts[3], CultureInfo.InvariantCulture) : 0
                    ));
                    break;

                case "vn": // Нормали
                    Normals.Add(new Vector(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture),
                        0
                    ));
                    break;

                case "f": // Грани
                    int count = parts.Length - 1;
                    int[] vIndices = new int[count];
                    int[] nIndices = new int[count];
                    int[] uvIndices = new int[count];

                    for (int i = 0; i < count; i++)
                    {
                        var subParts = parts[i + 1].Split('/');

                        // Индекс вершины (v)
                        int vIdx = int.Parse(subParts[0]);
                        vIndices[i] = vIdx > 0 ? vIdx - 1 : Vertices.Count + vIdx;

                        // Индекс текстуры (vt) - находится между первым и вторым '/'
                        if (subParts.Length >= 2 && !string.IsNullOrEmpty(subParts[1]))
                        {
                            int vtIdx = int.Parse(subParts[1]);
                            uvIndices[i] = vtIdx > 0 ? vtIdx - 1 : UVs.Count + vtIdx;
                        }

                        // Индекс нормали (vn)
                        if (subParts.Length >= 3 && !string.IsNullOrEmpty(subParts[2]))
                        {
                            int nIdx = int.Parse(subParts[2]);
                            nIndices[i] = nIdx > 0 ? nIdx - 1 : Normals.Count + nIdx;
                        }
                    }

                    // Триангуляция (если в полигоне больше 3 вершин)
                    TriangulateFace(vIndices, nIndices, uvIndices);
                    break;
            }
        }

        MessageBox.Show($"Загружено:\nВершин: {Vertices.Count}\nUV-коорд: {UVs.Count}\nНормалей: {Normals.Count}\nПолигонов: {Faces.Count}");
    }

    private void TriangulateFace(int[] vIndices, int[] nIndices, int[] uvIndices)
    {
        // Создаем треугольники по методу "Triangle Fan"
        for (int i = 1; i < vIndices.Length - 1; i++)
        {
            Faces.Add(new Face
            {
                VertexIndices = new int[] { vIndices[0], vIndices[i], vIndices[i + 1] },
                NormalIndices = new int[] { nIndices[0], nIndices[i], nIndices[i + 1] },
                UVIndices = new int[] { uvIndices[0], uvIndices[i], uvIndices[i + 1] }
            });
        }
    }
}