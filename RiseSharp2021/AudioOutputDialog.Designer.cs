namespace SimpleWR2
{
    partial class AudioOutputDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioOutputDialog));
            this.outputPanel = new System.Windows.Forms.Panel();
            this.refreshOutBtn = new System.Windows.Forms.Button();
            this.switchOutBtn = new System.Windows.Forms.Button();
            this.currDeviceBox = new System.Windows.Forms.TextBox();
            this.currDeviceLabel = new System.Windows.Forms.Label();
            this.outputCombo = new System.Windows.Forms.ComboBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.outputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputPanel
            // 
            this.outputPanel.BackColor = System.Drawing.Color.Black;
            this.outputPanel.Controls.Add(this.refreshOutBtn);
            this.outputPanel.Controls.Add(this.switchOutBtn);
            this.outputPanel.Controls.Add(this.currDeviceBox);
            this.outputPanel.Controls.Add(this.currDeviceLabel);
            this.outputPanel.Controls.Add(this.outputCombo);
            this.outputPanel.Controls.Add(this.outputLabel);
            this.outputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputPanel.Location = new System.Drawing.Point(0, 0);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Size = new System.Drawing.Size(315, 90);
            this.outputPanel.TabIndex = 1;
            // 
            // refreshOutBtn
            // 
            this.refreshOutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshOutBtn.ForeColor = System.Drawing.Color.White;
            this.refreshOutBtn.Location = new System.Drawing.Point(191, 58);
            this.refreshOutBtn.Name = "refreshOutBtn";
            this.refreshOutBtn.Size = new System.Drawing.Size(117, 23);
            this.refreshOutBtn.TabIndex = 6;
            this.refreshOutBtn.Text = "Refresh device list";
            this.refreshOutBtn.UseVisualStyleBackColor = true;
            this.refreshOutBtn.Click += new System.EventHandler(this.refreshOutBtn_Click);
            // 
            // switchOutBtn
            // 
            this.switchOutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.switchOutBtn.ForeColor = System.Drawing.Color.White;
            this.switchOutBtn.Location = new System.Drawing.Point(6, 58);
            this.switchOutBtn.Name = "switchOutBtn";
            this.switchOutBtn.Size = new System.Drawing.Size(75, 23);
            this.switchOutBtn.TabIndex = 5;
            this.switchOutBtn.Text = "Switch";
            this.switchOutBtn.UseVisualStyleBackColor = true;
            this.switchOutBtn.Click += new System.EventHandler(this.switchOutBtn_Click);
            // 
            // currDeviceBox
            // 
            this.currDeviceBox.BackColor = System.Drawing.Color.Black;
            this.currDeviceBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currDeviceBox.ForeColor = System.Drawing.Color.White;
            this.currDeviceBox.Location = new System.Drawing.Point(86, 4);
            this.currDeviceBox.Name = "currDeviceBox";
            this.currDeviceBox.ReadOnly = true;
            this.currDeviceBox.Size = new System.Drawing.Size(222, 20);
            this.currDeviceBox.TabIndex = 4;
            // 
            // currDeviceLabel
            // 
            this.currDeviceLabel.AutoSize = true;
            this.currDeviceLabel.ForeColor = System.Drawing.Color.White;
            this.currDeviceLabel.Location = new System.Drawing.Point(3, 7);
            this.currDeviceLabel.Name = "currDeviceLabel";
            this.currDeviceLabel.Size = new System.Drawing.Size(76, 13);
            this.currDeviceLabel.TabIndex = 3;
            this.currDeviceLabel.Text = "Current device";
            // 
            // outputCombo
            // 
            this.outputCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputCombo.FormattingEnabled = true;
            this.outputCombo.Location = new System.Drawing.Point(86, 30);
            this.outputCombo.Name = "outputCombo";
            this.outputCombo.Size = new System.Drawing.Size(222, 21);
            this.outputCombo.TabIndex = 2;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.ForeColor = System.Drawing.Color.White;
            this.outputLabel.Location = new System.Drawing.Point(3, 33);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(79, 13);
            this.outputLabel.TabIndex = 0;
            this.outputLabel.Text = "Output devices";
            // 
            // fmAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 90);
            this.Controls.Add(this.outputPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fmAudioOutput";
            this.Text = "Set Audio Output";
            this.TopMost = true;
            this.outputPanel.ResumeLayout(false);
            this.outputPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel outputPanel;
        private System.Windows.Forms.Button refreshOutBtn;
        private System.Windows.Forms.Button switchOutBtn;
        private System.Windows.Forms.TextBox currDeviceBox;
        private System.Windows.Forms.Label currDeviceLabel;
        private System.Windows.Forms.ComboBox outputCombo;
        private System.Windows.Forms.Label outputLabel;
    }
}