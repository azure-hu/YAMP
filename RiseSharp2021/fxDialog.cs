using SimpleWR2.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Azure.MediaUtils;

namespace SimpleWR2
{
	public class fxDialog : Form
	{
		private IContainer components = null;

		private Button btnBack;

		private Button btnBase;

		private Button btnPeak;

		private Button btnHold;

		private Button button5;

		private Button button6;

		private Label label1;

		private Label label2;

		private Label label3;

		private Label label4;


		public fxDialog(Visualizer fireWorks)
		{
			this.InitializeComponent();
			Color color = fireWorks.GetColor(Visualizer.VisualItem.Background);
			this.btnBack.BackColor = (color == Color.Transparent ? Color.Black : color);
			this.btnBase.BackColor = fireWorks.GetColor(Visualizer.VisualItem.Base);
			this.btnPeak.BackColor = fireWorks.GetColor(Visualizer.VisualItem.Peak);
			this.btnHold.BackColor = fireWorks.GetColor(Visualizer.VisualItem.Hold);
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public Color GetColor(Visualizer.VisualItem which)
		{
			Color color = Color.White;
            switch (which)
            {
                case Visualizer.VisualItem.Background:
                    color = (this.btnBack.BackColor == Color.Black ? Color.Transparent : this.btnBack.BackColor);
                    break;
                case Visualizer.VisualItem.Base:
                    color = this.btnBase.BackColor;
                    break;
                case Visualizer.VisualItem.Peak:
                    color = this.btnPeak.BackColor;
                    break;
                case Visualizer.VisualItem.Hold:
                    color = this.btnHold.BackColor;
                    break;
                default:
                    break;
            }
			return color;
		}

		private void InitializeComponent()
		{
            this.btnBack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBase = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPeak = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnHold = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnBack.SuspendLayout();
            this.btnBase.SuspendLayout();
            this.btnPeak.SuspendLayout();
            this.btnHold.SuspendLayout();
            this.SuspendLayout();
            //
            // btnBack
            //
            this.btnBack.Controls.Add(this.label1);
            this.btnBack.Location = new System.Drawing.Point(13, 13);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 0;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.SelectColor);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(20, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Back";
            this.label1.Click += new System.EventHandler(this.Label1Click);
            //
            // btnBase
            //
            this.btnBase.Controls.Add(this.label2);
            this.btnBase.Location = new System.Drawing.Point(13, 43);
            this.btnBase.Name = "btnBase";
            this.btnBase.Size = new System.Drawing.Size(75, 23);
            this.btnBase.TabIndex = 1;
            this.btnBase.UseVisualStyleBackColor = true;
            this.btnBase.Click += new System.EventHandler(this.SelectColor);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(20, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Base";
            this.label2.Click += new System.EventHandler(this.Label2Click);
            //
            // btnPeak
            //
            this.btnPeak.Controls.Add(this.label3);
            this.btnPeak.Location = new System.Drawing.Point(95, 13);
            this.btnPeak.Name = "btnPeak";
            this.btnPeak.Size = new System.Drawing.Size(75, 23);
            this.btnPeak.TabIndex = 2;
            this.btnPeak.UseVisualStyleBackColor = true;
            this.btnPeak.Click += new System.EventHandler(this.SelectColor);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Location = new System.Drawing.Point(20, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Peak";
            this.label3.Click += new System.EventHandler(this.Label3Click);
            //
            // btnHold
            //
            this.btnHold.Controls.Add(this.label4);
            this.btnHold.Location = new System.Drawing.Point(95, 43);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(75, 23);
            this.btnHold.TabIndex = 3;
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.SelectColor);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Location = new System.Drawing.Point(20, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Hold";
            this.label4.Click += new System.EventHandler(this.Label4Click);
            //
            // button5
            //
            this.button5.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button5.Location = new System.Drawing.Point(13, 73);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "OK";
            this.button5.UseVisualStyleBackColor = true;
            //
            // button6
            //
            this.button6.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button6.Location = new System.Drawing.Point(95, 73);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "Cancel";
            this.button6.UseVisualStyleBackColor = true;
            //
            // fxDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(186, 114);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btnHold);
            this.Controls.Add(this.btnPeak);
            this.Controls.Add(this.btnBase);
            this.Controls.Add(this.btnBack);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fxDialog";
            this.Text = "fxDialog";
            this.TopMost = true;
            this.btnBack.ResumeLayout(false);
            this.btnBack.PerformLayout();
            this.btnBase.ResumeLayout(false);
            this.btnBase.PerformLayout();
            this.btnPeak.ResumeLayout(false);
            this.btnPeak.PerformLayout();
            this.btnHold.ResumeLayout(false);
            this.btnHold.PerformLayout();
            this.ResumeLayout(false);

		}

		private void Label1Click(object sender, EventArgs e)
		{
			this.btnBack.PerformClick();
		}

		private void Label2Click(object sender, EventArgs e)
		{
			this.btnBase.PerformClick();
		}

		private void Label3Click(object sender, EventArgs e)
		{
			this.btnPeak.PerformClick();
		}

		private void Label4Click(object sender, EventArgs e)
		{
			this.btnHold.PerformClick();
		}

		private void SelectColor(object sender, EventArgs e)
		{
            Cyotek.Windows.Forms.ColorPickerDialog colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog(this);
            colorPicker.TopMost = true;
			try
			{
				colorPicker.Color = ((Control)sender).BackColor;
				if (colorPicker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					((Control)sender).BackColor = colorPicker.Color;
				}
				else
				{
					return;
				}
			}
			finally
			{
				if (colorPicker != null)
				{
					((IDisposable)colorPicker).Dispose();
				}
			}
		}
	}
}