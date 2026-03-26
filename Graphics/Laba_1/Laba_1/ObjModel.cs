using System.Globalization;

namespace Laba_1;

public class ObjModel
{
    public float AngleX = 0, AngleY = 0,AngleZ = 0;
    public float X = 0, Y = 0,Z = 0;
    public float Scale = 0;
    
    public List<Vector> Vertices = new List<Vector>();
    public List<Vector> Normals = new List<Vector>();
    public List<Face> Faces = new List<Face>();

    public float k_ambient = 0.1f;
    public float k_diffuse = 0.7f;
    public float k_specular = 0.5f;
    public float shininess = 32.0f;

    public class Face
    {
        public int[] VertexIndices;
        public int[] NormalIndices;
    }

    public void Load(string filePath)
    {
        Vertices.Clear();
        Normals.Clear();
        Faces.Clear();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            if (parts[0] == "v")
            {
                Vertices.Add(new Vector(
                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                    float.Parse(parts[3], CultureInfo.InvariantCulture)
                ));
            }
            else if (parts[0] == "vn")
            {
                Normals.Add(new Vector(
                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                    float.Parse(parts[3], CultureInfo.InvariantCulture),
                    0
                ));
            }
            else if (parts[0] == "f")
            {
                int count = parts.Length - 1;
                int[] vIndices = new int[count];
                int[] nIndices = new int[count];

                for (int i = 1; i <= count; i++)
                {
                    var subParts = parts[i].Split('/');

                    int vIdx = int.Parse(subParts[0]);
                    vIndices[i - 1] = vIdx > 0 ? vIdx - 1 : Vertices.Count + vIdx;

                    if (subParts.Length >= 3 && !string.IsNullOrEmpty(subParts[2]))
                    {
                        int nIdx = int.Parse(subParts[2]);
                        nIndices[i - 1] = nIdx > 0 ? nIdx - 1 : Normals.Count + nIdx;
                    }
                }

                TriangulateFace(vIndices, nIndices);
            }
        }

        MessageBox.Show($"Загружено: Вершин: {Vertices.Count}, Нормалей: {Normals.Count}, Полигонов: {Faces.Count}");
    }

    private void TriangulateFace(int[] vIndices, int[] nIndices)
    {
        for (int i = 1; i < vIndices.Length - 1; i++)
        {
            Faces.Add(new Face
            {
                VertexIndices = new int[] { vIndices[0], vIndices[i], vIndices[i + 1] },
                NormalIndices = new int[] { nIndices[0], nIndices[i], nIndices[i + 1] }
            });
        }
    }
}