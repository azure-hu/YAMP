using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using libDomingo;

namespace NeoFM
{
    public partial class Form1 : Form
    {
        private AxShockwaveFlashObjects.AxShockwaveFlash neoNowPlay;
        private String flashFile = "http://neofm.hu/mostszol.swf", 
            StrUrl = "http://neofmstream2.gtk.hu:8080", 
            StrTitle = "Neo FM Hungary",
            FormTitle = "SPlaIn!";
        private Timer visTimer;

        public Form1()
        {
            InitializeComponent();
            this.Text = FormTitle;
            InitBass();
        }

        ~Form1()
        {
            BassEngine.Clean();
        }

        private void InitBass()
        {
            BassEngine.Init(".\\AudioLib");
            BassEngine.SetVis(BassEngine.VisualSet.SLinePeak);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (loadFlash())
            {
                Timer openNeoLogo = new Timer();
                {
                    openNeoLogo.Interval = 10;
                    openNeoLogo.Tick += new EventHandler(openNeoLogo_Tick);
                    openNeoLogo.Enabled = true;
                }
            }
            else
            {
                ifStreamNotAvailible();
            }
        }

        private void openNeoLogo_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Bottom >= 0)
            {
                pictureBox1.Top -= 1;
                pictureBox2.Top += 1;
            }
            else
            {
                ((Timer)sender).Enabled = false;
                if (BassEngine.ShowStatus(this, FormTitle, StrUrl, StrTitle, 1)) 
                {
                    MessBroadcast.StartMSNBroadCast("Playing", StrTitle, FormTitle);
                    visTimer = new Timer();
                    visTimer.Tick += new EventHandler(visTimer_Tick);
                    visTimer.Interval = 24;
                    visTimer.Enabled = true;
                }
                else
                {
                    ifStreamNotAvailible();
                }           
            }
        }

        private void ifStreamNotAvailible()
        {
            HttpStatusCode response = NetClass.getResponseStatus(StrUrl);
            MBoxHelper.ShowWarnMsg("Error Code: " + ((Int32)response).ToString() + ", " + response.ToString(),
                "Stream unavailible. Closing...");
            this.Close();
            this.Dispose();
        }

        private void closeNeoLogo_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Bottom != pictureBox2.Top)
            {
                pictureBox1.Top += 1;
                pictureBox2.Top -= 1;
            }
            else
            {
                ((Timer)sender).Enabled = false;
                this.Dispose();
            }
        }

        private bool loadFlash()
        {
            if (NetClass.ConnectionAvailable(flashFile))
            {
                neoNowPlay = new AxShockwaveFlashObjects.AxShockwaveFlash();
                neoNowPlay.Location = new Point(0, 0);
                this.Controls.Add(neoNowPlay);
                neoNowPlay.LoadMovie(0, flashFile);
                neoNowPlay.Size = new Size(300, 120);
                neoNowPlay.Play();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void visTimer_Tick(object sender, EventArgs e)
        {
            if (BassEngine.getStatus() > 0)
            {
                try
                {
                    visBox.Image = BassEngine.CreateSpectrum(Color.SkyBlue, Color.Gold,
                        Color.Black, true, true, visBox);
                }
                catch (Exception x)
                {
                    visBox.Image = null;
                }
            }
        }

        private void visBox_Click(object sender, EventArgs e)
        {
            BassEngine.SetVis();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pictureBox1.Bottom <= 0)
            {
                Timer closeNeoLogo = new Timer();
                {
                    closeNeoLogo.Interval = 10;
                    closeNeoLogo.Tick += new EventHandler(closeNeoLogo_Tick);
                    closeNeoLogo.Enabled = true;
                }
            }
            e.Cancel = true;
            visTimer.Enabled = false;
            MessBroadcast.StopMSNBroadCast();
            BassEngine.Stop();
            BassEngine.Wipe();
        }
    }
}
