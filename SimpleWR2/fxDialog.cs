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

		public fxDialog(Visualizer vis)
		{
			this.InitializeComponent();
			Color color = vis.GetColor("back");
			this.btnBack.BackColor = (color == Color.Transparent ? Color.Black : color);
			this.btnBase.BackColor = vis.GetColor("base");
			this.btnPeak.BackColor = vis.GetColor("peak");
			this.btnHold.BackColor = vis.GetColor("hold");
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public Color GetColor(string what)
		{
			Color white;
			string str = what;
			if (str == null)
			{
				white = Color.White;
				return white;
			}
			else if (str == "back")
			{
				white = (this.btnBack.BackColor == Color.Black ? Color.Transparent : this.btnBack.BackColor);
			}
			else if (str == "base")
			{
				white = this.btnBase.BackColor;
			}
			else if (str == "peak")
			{
				white = this.btnPeak.BackColor;
			}
			else
			{
				if (str != "hold")
				{
					white = Color.White;
					return white;
				}
				white = this.btnHold.BackColor;
			}
			return white;
		}

		private void InitializeComponent()
		{
			this.btnBack = new Button();
			this.label1 = new Label();
			this.btnBase = new Button();
			this.label2 = new Label();
			this.btnPeak = new Button();
			this.label3 = new Label();
			this.btnHold = new Button();
			this.label4 = new Label();
			this.button5 = new Button();
			this.button6 = new Button();
			this.btnBack.SuspendLayout();
			this.btnBase.SuspendLayout();
			this.btnPeak.SuspendLayout();
			this.btnHold.SuspendLayout();
			base.SuspendLayout();
			this.btnBack.Controls.Add(this.label1);
			this.btnBack.Location = new Point(13, 13);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(75, 23);
			this.btnBack.TabIndex = 0;
			this.btnBack.UseVisualStyleBackColor = true;
			this.btnBack.Click += new EventHandler(this.SelectColor);
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.FromArgb(128, 255, 255, 255);
			this.label1.Location = new Point(20, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Back";
			this.label1.Click += new EventHandler(this.Label1Click);
			this.btnBase.Controls.Add(this.label2);
			this.btnBase.Location = new Point(13, 43);
			this.btnBase.Name = "btnBase";
			this.btnBase.Size = new System.Drawing.Size(75, 23);
			this.btnBase.TabIndex = 1;
			this.btnBase.UseVisualStyleBackColor = true;
			this.btnBase.Click += new EventHandler(this.SelectColor);
			this.label2.AutoSize = true;
			this.label2.BackColor = Color.FromArgb(128, 255, 255, 255);
			this.label2.Location = new Point(20, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Base";
			this.label2.Click += new EventHandler(this.Label2Click);
			this.btnPeak.Controls.Add(this.label3);
			this.btnPeak.Location = new Point(95, 13);
			this.btnPeak.Name = "btnPeak";
			this.btnPeak.Size = new System.Drawing.Size(75, 23);
			this.btnPeak.TabIndex = 2;
			this.btnPeak.UseVisualStyleBackColor = true;
			this.btnPeak.Click += new EventHandler(this.SelectColor);
			this.label3.AutoSize = true;
			this.label3.BackColor = Color.FromArgb(128, 255, 255, 255);
			this.label3.Location = new Point(20, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Peak";
			this.label3.Click += new EventHandler(this.Label3Click);
			this.btnHold.Controls.Add(this.label4);
			this.btnHold.Location = new Point(95, 43);
			this.btnHold.Name = "btnHold";
			this.btnHold.Size = new System.Drawing.Size(75, 23);
			this.btnHold.TabIndex = 3;
			this.btnHold.UseVisualStyleBackColor = true;
			this.btnHold.Click += new EventHandler(this.SelectColor);
			this.label4.AutoSize = true;
			this.label4.BackColor = Color.FromArgb(128, 255, 255, 255);
			this.label4.Location = new Point(20, 5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Hold";
			this.label4.Click += new EventHandler(this.Label4Click);
			this.button5.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button5.Location = new Point(13, 73);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 23);
			this.button5.TabIndex = 4;
			this.button5.Text = "OK";
			this.button5.UseVisualStyleBackColor = true;
			this.button6.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button6.Location = new Point(95, 73);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 5;
			this.button6.Text = "Cancel";
			this.button6.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = SimpleWR2.Properties.Resources.background;
			base.ClientSize = new System.Drawing.Size(184, 112);
			base.Controls.Add(this.button6);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.btnHold);
			base.Controls.Add(this.btnPeak);
			base.Controls.Add(this.btnBase);
			base.Controls.Add(this.btnBack);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "fxDialog";
			this.Text = "fxDialog";
			this.btnBack.ResumeLayout(false);
			this.btnBack.PerformLayout();
			this.btnBase.ResumeLayout(false);
			this.btnBase.PerformLayout();
			this.btnPeak.ResumeLayout(false);
			this.btnPeak.PerformLayout();
			this.btnHold.ResumeLayout(false);
			this.btnHold.PerformLayout();
			base.ResumeLayout(false);
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
			ColorDialog colorDialog = new ColorDialog();
			try
			{
				colorDialog.Color = ((Control)sender).BackColor;
				if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					((Control)sender).BackColor = colorDialog.Color;
				}
				else
				{
					return;
				}
			}
			finally
			{
				if (colorDialog != null)
				{
					((IDisposable)colorDialog).Dispose();
				}
			}
		}
	}
}