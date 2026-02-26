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
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            zCoord = new TextBox();
            yCoord = new TextBox();
            xCoord = new TextBox();
            LoadButton = new Button();
            ModelType = new ComboBox();
            fPSCounter = new Label();
            LoadPreset = new Label();
            comboBox1 = new ComboBox();
            DeleteModelsBtn = new Button();
            listBox1 = new ListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // _glControl
            // 
            _glControl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            _glControl.APIVersion = new Version(3, 3, 0, 0);
            _glControl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            _glControl.IsEventDriven = true;
            _glControl.Location = new Point(12, 12);
            _glControl.Name = "_glControl";
            _glControl.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            _glControl.SharedContext = null;
            _glControl.Size = new Size(711, 671);
            _glControl.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(zCoord);
            panel1.Controls.Add(yCoord);
            panel1.Controls.Add(xCoord);
            panel1.Controls.Add(LoadButton);
            panel1.Controls.Add(ModelType);
            panel1.Controls.Add(fPSCounter);
            panel1.Controls.Add(LoadPreset);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(DeleteModelsBtn);
            panel1.Controls.Add(listBox1);
            panel1.Location = new Point(729, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(414, 671);
            panel1.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(307, 120);
            label3.Name = "label3";
            label3.Size = new Size(20, 23);
            label3.TabIndex = 13;
            label3.Text = "Z";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(154, 120);
            label2.Name = "label2";
            label2.Size = new Size(19, 23);
            label2.TabIndex = 12;
            label2.Text = "Y";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(3, 120);
            label1.Name = "label1";
            label1.Size = new Size(20, 23);
            label1.TabIndex = 11;
            label1.Text = "X";
            // 
            // zCoord
            // 
            zCoord.Location = new Point(307, 146);
            zCoord.Name = "zCoord";
            zCoord.Size = new Size(104, 27);
            zCoord.TabIndex = 10;
            zCoord.Text = "0";
            zCoord.KeyPress += Coord_KeyPress;
            // 
            // yCoord
            // 
            yCoord.Location = new Point(154, 146);
            yCoord.Name = "yCoord";
            yCoord.Size = new Size(104, 27);
            yCoord.TabIndex = 9;
            yCoord.Text = "0";
            yCoord.KeyPress += Coord_KeyPress;
            // 
            // xCoord
            // 
            xCoord.ImeMode = ImeMode.NoControl;
            xCoord.Location = new Point(3, 146);
            xCoord.Name = "xCoord";
            xCoord.Size = new Size(104, 27);
            xCoord.TabIndex = 8;
            xCoord.Text = "0";
            xCoord.KeyPress += Coord_KeyPress;
            // 
            // LoadButton
            // 
            LoadButton.BackColor = Color.YellowGreen;
            LoadButton.Location = new Point(0, 177);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(414, 47);
            LoadButton.TabIndex = 7;
            LoadButton.Text = "Load model";
            LoadButton.UseVisualStyleBackColor = false;
            LoadButton.Click += BtnAddObject_Click;
            // 
            // ModelType
            // 
            ModelType.FormattingEnabled = true;
            ModelType.Items.AddRange(new object[] { "Static", "Move" });
            ModelType.Location = new Point(3, 84);
            ModelType.Name = "ModelType";
            ModelType.Size = new Size(408, 28);
            ModelType.TabIndex = 6;
            ModelType.Text = "Type (Static default)";
            // 
            // fPSCounter
            // 
            fPSCounter.AutoSize = true;
            fPSCounter.Font = new Font("Segoe UI", 20F);
            fPSCounter.Location = new Point(154, 595);
            fPSCounter.Name = "fPSCounter";
            fPSCounter.Size = new Size(118, 46);
            fPSCounter.TabIndex = 5;
            fPSCounter.Text = "FPS: --";
            // 
            // LoadPreset
            // 
            LoadPreset.AutoSize = true;
            LoadPreset.Font = new Font("Segoe UI", 20F);
            LoadPreset.Location = new Point(60, 1);
            LoadPreset.Name = "LoadPreset";
            LoadPreset.Size = new Size(314, 46);
            LoadPreset.TabIndex = 4;
            LoadPreset.Text = "Load preset models";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 50);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(408, 28);
            comboBox1.TabIndex = 3;
            comboBox1.Text = "Models";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // DeleteModelsBtn
            // 
            DeleteModelsBtn.BackColor = Color.Tomato;
            DeleteModelsBtn.Location = new Point(0, 520);
            DeleteModelsBtn.Name = "DeleteModelsBtn";
            DeleteModelsBtn.Size = new Size(414, 75);
            DeleteModelsBtn.TabIndex = 2;
            DeleteModelsBtn.Text = "Delete selected models";
            DeleteModelsBtn.UseVisualStyleBackColor = false;
            DeleteModelsBtn.Click += DeleteModels_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(3, 231);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(411, 284);
            listBox1.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1155, 695);
            Controls.Add(panel1);
            Controls.Add(_glControl);
            Name = "Form1";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
    }
}
