namespace Laba_1;

public class ObjModel
{
    public float AngleX = 0, AngleY = 0,AngleZ = 0;
    public float X = 0, Y = 0,Z = 0;
    public float Scale = 0;
    
    public List<Vector> Vertices = new List<Vector>();
    public List<int[]> Faces = new List<int[]>();
    
    public void Load(string filePath) {
        Vertices.Clear();
        Faces.Clear();
        foreach (var line in File.ReadAllLines(filePath)) {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            if (parts[0] == "v") {
                Vertices.Add(new Vector(
                    float.Parse(parts[1]),
                    float.Parse(parts[2]),
                    float.Parse(parts[3])
                ));
            } else if (parts[0] == "f") {
                int[] faceIndices = new int[parts.Length - 1];
                for (int i = 1; i < parts.Length; i++) {
                    
                    string vIdxStr = parts[i].Split('/')[0];
                    int idx = int.Parse(vIdxStr);
                    
                    faceIndices[i - 1] = idx > 0 ? idx - 1 : Vertices.Count + idx;
                }
                Faces.Add(faceIndices);
            }
        }
    }
}