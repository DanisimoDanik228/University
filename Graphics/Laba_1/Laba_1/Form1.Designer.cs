namespace Laba_1;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        openFileDialog1 = new OpenFileDialog();
        splitContainer1 = new SplitContainer();
        numericUpDown_Z = new NumericUpDown();
        numericUpDown_Y = new NumericUpDown();
        numericUpDown_X = new NumericUpDown();
        button8 = new Button();
        button7 = new Button();
        button6 = new Button();
        button5 = new Button();
        button4 = new Button();
        button3 = new Button();
        button2 = new Button();
        button1 = new Button();
        pictureBox1 = new PictureBox();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numericUpDown_Z).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown_Y).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown_X).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // openFileDialog1
        // 
        openFileDialog1.FileName = "openFileDialog1";
        // 
        // splitContainer1
        // 
        splitContainer1.BorderStyle = BorderStyle.Fixed3D;
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(numericUpDown_Z);
        splitContainer1.Panel1.Controls.Add(numericUpDown_Y);
        splitContainer1.Panel1.Controls.Add(numericUpDown_X);
        splitContainer1.Panel1.Controls.Add(button8);
        splitContainer1.Panel1.Controls.Add(button7);
        splitContainer1.Panel1.Controls.Add(button6);
        splitContainer1.Panel1.Controls.Add(button5);
        splitContainer1.Panel1.Controls.Add(button4);
        splitContainer1.Panel1.Controls.Add(button3);
        splitContainer1.Panel1.Controls.Add(button2);
        splitContainer1.Panel1.Controls.Add(button1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(pictureBox1);
        splitContainer1.Size = new Size(800, 450);
        splitContainer1.SplitterDistance = 215;
        splitContainer1.TabIndex = 0;
        splitContainer1.Text = "splitContainer1";
        // 
        // numericUpDown_Z
        // 
        numericUpDown_Z.Location = new Point(3, 273);
        numericUpDown_Z.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
        numericUpDown_Z.Name = "numericUpDown_Z";
        numericUpDown_Z.Size = new Size(50, 27);
        numericUpDown_Z.TabIndex = 10;
        numericUpDown_Z.ValueChanged += numericUpDown_Z_ValueChanged;
        // 
        // numericUpDown_Y
        // 
        numericUpDown_Y.Location = new Point(3, 240);
        numericUpDown_Y.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
        numericUpDown_Y.Name = "numericUpDown_Y";
        numericUpDown_Y.Size = new Size(50, 27);
        numericUpDown_Y.TabIndex = 9;
        numericUpDown_Y.ValueChanged += numericUpDown_Y_ValueChanged;
        // 
        // numericUpDown_X
        // 
        numericUpDown_X.Location = new Point(3, 207);
        numericUpDown_X.Minimum = new decimal(new int[] { 100, 0, 0, int.MinValue });
        numericUpDown_X.Name = "numericUpDown_X";
        numericUpDown_X.Size = new Size(50, 27);
        numericUpDown_X.TabIndex = 8;
        numericUpDown_X.ValueChanged += numericUpDown_X_ValueChanged;
        // 
        // button8
        // 
        button8.Dock = DockStyle.Top;
        button8.Location = new Point(0, 178);
        button8.Name = "button8";
        button8.Size = new Size(211, 23);
        button8.TabIndex = 7;
        button8.Text = "#  -  #";
        button8.UseVisualStyleBackColor = true;
        button8.Click += button8_Click;
        // 
        // button7
        // 
        button7.Dock = DockStyle.Top;
        button7.Location = new Point(0, 155);
        button7.Name = "button7";
        button7.Size = new Size(211, 23);
        button7.TabIndex = 6;
        button7.Text = "#  +  #";
        button7.UseVisualStyleBackColor = true;
        button7.Click += button7_Click;
        // 
        // button6
        // 
        button6.Dock = DockStyle.Top;
        button6.Location = new Point(0, 132);
        button6.Name = "button6";
        button6.Size = new Size(211, 23);
        button6.TabIndex = 5;
        button6.Text = "-";
        button6.UseVisualStyleBackColor = true;
        button6.Click += button_Click_Minus;
        // 
        // button5
        // 
        button5.Dock = DockStyle.Top;
        button5.Location = new Point(0, 109);
        button5.Name = "button5";
        button5.Size = new Size(211, 23);
        button5.TabIndex = 4;
        button5.Text = "+";
        button5.UseVisualStyleBackColor = true;
        button5.Click += button_Click_Plus;
        // 
        // button4
        // 
        button4.Dock = DockStyle.Top;
        button4.Location = new Point(0, 86);
        button4.Name = "button4";
        button4.Size = new Size(211, 23);
        button4.TabIndex = 3;
        button4.Text = "Z";
        button4.UseVisualStyleBackColor = true;
        button4.Click += button_Click_Z;
        // 
        // button3
        // 
        button3.Dock = DockStyle.Top;
        button3.Location = new Point(0, 63);
        button3.Name = "button3";
        button3.Size = new Size(211, 23);
        button3.TabIndex = 2;
        button3.Text = "Y";
        button3.UseVisualStyleBackColor = true;
        button3.Click += button_Click_Y;
        // 
        // button2
        // 
        button2.Dock = DockStyle.Top;
        button2.Location = new Point(0, 40);
        button2.Name = "button2";
        button2.Size = new Size(211, 23);
        button2.TabIndex = 1;
        button2.Text = "X";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button_Click_X;
        // 
        // button1
        // 
        button1.Dock = DockStyle.Top;
        button1.Location = new Point(0, 0);
        button1.Name = "button1";
        button1.Size = new Size(211, 40);
        button1.TabIndex = 0;
        button1.Text = "Open File";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button_Click_OpenFile;
        // 
        // pictureBox1
        // 
        pictureBox1.Dock = DockStyle.Fill;
        pictureBox1.Location = new Point(0, 0);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(2000, 2000);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(splitContainer1);
        KeyPreview = true;
        Name = "Form1";
        Text = "Form1";
        WindowState = FormWindowState.Maximized;
        KeyDown += Form1_KeyDown;
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numericUpDown_Z).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown_Y).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown_X).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button button7;
    private System.Windows.Forms.Button button8;

    private System.Windows.Forms.PictureBox pictureBox1;

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.Button button6;

    private System.Windows.Forms.SplitContainer splitContainer1;

    private System.Windows.Forms.OpenFileDialog openFileDialog1;

    #endregion

    private NumericUpDown numericUpDown_Z;
    private NumericUpDown numericUpDown_Y;
    private NumericUpDown numericUpDown_X;
}