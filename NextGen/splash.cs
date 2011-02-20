using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace YAMP
{
        public partial class frmSplash : Form
        {
            // The queue that stores AssemblyLoad event info
            public static Queue<string> AsmLoads = new Queue<string>();
            // Controls
            private System.Windows.Forms.TextBox textBox1;
            private System.Windows.Forms.Timer timer1;
            private System.Windows.Forms.Timer timer2;

            public frmSplash()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // frmSplash
                // 
                this.ClientSize = new System.Drawing.Size(292, 266);
                this.Name = "frmSplash";
                this.Load += new System.EventHandler(this.frmSplash_Load);
                this.ResumeLayout(false);

            }

            private void timer1_Tick(object sender, EventArgs e)
            {
                while (AsmLoads.Count > 0)
                {
                    textBox1.Text += AsmLoads.Dequeue() + "\r\n";
                }
                textBox1.ScrollToCaret();
            }

            private void timer2_Tick(object sender, EventArgs e)
            {
                this.Close();
            }

            private void frmSplash_Load(object sender, EventArgs e)
            {
                // Textbox to display assemblies being loaded
                this.textBox1 = new System.Windows.Forms.TextBox();
                this.textBox1.Location = new System.Drawing.Point(25, 45);
                this.textBox1.Multiline = true;
                this.textBox1.Name = "textBox1";
                this.textBox1.Size = new System.Drawing.Size(300, 155);
                this.textBox1.TabIndex = 0;
                this.Controls.Add(this.textBox1);
                // Timer to empty to queue
                this.timer1 = new System.Windows.Forms.Timer();
                this.timer1.Enabled = true;
                this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
                // Timer to simulate program initializing
                this.timer2 = new System.Windows.Forms.Timer();
                this.timer2.Enabled = true;
                this.timer2.Interval = 5000;
                this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
                // Force an assembly to be loaded so we can see it
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
            }
        }
}
