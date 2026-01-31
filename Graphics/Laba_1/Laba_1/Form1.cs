using System.Numerics;

namespace Laba_1;

public partial class Form1 : Form
{
    private readonly Pen _pen;
    private readonly Graphics _graphics;
    private TriangleStrip _triangleStrip;
    
    public Form1()
    {
        InitializeComponent();
        
        _pen = new Pen(Color.Black);
        _pen.Width = 1;
        
        Bitmap image = new Bitmap(pictureBox1.Width,pictureBox1.Height);
        _graphics = Graphics.FromImage(image);
        pictureBox1.Image = image;
        _graphics.Clear(Color.White);
    }
    
    private void button_Click_OpenFile(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            var fileName = openFileDialog1.FileName;
            _triangleStrip = new TriangleStrip(ReadPointsFromFile(fileName));
            Print();
        }
    }

    private void Print()
    {
        _graphics.Clear(Color.White);
        _triangleStrip.DrawTriangle(_graphics,_pen);
        pictureBox1.Invalidate();
    }

    public static List<Vector4> ReadPointsFromFile(string filePath)
    {
        List<Vector4> points = new List<Vector4>();
            
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                    
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                    
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0 || parts[0] != "v") 
                    continue;
                    
                float x = float.Parse(parts[1]);
                float y = float.Parse(parts[2]);
                float z = float.Parse(parts[3]);
                float w = parts.Length > 4 ? float.Parse(parts[4]) : 1.0f;
                    
                points.Add(new Vector4(x, y, z, w));
            }
        }
            
        return points;
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {

        switch (e.KeyCode)
        {
            case Keys.W:
                _triangleStrip.Move(0,-50,0);
                break;
            case Keys.A:
                _triangleStrip.Move(-50,0,0);
                break;
            case Keys.S:
                _triangleStrip.Move(0,50,0);
                break;
            case Keys.D:
                _triangleStrip.Move(50,0,0);
                break;
        }
        
        Print();
    }

    private void button_Click_X(object sender, EventArgs e)
    {
        _triangleStrip.Rotate(15,0,0);
        Print();
    }

    private void button_Click_Y(object sender, EventArgs e)
    {
        _triangleStrip.Rotate(0,15,0);
        Print();
    }

    private void button_Click_Z(object sender, EventArgs e)
    {
        _triangleStrip.Rotate(0,0,15);
        Print();
    }

    private void button_Click_Plus(object sender, EventArgs e)
    {
        _triangleStrip.Scale(200f);
        Print();
    }

    private void button_Click_Minus(object sender, EventArgs e)
    {
        _triangleStrip.Scale(50f);
        Print();
    }
    
}