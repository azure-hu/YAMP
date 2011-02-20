using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace YAMP
{
    public partial class visSetWnd : Form
    {
        public byte visMode;
        public int visTime;

        public visSetWnd(byte vsStyle, int refrTime, Color[] visColors)
        {
            InitializeComponent();
            setBtnColor(visColors[0], btmColorBtn);
            setBtnColor(visColors[1], topColorBtn);
            setBtnColor(visColors[2], peakColorBtn);
            setBtnColor(visColors[3], seekColorBtn);
            visMode = vsStyle;
            visTime = refrTime;
        }

        private void setBtnColor(Color cl, Button btn)
        {
            if (cl != Color.Transparent)
            {
                btn.BackColor = cl;
            }
            else
            {
                btn.BackColor = Color.Black;
            }
            if ((btn.BackColor.R < 128) || (btn.BackColor.G <128) 
                || (btn.BackColor.B < 128))
            {
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.ForeColor = Color.Black;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            //Dispose();
        }

        private void fullSpectRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 2;
        }

        private void lineSpectRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 0;
        }

        private void peakSpectRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 1;
        }

        private void waveSpectRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 6;
        }

        private void beanSpectRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 3;
        }

        private void waveFormRad_CheckedChanged(object sender, EventArgs e)
        {
            visMode = 7;
        }

        private void chooseColor(object sender, EventArgs e)
        {
            using (ColorDialog cld = new ColorDialog())
            {
                cld.Color = ((Button)sender).BackColor;
                if (cld.ShowDialog() == DialogResult.OK)
                {
                    setBtnColor(cld.Color, (Button)sender);
                }
            }
        }

        private void rfrBar_ValueChanged(object sender, EventArgs e)
        {
            visTime = rfrBar.Value;
            rfLbl.Text = visTime.ToString() + " ms";
        }

        private void visSetWnd_Shown(object sender, EventArgs e)
        {
            switch (visMode)
            {
                case 2: fullSpectRad.Checked = true;
                    break;
                case 0: lineSpectRad.Checked = true;
                    break;
                case 1: peakSpectRad.Checked = true;
                    break;
                case 6: waveSpectRad.Checked = true;
                    break;
                case 3: beanSpectRad.Checked = true;
                    break;
                case 7: waveFormRad.Checked = true;
                    break;
                default: break;
            }
            if ((visTime < 25) || (visTime > 100))
            {
                rfrBar.Value = (rfrBar.Minimum + rfrBar.Maximum) / 2;
            }
            else
            {
                rfrBar.Value = visTime;
            }
            rfrBar_ValueChanged(null, null);
        }
    }
}
