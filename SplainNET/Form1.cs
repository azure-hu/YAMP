using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using libVeronicka;
using System.Threading;

namespace SplainNET
{
    public partial class Form1 : Form
    {
        ChannelReader chRead;
        string strFile = "Streams.splain";
        string StrUrl;
        string StrTitle;
        string FmTitle = "SplainNET";
        bool isPlaying;
        Bass_TagReadWrite ID_Basic;
        //Thread visThread;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            if (InitGui())
            {
                InitBass();
                isPlaying = false;
                FireWorks.Init(ref visBox, 25);
                InitVisualColors();
            }
            else
            {
                MBoxHelper.ShowWarnMsg("Program shutting down...", "SplainNET cannot continue :(");
                this.Close();
            }
        }

        private static void InitVisualColors()
        {
            FireWorks.Set("baseColor", SplainNET.Properties.Settings.Default.baseColor);
            FireWorks.Set("peakColor", SplainNET.Properties.Settings.Default.peakColor);
            FireWorks.Set("holdColor", SplainNET.Properties.Settings.Default.holdColor);
        }


        ~Form1()
        {
            AudioEngine.Clean();
        }

        private void InitBass()
        {
            if (!AudioEngine.Init(string.Format("{0}\\AudioLib_{1}", AudioEngine.AssemblyDirectory, AudioEngine.ProcessorArchitecture)))
            {
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
                proc.Kill();
            }
        }

        private Boolean InitGui()
        {            
            
            if(!File.Exists(".\\" + strFile))
            {
                using (StreamWriter stw = File.CreateText(".\\" + strFile))
                {
                    stw.Write(SplainNET.Properties.Resources.Streams);
                    stw.Close();
                }
            }

            if (ChannelLoading(false))
            {
                loadChannelList();
                streamList.SelectedIndexChanged +=
                    new EventHandler(streamChanged);
                streamList.SelectedIndex = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean ChannelLoading(Boolean choose)
        {
            if (!InitStreams(choose))
            {
                switch (MBoxHelper.ShowAskMsg("Error in channel file. Choose a correct?", "Cannot load channels!"))
                {
                    case DialogResult.Yes: return ChannelLoading(true);
                    case DialogResult.No: return false;
                }
            }
            return true;
        }

        private void loadChannelList()
        {
            streamList.Items.Clear();
            foreach (string i in chRead.Names)
                streamList.Items.Add(i);
        }

        private void streamChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            StrUrl = chRead.GetURL(cb.SelectedIndex);
            StrTitle = (string)cb.Items[cb.SelectedIndex];
            if(isPlaying)
                StopListening();
        }

        private Boolean InitStreams(Boolean choose)
        {

            if (choose)
            {
                OpenFileDialog openF = new OpenFileDialog();
                openF.Filter = "Splain Stream File|*.splain";
                if (openF.ShowDialog() == DialogResult.OK)
                {
                    strFile = openF.FileName;
                }
            }
            
            chRead = new ChannelReader(strFile);
            if (chRead.Size <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (StrUrl != null)
                {
                    if (isPlaying)
                    {
                        StopListening();
                    }
                    else
                    {
                        StartListening(false, null);
                    }
                }
            }
            catch (Exception excep)
            {
                MBoxHelper.ShowWarnMsg(excep, "Warning!");
            }
        }

        private void StartListening(bool record, string recFile)
        {
            isPlaying = AudioEngine.ListenInternetStream(this, FmTitle,
                StrUrl, StrTitle, 3, record, recFile);
            if (isPlaying)
            {
                playBtn.Text = "Stop";
                InitLabels();
                MessBroadcast.StartMSNBroadCast(null, StrTitle, FmTitle);
            }
        }

        private string ShowSaveFileDialog()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "MPEG-1 Layer 3 Audio|*.mp3";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    return sfd.FileName;
                }
                return "output_"+DateTime.Now.ToString().Replace(":","_");
            }
        }

        private void StopListening()
        {
            MessBroadcast.StopMSNBroadCast();
            AudioEngine.ShutDownNet();
            FireWorks.StopDraw();
            visBox.Image = new Bitmap(visBox.Width, visBox.Height);
            this.Text = this.FmTitle;
            isPlaying = false;
            playBtn.Text = "Play";
            recButton.Enabled = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Editor e1 = new Editor(strFile);
            if (e1.ShowDialog() == DialogResult.OK)
            {
                chRead = null;
                chRead = new ChannelReader(strFile);
                loadChannelList();
            }
        }

        private void InitLabels()
        {
            ID_Load();
            ID_Grid.PropertySort = PropertySort.NoSort;
            ID_Grid.SelectedObject = ID_Basic;
            //this.Text = chRead.Names[streamList.SelectedIndex];
        }

        private void ID_Load()
        {
            if (ID_Basic == null)
                ID_Basic = new Bass_TagReadWrite(StrUrl);
            else
            {
                ID_Basic.Update(StrUrl);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListening();
            visBox.Image = null;
            AudioEngine.ShutDownNet();
            AudioEngine.Wipe();
        }

        
        private void visBox_Click(object sender, EventArgs e)
        {
            FireWorks.SetVisual(FireWorks.GetVSType + 1);
        }

        private void recButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (StrUrl != null)
                {
                    string recFile = ShowSaveFileDialog();
                    if (isPlaying)
                    {
                        StopListening();
                    }
                    StartListening(true, recFile);
                    recButton.Enabled = false;
                }
            }
            catch (Exception excep)
            {
                MBoxHelper.ShowWarnMsg(excep, "Warning!");
            }
        }

        private void visBtn_Click(object sender, EventArgs e)
        {
            using (ColorDialog _cld = new ColorDialog())
            {
                for (int i = 0; i < 3; i++)
                {
                    if (_cld.ShowDialog() == DialogResult.OK)
                    {
                        switch (i)
                        {
                            case 0: SplainNET.Properties.Settings.Default.baseColor = _cld.Color; break;
                            case 1: SplainNET.Properties.Settings.Default.peakColor = _cld.Color; break;
                            case 2: SplainNET.Properties.Settings.Default.holdColor = _cld.Color; break;
                        }
                    }
                }
                InitVisualColors();
            }
        }

    }
}
