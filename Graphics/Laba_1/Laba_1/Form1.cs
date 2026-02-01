using System.Numerics;

namespace Laba_1;

public partial class Form1 : Form
{
    private Camera _camera;
    private ObjModel _model;
    private Bitmap _bmp;
    
    public Form1()
    {
        InitializeComponent();
        
        _bmp = new Bitmap(pictureBox1.Width,pictureBox1.Height);
    }
    
    private void button_Click_OpenFile(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            var fileName = openFileDialog1.FileName;
            _model = new ObjModel();
            _model.Load(fileName);
            _camera = new Camera();
            
            Print();
        }
    }


    private void Print()
    {
        Render.Rendering(_bmp,_model,_camera);
        pictureBox1.Image = _bmp;
    }
    

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {

        switch (e.KeyCode)
        {
            case Keys.W:
                _model.Y += 0.5f;
                break;
            case Keys.A:
                _model.X -= 0.5f;
                break;
            case Keys.S:
                _model.Y -= 0.5f;
                break;
            case Keys.D:
                _model.X += 0.5f;
                break;
        }
        
        Print();
    }

    private void button_Click_X(object sender, EventArgs e)
    {
        _model.AngleX += 0.2f;
        Print();
    }

    private void button_Click_Y(object sender, EventArgs e)
    {
        _model.AngleY += 0.2f;
        Print();
    }

    private void button_Click_Z(object sender, EventArgs e)
    {
        _model.AngleZ += 0.2f;
        Print();
    }

    private void button_Click_Plus(object sender, EventArgs e)
    {
        _model.Scale += 0.2f;
        Print();
    }

    private void button_Click_Minus(object sender, EventArgs e)
    {
        _model.Scale -= 0.2f;
        Print();
    }

    private void button7_Click(object sender, EventArgs e)
    {
        //_camera.fov -=  0.2f;
        _camera.eye.X += 10;
        //_camera.aspect += 0.05f;
        //_camera.far += 10f;
        //_camera.near -= 10f;
        Print();
    }
    
    private void button8_Click(object sender, EventArgs e)
    {
        //_camera.fov +=  0.2f;
        _camera.eye.X -= 10;
        //_camera.aspect -= 0.05f;
        //_camera.far -= 10f;
        //_camera.near -= 10f;
        Print();
    }
}