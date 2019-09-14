namespace Azure.YAMP
{
    partial class visSetWnd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(visSetWnd));
			this.styleGroup = new System.Windows.Forms.GroupBox();
			this.beanSpectRad = new System.Windows.Forms.RadioButton();
			this.waveSpectRad = new System.Windows.Forms.RadioButton();
			this.peakSpectRad = new System.Windows.Forms.RadioButton();
			this.waveFormRad = new System.Windows.Forms.RadioButton();
			this.lineSpectRad = new System.Windows.Forms.RadioButton();
			this.fullSpectRad = new System.Windows.Forms.RadioButton();
			this.topColorBtn = new System.Windows.Forms.Button();
			this.colorLbl = new System.Windows.Forms.Label();
			this.btmColorBtn = new System.Windows.Forms.Button();
			this.noBtn = new System.Windows.Forms.Button();
			this.okBtn = new System.Windows.Forms.Button();
			this.vtLbl = new System.Windows.Forms.Label();
			this.rfLbl = new System.Windows.Forms.Label();
			this.rfrBar = new System.Windows.Forms.TrackBar();
			this.peakColorBtn = new System.Windows.Forms.Button();
			this.seekColorBtn = new System.Windows.Forms.Button();
			this.useDigitFontCB = new System.Windows.Forms.CheckBox();
			this.styleGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rfrBar)).BeginInit();
			this.SuspendLayout();
			// 
			// styleGroup
			// 
			this.styleGroup.BackColor = System.Drawing.Color.Transparent;
			this.styleGroup.Controls.Add(this.beanSpectRad);
			this.styleGroup.Controls.Add(this.waveSpectRad);
			this.styleGroup.Controls.Add(this.peakSpectRad);
			this.styleGroup.Controls.Add(this.waveFormRad);
			this.styleGroup.Controls.Add(this.lineSpectRad);
			this.styleGroup.Controls.Add(this.fullSpectRad);
			this.styleGroup.ForeColor = System.Drawing.Color.White;
			this.styleGroup.Location = new System.Drawing.Point(12, 12);
			this.styleGroup.Name = "styleGroup";
			this.styleGroup.Size = new System.Drawing.Size(268, 100);
			this.styleGroup.TabIndex = 0;
			this.styleGroup.TabStop = false;
			this.styleGroup.Text = "Visualizer Style";
			// 
			// beanSpectRad
			// 
			this.beanSpectRad.AutoSize = true;
			this.beanSpectRad.Location = new System.Drawing.Point(161, 42);
			this.beanSpectRad.Name = "beanSpectRad";
			this.beanSpectRad.Size = new System.Drawing.Size(98, 17);
			this.beanSpectRad.TabIndex = 5;
			this.beanSpectRad.Text = "Bean Spectrum";
			this.beanSpectRad.UseVisualStyleBackColor = true;
			this.beanSpectRad.CheckedChanged += new System.EventHandler(this.beanSpectRad_CheckedChanged);
			// 
			// waveSpectRad
			// 
			this.waveSpectRad.AutoSize = true;
			this.waveSpectRad.Location = new System.Drawing.Point(161, 19);
			this.waveSpectRad.Name = "waveSpectRad";
			this.waveSpectRad.Size = new System.Drawing.Size(102, 17);
			this.waveSpectRad.TabIndex = 4;
			this.waveSpectRad.Text = "Wave Spectrum";
			this.waveSpectRad.UseVisualStyleBackColor = true;
			this.waveSpectRad.CheckedChanged += new System.EventHandler(this.waveSpectRad_CheckedChanged);
			// 
			// peakSpectRad
			// 
			this.peakSpectRad.AutoSize = true;
			this.peakSpectRad.Location = new System.Drawing.Point(6, 65);
			this.peakSpectRad.Name = "peakSpectRad";
			this.peakSpectRad.Size = new System.Drawing.Size(149, 17);
			this.peakSpectRad.TabIndex = 3;
			this.peakSpectRad.Text = "Lined Spectrum with Peak";
			this.peakSpectRad.UseVisualStyleBackColor = true;
			this.peakSpectRad.CheckedChanged += new System.EventHandler(this.peakSpectRad_CheckedChanged);
			// 
			// waveFormRad
			// 
			this.waveFormRad.AutoSize = true;
			this.waveFormRad.Location = new System.Drawing.Point(161, 65);
			this.waveFormRad.Name = "waveFormRad";
			this.waveFormRad.Size = new System.Drawing.Size(80, 17);
			this.waveFormRad.TabIndex = 6;
			this.waveFormRad.Text = "Wave Form";
			this.waveFormRad.UseVisualStyleBackColor = true;
			this.waveFormRad.CheckedChanged += new System.EventHandler(this.waveFormRad_CheckedChanged);
			// 
			// lineSpectRad
			// 
			this.lineSpectRad.AutoSize = true;
			this.lineSpectRad.Location = new System.Drawing.Point(6, 42);
			this.lineSpectRad.Name = "lineSpectRad";
			this.lineSpectRad.Size = new System.Drawing.Size(99, 17);
			this.lineSpectRad.TabIndex = 2;
			this.lineSpectRad.Text = "Lined Spectrum";
			this.lineSpectRad.UseVisualStyleBackColor = true;
			this.lineSpectRad.CheckedChanged += new System.EventHandler(this.lineSpectRad_CheckedChanged);
			// 
			// fullSpectRad
			// 
			this.fullSpectRad.AutoSize = true;
			this.fullSpectRad.Checked = true;
			this.fullSpectRad.Location = new System.Drawing.Point(6, 19);
			this.fullSpectRad.Name = "fullSpectRad";
			this.fullSpectRad.Size = new System.Drawing.Size(89, 17);
			this.fullSpectRad.TabIndex = 1;
			this.fullSpectRad.TabStop = true;
			this.fullSpectRad.Text = "Full Spectrum";
			this.fullSpectRad.UseVisualStyleBackColor = true;
			this.fullSpectRad.CheckedChanged += new System.EventHandler(this.fullSpectRad_CheckedChanged);
			// 
			// topColorBtn
			// 
			this.topColorBtn.Location = new System.Drawing.Point(112, 118);
			this.topColorBtn.Name = "topColorBtn";
			this.topColorBtn.Size = new System.Drawing.Size(52, 24);
			this.topColorBtn.TabIndex = 16;
			this.topColorBtn.Text = "Top";
			this.topColorBtn.UseVisualStyleBackColor = true;
			this.topColorBtn.Click += new System.EventHandler(this.chooseColor);
			// 
			// colorLbl
			// 
			this.colorLbl.AutoSize = true;
			this.colorLbl.BackColor = System.Drawing.Color.Transparent;
			this.colorLbl.ForeColor = System.Drawing.Color.White;
			this.colorLbl.Location = new System.Drawing.Point(9, 124);
			this.colorLbl.Name = "colorLbl";
			this.colorLbl.Size = new System.Drawing.Size(42, 13);
			this.colorLbl.TabIndex = 18;
			this.colorLbl.Text = "Colors: ";
			// 
			// btmColorBtn
			// 
			this.btmColorBtn.Location = new System.Drawing.Point(170, 118);
			this.btmColorBtn.Name = "btmColorBtn";
			this.btmColorBtn.Size = new System.Drawing.Size(52, 24);
			this.btmColorBtn.TabIndex = 19;
			this.btmColorBtn.Text = "Bottom";
			this.btmColorBtn.UseVisualStyleBackColor = true;
			this.btmColorBtn.Click += new System.EventHandler(this.chooseColor);
			// 
			// noBtn
			// 
			this.noBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.noBtn.Location = new System.Drawing.Point(232, 172);
			this.noBtn.Name = "noBtn";
			this.noBtn.Size = new System.Drawing.Size(48, 22);
			this.noBtn.TabIndex = 21;
			this.noBtn.Text = "Cancel";
			this.noBtn.UseVisualStyleBackColor = true;
			// 
			// okBtn
			// 
			this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okBtn.Location = new System.Drawing.Point(232, 148);
			this.okBtn.Name = "okBtn";
			this.okBtn.Size = new System.Drawing.Size(48, 22);
			this.okBtn.TabIndex = 22;
			this.okBtn.Text = "OK";
			this.okBtn.UseVisualStyleBackColor = true;
			this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
			// 
			// vtLbl
			// 
			this.vtLbl.AutoSize = true;
			this.vtLbl.BackColor = System.Drawing.Color.Transparent;
			this.vtLbl.ForeColor = System.Drawing.Color.White;
			this.vtLbl.Location = new System.Drawing.Point(9, 153);
			this.vtLbl.Name = "vtLbl";
			this.vtLbl.Size = new System.Drawing.Size(73, 13);
			this.vtLbl.TabIndex = 23;
			this.vtLbl.Text = "Refresh Time:";
			// 
			// rfLbl
			// 
			this.rfLbl.AutoSize = true;
			this.rfLbl.BackColor = System.Drawing.Color.Transparent;
			this.rfLbl.ForeColor = System.Drawing.Color.White;
			this.rfLbl.Location = new System.Drawing.Point(9, 177);
			this.rfLbl.Name = "rfLbl";
			this.rfLbl.Size = new System.Drawing.Size(0, 13);
			this.rfLbl.TabIndex = 24;
			// 
			// rfrBar
			// 
			this.rfrBar.BackColor = System.Drawing.Color.Gray;
			this.rfrBar.LargeChange = 10;
			this.rfrBar.Location = new System.Drawing.Point(88, 153);
			this.rfrBar.Maximum = 100;
			this.rfrBar.Minimum = 25;
			this.rfrBar.Name = "rfrBar";
			this.rfrBar.Size = new System.Drawing.Size(140, 45);
			this.rfrBar.SmallChange = 5;
			this.rfrBar.TabIndex = 25;
			this.rfrBar.TickFrequency = 5;
			this.rfrBar.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.rfrBar.Value = 50;
			this.rfrBar.ValueChanged += new System.EventHandler(this.rfrBar_ValueChanged);
			// 
			// peakColorBtn
			// 
			this.peakColorBtn.Location = new System.Drawing.Point(54, 118);
			this.peakColorBtn.Name = "peakColorBtn";
			this.peakColorBtn.Size = new System.Drawing.Size(52, 24);
			this.peakColorBtn.TabIndex = 20;
			this.peakColorBtn.Text = "Peak";
			this.peakColorBtn.UseVisualStyleBackColor = true;
			this.peakColorBtn.Click += new System.EventHandler(this.chooseColor);
			// 
			// seekColorBtn
			// 
			this.seekColorBtn.Location = new System.Drawing.Point(228, 118);
			this.seekColorBtn.Name = "seekColorBtn";
			this.seekColorBtn.Size = new System.Drawing.Size(52, 24);
			this.seekColorBtn.TabIndex = 26;
			this.seekColorBtn.Text = "Seeker";
			this.seekColorBtn.UseVisualStyleBackColor = true;
			this.seekColorBtn.Click += new System.EventHandler(this.chooseColor);
			// 
			// useDigitFontCB
			// 
			this.useDigitFontCB.AutoSize = true;
			this.useDigitFontCB.BackColor = System.Drawing.Color.Transparent;
			this.useDigitFontCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.useDigitFontCB.ForeColor = System.Drawing.Color.White;
			this.useDigitFontCB.Location = new System.Drawing.Point(12, 204);
			this.useDigitFontCB.Name = "useDigitFontCB";
			this.useDigitFontCB.Size = new System.Drawing.Size(262, 17);
			this.useDigitFontCB.TabIndex = 27;
			this.useDigitFontCB.Text = "Enable LCD-like font style (instead of Arial Narrow)";
			this.useDigitFontCB.UseVisualStyleBackColor = false;
			this.useDigitFontCB.Visible = false;
			// 
			// visSetWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::Azure.YAMP.Properties.Resources.background;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(292, 234);
			this.Controls.Add(this.useDigitFontCB);
			this.Controls.Add(this.seekColorBtn);
			this.Controls.Add(this.rfrBar);
			this.Controls.Add(this.rfLbl);
			this.Controls.Add(this.vtLbl);
			this.Controls.Add(this.okBtn);
			this.Controls.Add(this.noBtn);
			this.Controls.Add(this.peakColorBtn);
			this.Controls.Add(this.btmColorBtn);
			this.Controls.Add(this.colorLbl);
			this.Controls.Add(this.styleGroup);
			this.Controls.Add(this.topColorBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "visSetWnd";
			this.Text = "Visualizer Settings";
			this.Shown += new System.EventHandler(this.visSetWnd_Shown);
			this.styleGroup.ResumeLayout(false);
			this.styleGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.rfrBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox styleGroup;
        private System.Windows.Forms.RadioButton fullSpectRad;
        private System.Windows.Forms.RadioButton waveSpectRad;
        private System.Windows.Forms.RadioButton peakSpectRad;
        private System.Windows.Forms.RadioButton waveFormRad;
        private System.Windows.Forms.RadioButton lineSpectRad;
        private System.Windows.Forms.RadioButton beanSpectRad;
        private System.Windows.Forms.Button topColorBtn;
        private System.Windows.Forms.Label colorLbl;
        private System.Windows.Forms.Button btmColorBtn;
        private System.Windows.Forms.Button noBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label vtLbl;
        private System.Windows.Forms.Label rfLbl;
        private System.Windows.Forms.TrackBar rfrBar;
        private System.Windows.Forms.Button peakColorBtn;
        private System.Windows.Forms.Button seekColorBtn;
        private System.Windows.Forms.CheckBox useDigitFontCB;
    }
}