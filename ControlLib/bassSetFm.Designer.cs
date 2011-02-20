namespace DomCtrlLib
{
    partial class BassSetFm
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
            this.opacBar = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
            this.setBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.waveBar = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
            this.timeoutBar = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.opacBar)).BeginInit();
            this.setBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutBar)).BeginInit();
            this.SuspendLayout();
            // 
            // opacBar
            // 
            this.opacBar.BackColor = System.Drawing.Color.Transparent;
            this.opacBar.Location = new System.Drawing.Point(5, 28);
            this.opacBar.Maximum = 100;
            this.opacBar.Minimum = 20;
            this.opacBar.Name = "opacBar";
            this.opacBar.Size = new System.Drawing.Size(234, 45);
            this.opacBar.TabIndex = 9;
            this.opacBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacBar.Value = 100;
            this.opacBar.Scroll += new System.EventHandler(this.opacBar_Scroll);
            // 
            // setBox
            // 
            this.setBox.BackColor = System.Drawing.SystemColors.Control;
            this.setBox.Controls.Add(this.label3);
            this.setBox.Controls.Add(this.label2);
            this.setBox.Controls.Add(this.label1);
            this.setBox.Controls.Add(this.waveBar);
            this.setBox.Controls.Add(this.timeoutBar);
            this.setBox.Controls.Add(this.opacBar);
            this.setBox.ForeColor = System.Drawing.Color.Maroon;
            this.setBox.Location = new System.Drawing.Point(12, 13);
            this.setBox.Name = "setBox";
            this.setBox.Size = new System.Drawing.Size(245, 167);
            this.setBox.TabIndex = 10;
            this.setBox.TabStop = false;
            this.setBox.Text = "Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(0, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "WaveOut Volume";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(0, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Timeout Length";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Opacity";
            // 
            // waveBar
            // 
            this.waveBar.BackColor = System.Drawing.Color.Transparent;
            this.waveBar.Location = new System.Drawing.Point(8, 116);
            this.waveBar.Maximum = 100;
            this.waveBar.Name = "waveBar";
            this.waveBar.Size = new System.Drawing.Size(231, 45);
            this.waveBar.TabIndex = 11;
            this.waveBar.TickFrequency = 10;
            this.waveBar.Value = 100;
            this.waveBar.Scroll += new System.EventHandler(this.waveBar_Scroll);
            // 
            // timeoutBar
            // 
            this.timeoutBar.BackColor = System.Drawing.Color.Transparent;
            this.timeoutBar.Location = new System.Drawing.Point(5, 71);
            this.timeoutBar.Maximum = 60;
            this.timeoutBar.Minimum = 10;
            this.timeoutBar.Name = "timeoutBar";
            this.timeoutBar.Size = new System.Drawing.Size(234, 45);
            this.timeoutBar.TabIndex = 10;
            this.timeoutBar.TickFrequency = 5;
            this.timeoutBar.Value = 10;
            this.timeoutBar.Scroll += new System.EventHandler(this.changeTimeOut);
            // 
            // settingsFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(269, 192);
            this.Controls.Add(this.setBox);
            this.Name = "settingsFm";
            this.Text = "Settings";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            ((System.ComponentModel.ISupportInitialize)(this.opacBar)).EndInit();
            this.setBox.ResumeLayout(false);
            this.setBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar opacBar;
        private System.Windows.Forms.GroupBox setBox;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar waveBar;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar timeoutBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}