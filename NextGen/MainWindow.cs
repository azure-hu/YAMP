using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using libIsh;

namespace YAMP
{
    public partial class MainWindow : Form
    {
        [System.Runtime.InteropServices.DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 SetWindowTheme(IntPtr hWnd,
                      String textSubAppName, String textSubIdList);

        public MainWindow(ref plEditWnd playlistWindow)
        {
            startDir = System.IO.Directory.GetCurrentDirectory()+ "\\";
            InitializeComponent();
            SetWindowTheme(this.Handle, "", "");
            InitializeIcons(titIco, YAMP.Properties.Resources.TitleIcon, Color.FromArgb(128,128,128));
            InitializeIcons(artIco, YAMP.Properties.Resources.ArtistIcon, Color.FromArgb(128, 128, 128));
            InitializeIcons(albIco, YAMP.Properties.Resources.AlbumIcon, Color.FromArgb(128, 128, 128));
            mainLabelToDefault();
            playStarted = false;
            BassEngine.Init(".\\AudioLib");
            openMedia.Filter = BassEngine.getSupFilter;
            if (playlistWindow != null)
            {
                plEdit = playlistWindow;
                plEdit.setParent(this);
            }
            else
            {
                plEdit = new plEditWnd(this);
            }
            plEdit.Top = this.Top;
            plEdit.Left = this.Right + 1;

            tagForm = new libIsh.tagWnd();
            tagForm.BackgroundImage = global::YAMP.Properties.Resources.background;
            tagForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.tagWindow_FormClosing);


            if (!getSettingsExist)
            {
                MBoxHelper.ShowWarnMsg("Settings file not found!", "Missing File!");
            }
            else
            {
                if (File.Exists(".\\yamp.settings"))
                {
                    if (!File.Exists(".\\yamp.old_config"))
                    {
                        File.Move(".\\yamp.exe.config", ".\\yamp.old_config");
                    }
                    File.Copy(".\\yamp.settings", ".\\yamp.exe.config", true);
                }
                this.TopMost = yamp.Default.alwaysOnTop;
                this.Location = new Point(yamp.Default.locationMainWindow.X, 
                    yamp.Default.locationMainWindow.Y);
                
                FireWorks.Init(ref visualBox, yamp.Default.VisTime);
                FireWorks.SetVisual(yamp.Default.VisualSetting);
                FireWorks.Set("baseColor",yamp.Default.VisColorBottom);
                FireWorks.Set("peakColor", yamp.Default.VisColorTop);
                FireWorks.Set("holdColor", yamp.Default.VisColorHold);
                FireWorks.Set("timerInterval", yamp.Default.VisTime);
                this.seekBar.ForeColor = yamp.Default.SeekBarColor;
                plEdit.Location = new Point(yamp.Default.locationPLEdit.X, 
                    yamp.Default.locationPLEdit.Y);
                plEdit.Size = new Size(yamp.Default.sizePLEdit);
                plEdit.Visible = yamp.Default.showPLEdit;
                tagForm.Visible = yamp.Default.showTagForm;
                this.Opacity = plEdit.Opacity = tagForm.Opacity = yamp.Default.opacity;
            }            
            setToolTips();
            volLabel.Text = "Volume: " + ((int)BassEngine.masterVolume).ToString() + "%";
        }

        private void mainLabelToDefault()
        {

            System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo _info = System.Diagnostics.FileVersionInfo.GetVersionInfo(_assembly.Location);
            mainLabel.Text = String.Copy(_info.ProductName);

        }

        private void tagWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form)sender).Hide();
            e.Cancel = true;
            if (System.IO.File.Exists(".\\yamp.exe.config"))
            {
                yamp.Default.showTagForm = false;
                yamp.Default.Save();
            }
        }

        private void InitializeIcons(PictureBox p, Bitmap b, Color transparentColor)
        {
            b.MakeTransparent(transparentColor);
            p.BackgroundImage = ((System.Drawing.Image)b);
        }

        private void setToolTips()
        {
            ToolTip tp = new ToolTip();
            tp.SetToolTip(visSetBtn, "Visualizer Settings");
            tp.SetToolTip(plistBtn, "PlayList Window");
            tp.SetToolTip(tagfmBtn, "Tag Info Window");
            tp.SetToolTip(volumeUp, "Adjust volume: +4% when left click, 50% on middle click.");
            tp.SetToolTip(volumeDn, "Adjust volume: -4% when left click, Mute on middle click.");
            tp.SetToolTip(opacityUp, "Adjust opacity: +4% when left click, 100% on middle click.");
            tp.SetToolTip(opacityDn, "Adjust opacity: -4% when left click, 20% on middle click.");
        }



        #region methods

        private void ShutDown(object sender, FormClosingEventArgs fce)
        {
            try
            {
                if ((plEdit.getFilesCount > 0) && (getSettingsExist))
                {
                    yamp.Default.lastPlayedIndex = plEdit.playList.SelectedIndex;
                    yamp.Default.Save();
                    plEdit.savePlayList(startDir + yamp.Default.lastPlayedList);
                }
            }
            catch (Exception x)
            {
                libIsh.MBoxHelper.ShowWarnMsg(x, "Warning!");
            }
            finally
            {
                MessBroadcast.StopMSNBroadCast();
                TagReader.Clean();
                BassEngine.Clean();
                Application.Exit();
            }
        }


        private void MouseMovementHandler(object sender, MouseEventArgs maus)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(maus.Location);
                Location = new Point
                    (currentScreenPos.X - offset.X,
                     currentScreenPos.Y - offset.Y);
            }
            if (seeking)
            {
                seekSet = (double)maus.X / (double)seekBar.Width;
                if (seekSet > 1.0)
                {
                    seekSet = 1.0;
                }
                if (seekSet < 0.0)
                {
                    seekSet = 0.0;
                }
                seekBar.Value = (int)(seekSet * seekBar.Maximum);
            }
        }

        private void startDragging(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                dragging = true;
                offset.X = me.X;
                offset.Y = me.Y;
            }
        }

        private void endDragging(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                dragging = false;
                if (getSettingsExist)
                {
                    yamp.Default.locationMainWindow = this.Location;
                    yamp.Default.Save();
                }
            }
        }

        private void startSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (BassEngine.getStatus() > 0))
            {
                seeking = true;
            }
        }

        private void endSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (seeking))
            {
                seeking = false;
                BassEngine.setPositionSeconds(BassEngine.getCurrentLengthSeconds() * seekSet);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;
            if (openMedia.ShowDialog() == DialogResult.OK)
            {
                currentFile = openMedia.FileName;
                plEdit.LoadSingleFile(currentFile);
                PlayNewFile(currentFile);
            }
            mainTimer.Enabled = true;
        }

        public void PlayNewFile(string openThis)
        {
            MessBroadcast.StopMSNBroadCast();
            if (TagReader.Init(openThis))
            {
                currentFile = openThis;
                coverBox.Image = null;
                setInfoControls();           
                BassEngine.Stop();
                BassEngine.Wipe();
                FireWorks.Set("drawFull", false);
                BassEngine.PlayInitFile(openThis.ToLower());
                seekBar.Maximum = (int)BassEngine.getCurrentLengthSeconds();
                drawFileInfo(openThis);
                playStarted = true;
                mainTimer.Enabled = true;
                mainLabel.Switch(true);
                InitTagsForDisplay(openThis);
                MsnMusic();
            }
        }

        private bool setInfoControls()
        {
            try
            {
                sampleRateLabel.Text = TagReader.GetProps[0];
                bitRateLabel.Text = TagReader.GetProps[1];
                chanLabel.Text = "Channels: " + TagReader.GetProps[2];
                totalTimeLabel.Text = "Total: " + TagReader.GetProps[3];
                artistBox.Text = TagReader.existingTag("Performers");
                titleBox.Text = TagReader.existingTag("Title");
                albumBox.Text = TagReader.existingTag("Album");

                mainLabel.Text = " # YAMP # " + artistBox.Text + " - " + titleBox.Text;
                mainLabel.DuplicateIf();

                coverBox.Image = TagReader.GetAlbumArtwork(coverBox.Width - 2, coverBox.Height - 2);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void MsnMusic()
        {
            string TitleText = titleBox.Text;
            if (TitleText == "")
            {
                TitleText = fileBox.Text;
            }
            MessBroadcast.StartMSNBroadCast(artistBox.Text, TitleText, "(YAMP!)");
        }

        private void InitTagsForDisplay(string openThis)
        {
            tagForm.LoadTags(openThis);
        }

        private void drawFileInfo(string openThis)
        {
            fileBox.Text = Path.GetFileName(openThis);
            
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            switch (BassEngine.getStatus())
            {

                case 1:
                    {
                        currentTimeLabel.Text = "Now : " + BassEngine.CurrentPositionString();
                        seekBar.Value = (int)BassEngine.getCurrentPositionSeconds();
                    }
                    break;
                case 0: 
                    {
                        if (playStarted)
                        {

                            switch (repeat)
                            {
                                case repeatOption.none:
                                    NoRepeat();
                                    break;
                                case repeatOption.all:
                                    RepeatAll();
                                    break;
                                case repeatOption.actual:
                                    PlayCurrentFile();

                                    break;
                            }                            
                        }
                    }
                    break;
            }
        }

        private void PlayCurrentFile()
        {
            BassEngine.Play();
        }

        private void RepeatAll()
        {
            if (plEdit.getFilesCount == 1)
                PlayCurrentFile();
            else
            {
                if (plEdit.getFilesCount == plEdit.getCurrentIndex)
                {
                    plEdit.playList_First();
                }
                else
                    plEdit.playList_Next(false);
            }
        }

        private void NoRepeat()
        {
            if ((plEdit.getFilesCount > 1) && (plEdit.getFilesCount > plEdit.getCurrentIndex))
                plEdit.playList_Next(false);
            else
            {
                if (playStarted)
                    stopBtn_Click(stopBtn, null);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            playStarted = false;
            BassEngine.Stop();
            seekBar.Value = seekBar.Minimum;
            sampleRateLabel.Text = bitRateLabel.Text = totalTimeLabel.Text = currentTimeLabel.Text =
                chanLabel.Text = artistBox.Text = titleBox.Text = albumBox.Text = "";
            visualBox.Image = coverBox.Image = null;
            mainTimer.Enabled = false;
            mainLabel.Switch(false);
            mainLabelToDefault();
            MessBroadcast.StopMSNBroadCast();
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            BassEngine.Pause();
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (currentFile != null)
            {
                //setInfoControls();
                PlayNewFile(currentFile);
                //mainTimer.Enabled = true;
            }
            else
            {
                if (plEdit.playList.Items.Count > 0)
                {
                    plEdit.playList_DoubleClick(plEdit.playList, null);
                }
            }
        }

        private void repeatBtn_Click(object sender, EventArgs e)
        {
            switch (repeat)
            {
                case repeatOption.none:
                    {
                        repLabel.ForeColor = Color.Goldenrod;
                        repeat = repeatOption.all;
                        repLabel.Text = "{●} Repeat";
                    }
                    break;
                case repeatOption.all:
                    {
                        repLabel.ForeColor = Color.SkyBlue;
                        repeat = repeatOption.actual;
                        repLabel.Text = "{ } Repeat";
                    }
                    break;
                case repeatOption.actual:
                    {
                        repLabel.ForeColor = Color.Gray;
                        repeat = repeatOption.none;
                    }
                    break;
                }
                    
                    
        }

        private void plistBtn_Click(object sender, EventArgs e)
        {
            plEdit.Visible = !plEdit.Visible;
            if (getSettingsExist)
            {
                yamp.Default.showPLEdit = plEdit.Visible;
                yamp.Default.Save();
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            plEdit.playList_Next(true);
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            plEdit.playList_Prev(true);
        }

        public bool getSettingsExist
        {
            get
            {
                return (File.Exists(".\\yamp.settings") | File.Exists(".\\yamp.exe.config"));
            }
        }

        private void tagfmBtn_Click(object sender, EventArgs e)
        {
            tagForm.Visible = !tagForm.Visible;
            if (getSettingsExist)
            {
                yamp.Default.showTagForm = tagForm.Visible;
                yamp.Default.Save();
            }
        }

        private void volumeUp_Click(object sender, EventArgs e)
        {            
            SetVolume(MouseButtons.Left, true);
        }


        private void volumeDn_Click(object sender, EventArgs e)
        {
            SetVolume(MouseButtons.Left, false);
        }

        private void volumeUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                SetVolume(MouseButtons.Middle, true);
            }
        }

        private void volumeDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                SetVolume(MouseButtons.Middle, false);
            }
        }
        private void SetVolume(MouseButtons e, bool UpOrDn)
        {
            switch (e)
            {
                case MouseButtons.Left:
                    {
                        int dte = (UpOrDn ? 1 : -1);
                        if (BassEngine.masterVolume <= 100)
                        {
                            //BassEngine.masterVolume = Math.Round(BassEngine.masterVolume);
                            BassEngine.masterVolume += (dte * 4);
                        }
                    }
                    break;
                case MouseButtons.Middle:
                    {
                        BassEngine.masterVolume = (UpOrDn ? 50 : 0);
                    }
                    break;
            }
            volLabel.Text = "Volume: " + ((int)BassEngine.masterVolume).ToString() + "%";
        }

        private void visualBox_Click(object sender, EventArgs e)
        {
            FireWorks.SetVisual(FireWorks.GetVSType + 1);
            if (getSettingsExist)
            {
                yamp.Default.VisualSetting = (byte)FireWorks.GetVSType;
                yamp.Default.Save();
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(startDir + yamp.Default.lastPlayedList))
                {
                    plEdit.loadDefaultPlayList(startDir + yamp.Default.lastPlayedList);
                    plEdit.playList.SelectedIndex = yamp.Default.lastPlayedIndex;
                }
            }
            catch (Exception)
            {
                File.Delete(yamp.Default.lastPlayedList);
            }
        }

        private void opacityUp_Click(object sender, EventArgs e)
        {
            SetOpacity(MouseButtons.Left, true);
        }

        private void opacityDn_Click(object sender, EventArgs e)
        {
            SetOpacity(MouseButtons.Left, false);
        }

        private void opacityUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                SetOpacity(MouseButtons.Middle, true);
            }
        }

        private void opacityDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                SetOpacity(MouseButtons.Middle, false);
            }
        }

        private void SetOpacity(MouseButtons e, bool UpOrDn)
        {
            switch (e)
            {
                case MouseButtons.Left:
                    {
                        float dte = (UpOrDn ? 1.00F : -1.00F)*0.04F;
                        double _tempOpac = this.Opacity + dte;
                        if ((_tempOpac > 0.40F) && (_tempOpac <= 1.00F))
                        {
                            this.Opacity = _tempOpac;
                        }
                        
                    }
                    break;
                case MouseButtons.Middle:
                    {
                        plEdit.Opacity = tagForm.Opacity = this.Opacity = (UpOrDn ? 1.00F : 0.40F);
                    }
                    break;
            }
            plEdit.Opacity = tagForm.Opacity = this.Opacity;
            if (getSettingsExist)
            {
                yamp.Default.opacity = this.Opacity;
                yamp.Default.Save();
            }
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        public string EngineSuppExts()
        {
            return BassEngine.getSupFilter;
        }

        private void minBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void topSwitchBtn_Click(object sender, EventArgs e)
        {
            this.TopMost = (!this.TopMost);
            if (getSettingsExist)
            {
                yamp.Default.alwaysOnTop = this.TopMost;
                yamp.Default.Save();
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            plEdit.playList_KeyDown(plEdit.playList, e);
        }

        #endregion methods

        #region variables

        private bool dragging, playStarted;
        private Point offset;
        private plEditWnd plEdit;
        private libIsh.tagWnd tagForm;
        private string currentFile;
        private repeatOption repeat;
        private string startDir;
        private bool seeking;
        private double seekSet;

        #endregion variables

        private void colorSetBtn_Click(object sender, EventArgs e)
        {
            visSetWnd visSelect = new visSetWnd((byte)FireWorks.GetVSType, FireWorks.GetTimerInterval(),
                new Color[] {FireWorks.GetColor("base"), FireWorks.GetColor("peak"), 
                    FireWorks.GetColor("hold"), this.seekBar.ForeColor} );
            if (visSelect.ShowDialog() == DialogResult.OK)
            {
                FireWorks.SetVisual(visSelect.visMode);
                FireWorks.Set("timerInterval", visSelect.visTime);

                FireWorks.Set("baseColor", visSelect.Controls["btmColorBtn"].BackColor);
                FireWorks.Set("peakColor", visSelect.Controls["topColorBtn"].BackColor);
                FireWorks.Set("holdColor", visSelect.Controls["peakColorBtn"].BackColor);
                FireWorks.Set("timerInterval", yamp.Default.VisTime);
                this.seekBar.ForeColor = visSelect.Controls["seekColorBtn"].BackColor;

                if (getSettingsExist)
                {
                    yamp.Default.VisualSetting = visSelect.visMode;
                    yamp.Default.VisTime = FireWorks.GetTimerInterval();
                    yamp.Default.VisColorBottom = FireWorks.GetColor("base");
                    yamp.Default.VisColorTop = FireWorks.GetColor("peak");
                    yamp.Default.VisColorHold = FireWorks.GetColor("hold");
                    yamp.Default.SeekBarColor = this.seekBar.ForeColor;
                    yamp.Default.Save();
                }
            }
        }


        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            int plItems = plEdit.playList.Items.Count;
            int fileCount = ((string[])e.Data.GetData(DataFormats.FileDrop)).Length;
            if ( fileCount > 0 )
            {
                plEdit.playList.SelectedIndex = plItems;
                plEdit.playList_DoubleClick(null, null);
            }
            
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            plEdit.playList_DragEnter(sender, e);
        }

        private void MainWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            if (plEdit != null)
            {
                plEdit.WindowState = this.WindowState;
                if (this.WindowState == FormWindowState.Normal)
                {
                    plEdit.TopMost = this.TopMost;
                    plEdit.Activate();
                }
            }
        }

    }

    public enum repeatOption
    {
        none = 0,
        actual = 1,
        all = -1
    }
}
