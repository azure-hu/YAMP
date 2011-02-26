using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using libZoi;
using System.Collections.Generic;

namespace YAMP
{
    public partial class SingleUI : Form
    {
        public SingleUI()
        {
            startDir = System.IO.Directory.GetCurrentDirectory()+ "\\";
            InitializeComponent();
            
            InitializeIcons(titIco, YAMP.Properties.Resources.TitleIcon, Color.FromArgb(128,128,128));
            InitializeIcons(artIco, YAMP.Properties.Resources.ArtistIcon, Color.FromArgb(128, 128, 128));
            InitializeIcons(albIco, YAMP.Properties.Resources.AlbumIcon, Color.FromArgb(128, 128, 128));
            mainLabelToDefault();
            playStarted = false;
            BassEngine.Init(".\\AudioLib");
            openMedia.Filter = BassEngine.getSupFilter;

            tagForm = new libZoi.tagWnd();
            tagForm.BackgroundImage = global::YAMP.Properties.Resources.background;
            tagForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.tagWindow_FormClosing);


            if (!getSettingsExist)
            {
                MBoxHelper.ShowWarnMsg("Settings file not found!", "Missing File!");
                SetWindowTheme(this.Handle, "", "");
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

                if (!yamp.Default.SeekBarStyle)
                    SetWindowTheme(this.Handle, "", "");
                else
                    Application.EnableVisualStyles();
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
                tagForm.Visible = yamp.Default.showTagForm;
                this.Opacity = yamp.Default.opacity;
                if (!yamp.Default.showPLEdit)
                    minimizeWnd();
            }            
            setToolTips();
            volLabel.Text = "Volume: " + ((int)BassEngine.masterVolume).ToString() + "%";

            loadingFiles = interruptLoading = false;
            loaded = new List<MediaItem>();
        }

        #region variables

        private bool dragging, playStarted, seeking;
        private Point offset;
        private libZoi.tagWnd tagForm;
        private string currentFile, startDir;
        private repeatOption repeat;
        private double seekSet;

        private bool loadingFiles, interruptLoading;
        private string defaultTitle;
        private List<MediaItem> loaded;
        private int currentIndex, searchIndex;
        private string helpPlayList, helpSearchBox;

        #endregion variables

        #region assistMethods

        [System.Runtime.InteropServices.DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 SetWindowTheme(IntPtr hWnd,
                      String textSubAppName, String textSubIdList);

        #endregion assistMethods

        #region mainMethods

        private void InitializeIcons(PictureBox p, Bitmap b, Color transparentColor)
        {
            b.MakeTransparent(transparentColor);
            p.BackgroundImage = ((System.Drawing.Image)b);
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
            helpPlayList = ("To move up (down) selected item, press '+' or 'W'  ('-' or 'S') button.");
            helpSearchBox = ("Type a substring to search, and press F3 to jump to the next occurrence.");
        }

        private void ShutDown(object sender, FormClosingEventArgs fce)
        {
            try
            {
                if ((playList.Items.Count > 0) && (getSettingsExist))
                {
                    yamp.Default.lastPlayedIndex = playList.SelectedIndex;
                    yamp.Default.Save();
                    this.savePlayList(startDir + yamp.Default.lastPlayedList);
                }
            }
            catch (Exception x)
            {
                libZoi.MBoxHelper.ShowWarnMsg(x, "Warning!");
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

        #endregion mainMethods

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            int plItems = playList.Items.Count;
            int fileCount = ((string[])e.Data.GetData(DataFormats.FileDrop)).Length;
            if (fileCount > 0)
            {
                playList.SelectedIndex = plItems;
                playList_DoubleClick(null, null);
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(startDir + yamp.Default.lastPlayedList))
                {
                    loadDefaultPlayList(startDir + yamp.Default.lastPlayedList);
                    playList.SelectedIndex = yamp.Default.lastPlayedIndex;
                }
            }
            catch (Exception)
            {
                File.Delete(yamp.Default.lastPlayedList);
            }
        }

        private void plistBtn_Click(object sender, EventArgs e)
        {
            if (this.Width != mainSkin.Width)
                minimizeWnd();

            else
                maximizeWnd();
        }

        private void maximizeWnd()
        {
            this.Width = 640;
            seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 12;
            fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 36);
            coverBox.Left = 258;
            yamp.Default.showPLEdit = true;
            yamp.Default.Save();
        }

        private void minimizeWnd()
        {
            this.Width = mainSkin.Width;
            seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 6;
            fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 30);
            coverBox.Left = 252;
            yamp.Default.showPLEdit = false;
            yamp.Default.Save();
        }
    }

    public enum repeatOption
    {
        none = 0,
        actual = 1,
        all = -1
    }
}
