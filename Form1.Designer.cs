namespace WinFormsOpenTK
{
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _glControl = new OpenTK.GLControl.GLControl();
            panel1 = new Panel();
            panel4 = new Panel();
            label9 = new Label();
            massText = new TextBox();
            checkBox3 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            panel3 = new Panel();
            label5 = new Label();
            xScale = new TextBox();
            label6 = new Label();
            yScale = new TextBox();
            label7 = new Label();
            zScale = new TextBox();
            label8 = new Label();
            panel2 = new Panel();
            label4 = new Label();
            xCoord = new TextBox();
            label3 = new Label();
            yCoord = new TextBox();
            label2 = new Label();
            zCoord = new TextBox();
            label1 = new Label();
            LoadButton = new Button();
            ModelType = new ComboBox();
            fPSCounter = new Label();
            LoadPreset = new Label();
            comboBox1 = new ComboBox();
            DeleteModelsBtn = new Button();
            listBox1 = new ListBox();
            checkBox4 = new CheckBox();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // _glControl
            // 
            _glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            _glControl.APIVersion = new Version(3, 3, 0, 0);
            _glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            _glControl.IsEventDriven = true;
            _glControl.Location = new Point(10, 9);
            _glControl.Margin = new Padding(3, 2, 3, 2);
            _glControl.Name = "_glControl";
            _glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            _glControl.SharedContext = null;
            _glControl.Size = new Size(931, 782);
            _glControl.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(LoadButton);
            panel1.Controls.Add(ModelType);
            panel1.Controls.Add(fPSCounter);
            panel1.Controls.Add(LoadPreset);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(DeleteModelsBtn);
            panel1.Controls.Add(listBox1);
            panel1.Location = new Point(947, 11);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(362, 780);
            panel1.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.ControlDarkDark;
            panel4.Controls.Add(checkBox4);
            panel4.Controls.Add(label9);
            panel4.Controls.Add(massText);
            panel4.Controls.Add(checkBox3);
            panel4.Controls.Add(checkBox2);
            panel4.Controls.Add(checkBox1);
            panel4.Location = new Point(13, 270);
            panel4.Name = "panel4";
            panel4.Size = new Size(335, 135);
            panel4.TabIndex = 16;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.ForeColor = SystemColors.ButtonFace;
            label9.Location = new Point(10, 87);
            label9.Name = "label9";
            label9.Size = new Size(41, 19);
            label9.TabIndex = 15;
            label9.Text = "Mass";
            // 
            // massText
            // 
            massText.ImeMode = ImeMode.NoControl;
            massText.Location = new Point(55, 87);
            massText.Margin = new Padding(3, 2, 3, 2);
            massText.Name = "massText";
            massText.Size = new Size(92, 23);
            massText.TabIndex = 15;
            massText.Text = "10";
            massText.KeyPress += Coord_KeyPress;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(10, 59);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(90, 19);
            checkBox3.TabIndex = 2;
            checkBox3.Text = "Is Kinematic";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += isObjectKinematic;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(10, 34);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(106, 19);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "Use Movement";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(10, 9);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(87, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Use Physics";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += isPhysicsAdded;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlDarkDark;
            panel3.Controls.Add(label5);
            panel3.Controls.Add(xScale);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(yScale);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(zScale);
            panel3.Controls.Add(label8);
            panel3.Location = new Point(13, 182);
            panel3.Name = "panel3";
            panel3.Size = new Size(335, 85);
            panel3.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(143, 11);
            label5.Name = "label5";
            label5.Size = new Size(39, 19);
            label5.TabIndex = 14;
            label5.Text = "Scale";
            // 
            // xScale
            // 
            xScale.ImeMode = ImeMode.NoControl;
            xScale.Location = new Point(3, 60);
            xScale.Margin = new Padding(3, 2, 3, 2);
            xScale.Name = "xScale";
            xScale.Size = new Size(92, 23);
            xScale.TabIndex = 8;
            xScale.Text = "1";
            xScale.KeyPress += Coord_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.ForeColor = SystemColors.ButtonFace;
            label6.Location = new Point(273, 30);
            label6.Name = "label6";
            label6.Size = new Size(17, 19);
            label6.TabIndex = 13;
            label6.Text = "Z";
            // 
            // yScale
            // 
            yScale.Location = new Point(123, 60);
            yScale.Margin = new Padding(3, 2, 3, 2);
            yScale.Name = "yScale";
            yScale.Size = new Size(92, 23);
            yScale.TabIndex = 9;
            yScale.Text = "1";
            yScale.KeyPress += Coord_KeyPress;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(155, 30);
            label7.Name = "label7";
            label7.Size = new Size(17, 19);
            label7.TabIndex = 12;
            label7.Text = "Y";
            // 
            // zScale
            // 
            zScale.Location = new Point(243, 60);
            zScale.Margin = new Padding(3, 2, 3, 2);
            zScale.Name = "zScale";
            zScale.Size = new Size(92, 23);
            zScale.TabIndex = 10;
            zScale.Text = "1";
            zScale.KeyPress += Coord_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(40, 30);
            label8.Name = "label8";
            label8.Size = new Size(17, 19);
            label8.TabIndex = 11;
            label8.Text = "X";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlDarkDark;
            panel2.Controls.Add(label4);
            panel2.Controls.Add(xCoord);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(yCoord);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(zCoord);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(13, 91);
            panel2.Name = "panel2";
            panel2.Size = new Size(335, 85);
            panel2.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.ForeColor = SystemColors.ButtonFace;
            label4.Location = new Point(134, 11);
            label4.Name = "label4";
            label4.Size = new Size(57, 19);
            label4.TabIndex = 14;
            label4.Text = "Position";
            // 
            // xCoord
            // 
            xCoord.ImeMode = ImeMode.NoControl;
            xCoord.Location = new Point(3, 60);
            xCoord.Margin = new Padding(3, 2, 3, 2);
            xCoord.Name = "xCoord";
            xCoord.Size = new Size(92, 23);
            xCoord.TabIndex = 8;
            xCoord.Text = "0";
            xCoord.KeyPress += Coord_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(273, 30);
            label3.Name = "label3";
            label3.Size = new Size(17, 19);
            label3.TabIndex = 13;
            label3.Text = "Z";
            // 
            // yCoord
            // 
            yCoord.Location = new Point(123, 60);
            yCoord.Margin = new Padding(3, 2, 3, 2);
            yCoord.Name = "yCoord";
            yCoord.Size = new Size(92, 23);
            yCoord.TabIndex = 9;
            yCoord.Text = "0";
            yCoord.KeyPress += Coord_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(155, 30);
            label2.Name = "label2";
            label2.Size = new Size(17, 19);
            label2.TabIndex = 12;
            label2.Text = "Y";
            // 
            // zCoord
            // 
            zCoord.Location = new Point(243, 60);
            zCoord.Margin = new Padding(3, 2, 3, 2);
            zCoord.Name = "zCoord";
            zCoord.Size = new Size(92, 23);
            zCoord.TabIndex = 10;
            zCoord.Text = "0";
            zCoord.KeyPress += Coord_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(40, 30);
            label1.Name = "label1";
            label1.Size = new Size(17, 19);
            label1.TabIndex = 11;
            label1.Text = "X";
            // 
            // LoadButton
            // 
            LoadButton.BackColor = Color.YellowGreen;
            LoadButton.Location = new Point(-1, 410);
            LoadButton.Margin = new Padding(3, 2, 3, 2);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(363, 50);
            LoadButton.TabIndex = 7;
            LoadButton.Text = "Load model";
            LoadButton.UseVisualStyleBackColor = false;
            LoadButton.Click += BtnAddObject_Click;
            // 
            // ModelType
            // 
            ModelType.FormattingEnabled = true;
            ModelType.Items.AddRange(new object[] { "Static", "Move" });
            ModelType.Location = new Point(3, 63);
            ModelType.Margin = new Padding(3, 2, 3, 2);
            ModelType.Name = "ModelType";
            ModelType.Size = new Size(358, 23);
            ModelType.TabIndex = 6;
            ModelType.Text = "Type (Static default)";
            // 
            // fPSCounter
            // 
            fPSCounter.AutoSize = true;
            fPSCounter.Font = new Font("Segoe UI", 20F);
            fPSCounter.Location = new Point(135, 725);
            fPSCounter.Name = "fPSCounter";
            fPSCounter.Size = new Size(94, 37);
            fPSCounter.TabIndex = 5;
            fPSCounter.Text = "FPS: --";
            // 
            // LoadPreset
            // 
            LoadPreset.AutoSize = true;
            LoadPreset.Font = new Font("Segoe UI", 20F);
            LoadPreset.Location = new Point(52, 1);
            LoadPreset.Name = "LoadPreset";
            LoadPreset.Size = new Size(250, 37);
            LoadPreset.TabIndex = 4;
            LoadPreset.Text = "Load preset models";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 38);
            comboBox1.Margin = new Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(358, 23);
            comboBox1.TabIndex = 3;
            comboBox1.Text = "Models";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // DeleteModelsBtn
            // 
            DeleteModelsBtn.BackColor = Color.Tomato;
            DeleteModelsBtn.Location = new Point(0, 667);
            DeleteModelsBtn.Margin = new Padding(3, 2, 3, 2);
            DeleteModelsBtn.Name = "DeleteModelsBtn";
            DeleteModelsBtn.Size = new Size(362, 56);
            DeleteModelsBtn.TabIndex = 2;
            DeleteModelsBtn.Text = "Delete selected models";
            DeleteModelsBtn.UseVisualStyleBackColor = false;
            DeleteModelsBtn.Click += DeleteModels_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(0, 464);
            listBox1.Margin = new Padding(3, 2, 3, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(363, 199);
            listBox1.TabIndex = 1;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(155, 9);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(99, 19);
            checkBox4.TabIndex = 16;
            checkBox4.Text = "Use Collisions";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += isCollision;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1321, 802);
            Controls.Add(panel1);
            Controls.Add(_glControl);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private OpenTK.GLControl.GLControl _glControl;
        private Panel panel1;
        private Button DeleteModelsBtn;
        private ListBox listBox1;
        private Label LoadPreset;
        private ComboBox comboBox1;
        private Label fPSCounter;
        private ComboBox ModelType;
        private Button LoadButton;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox zCoord;
        private TextBox yCoord;
        private TextBox xCoord;
        private Panel panel2;
        private Panel panel3;
        private Label label5;
        private TextBox xScale;
        private Label label6;
        private TextBox yScale;
        private Label label7;
        private TextBox zScale;
        private Label label8;
        private Label label4;
        private Panel panel4;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private Label label9;
        private TextBox massText;
        private CheckBox checkBox4;
    }
}
