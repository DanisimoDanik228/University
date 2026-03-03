using System.Numerics;

namespace Laba_1;

public partial class Form1 : Form
{
    private Camera _camera;
    private Bitmap _bmp;
    private Vector _lightDir = new Vector(0, 0, -1);

    private ObjModel _model;
    private Bitmap _diffuseMap;
    private Bitmap _nmMap; 
    private Bitmap _specMap;

    private string _fileObj = @"C:\Users\danil\_ DELETE _\3D model\Head\head.obj";
    private string _fileDiffuse = @"C:\Users\danil\_ DELETE _\3D model\Eye\eyes_diffuse.tga";//.Replace("tga","png");
    private string _fileNm = @"C:\Users\danil\_ DELETE _\3D model\Eye\eyes_nm_tangent.tga";//.Replace("tga", "png");
    private string _fileSpec = @"C:\Users\danil\_ DELETE _\3D model\Eye\eyes_spec.tga";//.Replace("tga", "png");

    public Form1()
    {
        InitializeComponent();

        _bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

        _model = new ObjModel();
        _model.Load(_fileObj);
        _camera = new Camera();

        //_diffuseMap = TgaLoader.LoadTga(_fileDiffuse);
        //_nmMap = TgaLoader.LoadTga(_fileNm);
        //_specMap = TgaLoader.LoadTga(_fileSpec);

        _diffuseMap = new Bitmap(@"C:\Users\danil\_ DELETE _\3D model\Head\head_diffuse.bmp");
        _nmMap = new Bitmap(@"C:\Users\danil\_ DELETE _\3D model\Head\head_nm_tangent.bmp");
        _specMap = new Bitmap(@"C:\Users\danil\_ DELETE _\3D model\Head\head_spec.bmp");
    }

    private void button_Click_OpenFile(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            _fileObj = openFileDialog1.FileName;

            Print();
        }
    }


    private void Print()
    {
        Render.Rendering(_bmp, _model, _camera, _lightDir, _diffuseMap,_nmMap,_specMap);
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
        _model.Scale += 1f;
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

    private void numericUpDown_X_ValueChanged(object sender, EventArgs e)
    {
        _lightDir.X = (float)numericUpDown_X.Value / 10;
        //_lightDir.Normalize();
        Print();
    }

    private void numericUpDown_Y_ValueChanged(object sender, EventArgs e)
    {
        _lightDir.Y = (float)numericUpDown_Y.Value / 10;
        //_lightDir.Normalize();
        Print();
    }

    private void numericUpDown_Z_ValueChanged(object sender, EventArgs e)
    {
        _lightDir.Z = (float)numericUpDown_Z.Value / 10;
        //_lightDir.Normalize();
        Print();
    }

    private void button_Texture_Click(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            _fileDiffuse = openFileDialog1.FileName;

            Print();
        }
    }
}