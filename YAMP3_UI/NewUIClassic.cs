using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Azure.LibCollection.CS;
using Fusionbird.FusionToolkit.FusionTrackBar;

namespace Azure.YAMP
{
    public partial class NewUIClassic : Form
    {
        #region variables

        private bool dragging, playStarted, seeking;
        private Point offset;
        private Azure.LibCollection.CS.tagWnd tagForm;
        private string currentFile, startDir;
        private repeatOption repeat;
        private double seekSet;

        private bool loadingFiles, interruptLoading;
        private string defaultTitle;
        private List<MediaItem> loaded;
        public int currentIndex, selectedIndex;
        private int searchIndex;
        private string helpPlayList, helpSearchBox;
        public bool dragHappened;
        private const int cGrip = 16;
        private PrivateFontCollection pfc;
        private List<string> pfc_names;
        private const int withPlWidth = 656;
        private EqualiserEngine eq;
        private readonly string _asmName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private bool startup;
        private PlayList playList;

        #endregion variables

        public NewUIClassic()
        {
            startup = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            startDir = System.IO.Directory.GetCurrentDirectory()+ "\\";
            InitializeComponent();
            playList = new PlayList(this.playListView, this);
            this.blockPanel.BringToFront();

            //InitializeIcons(titIco, YAMP.Properties.Resources.TitleIcon, Color.FromArgb(128,128,128));
            //InitializeIcons(artIco, YAMP.Properties.Resources.ArtistIcon, Color.FromArgb(128, 128, 128));
            //InitializeIcons(albIco, YAMP.Properties.Resources.AlbumIcon, Color.FromArgb(128, 128, 128));
            mainLabelToDefault();
            playStarted = false;

            string[] libPaths = new string[] { string.Format("{0}\\AudioLib_{1}", Program.AssemblyDirectory, AudioEngine.ProcessorArchitecture),
                string.Format("{0}\\AudioLib_{1}", AudioEngine.AssemblyDirectory, AudioEngine.ProcessorArchitecture)};
            bool initSuccess = false;
            string initErrorMsg = string.Empty;

            for (int i = 0; i < libPaths.Length && !initSuccess; i++)
            {
                try
                {
                    initSuccess = AudioEngine.Init(libPaths[i], this.Handle);
                }
                catch (Exception x)
                {
                    initErrorMsg = x.Message;
                }                
            }

            if (!initSuccess)
            {
                MBoxHelper.ShowErrorMsg(initErrorMsg, "BASS Init Error!");
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
                proc.Kill();
            }
            openMedia.Filter = AudioEngine.getSupFilter;

            tagForm = new Azure.LibCollection.CS.tagWnd();
            tagForm.BackgroundImage = global::Azure.YAMP.Properties.Resources.background;
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
                    if (!File.Exists(".\\" + _asmName + ".old_config"))
                    {
                        File.Move(".\\" + _asmName + ".exe.config", ".\\" + _asmName + ".old_config");
                    }
                    File.Copy(".\\yamp.settings", ".\\" + _asmName + ".exe.config", true);
                }

                if (!yamp.Default.SeekBarStyle)
                    SetWindowTheme(this.Handle, "", "");
                else
                    Application.EnableVisualStyles();

                this.TopLevel = true;
                this.TopMost = yamp.Default.alwaysOnTop;
                setPinButton();
                this.Location = new Point(yamp.Default.locationMainWindow.X, 
                    yamp.Default.locationMainWindow.Y);

                FireWorks.Init(ref visualBox, GetPaddedRectangle(visualBox), yamp.Default.VisTime);
                FireWorks.SetVisual(yamp.Default.VisualSetting);
                FireWorks.Set("backColor", Color.Transparent);
                FireWorks.Set("baseColor", yamp.Default.VisColorBottom);
                FireWorks.Set("peakColor", yamp.Default.VisColorTop);
                FireWorks.Set("holdColor", yamp.Default.VisColorHold);

                this.seekBar.BarColor = yamp.Default.SeekBarColor;
                tagForm.Visible = yamp.Default.showTagForm;
                if (!yamp.Default.showPLEdit)
                    minimizeWnd();

                if (yamp.Default.UseDigitFont)
                {
                    ChangeToDigitFontOrBack(true);
                }
            }
            
            setToolTips();
            SetVolumeText();
            volumeKnob.Value = yamp.Default.VolumeLevel;
            opacityKnob.Value = (float)(yamp.Default.opacity - 0.3) * 10F;
            loadingFiles = interruptLoading = false;
            loaded = new List<MediaItem>();
            startup = false;
        }

        private void ChangeToDigitFontOrBack(bool use)
        {
            List<Control> _controlsToChangeFont = new List<Control>() { 
                        this.currentTimeLabel, this.mainLabel, this.bitRateLabel, this.sampleRateLabel, this.chanLabel, this.volLabel
                    };
            if (use)
                SetTextFontFromResource("Transponder AOE", "Transpond_ttf", _controlsToChangeFont);
            else
                ChangeFontStyle(_controlsToChangeFont, new FontFamily("Arial Narrow"));
        }

        private void SetTextFontFromResource(string font_name, string resource_name)
        {
            List<Control> _controls = FindControlsByFontName(this, font_name);
            SetTextFontFromResource(font_name, resource_name, _controls);
        }

        private void SetTextFontFromResource(string font_name, string resource_name, List<Control> _controls)
        {
            if (_controls.Count > 0)
            {
                GetPrivateFont(font_name, resource_name);

                int _fontIndex = pfc_names.IndexOf(font_name);

                ChangeFontStyle(_controls, pfc.Families[_fontIndex]);
            }
        }

        private void ChangeFontStyle(List<Control> _controls, FontFamily fontFamily)
        {
            foreach (var _control in _controls)
            {
                Font _oldStyle = _control.Font;
                _control.Font = new Font(fontFamily, _oldStyle.Size, _oldStyle.Style);
            }
        }

        private void GetPrivateFont(string font_name, string resource_name)
        {
            if (pfc == null)
            {
                pfc = new PrivateFontCollection();
                pfc_names = new List<String>();
            }

            if (!pfc_names.Contains(font_name))
            {
                Byte[] fontBytes = Azure.YAMP.Properties.Resources.ResourceManager.GetObject(resource_name) as byte[];
                IntPtr fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
                pfc.AddMemoryFont(fontData, fontBytes.Length);
                pfc_names.Add(font_name);
                Marshal.FreeCoTaskMem(fontData);
            }           
        }

        private List<Control> FindControlsByFontName(Control root, string font_name)
        {
            List<Control> _controls = null;

            if (root != null)
            {
                _controls = new List<Control>();
                foreach (Control child in root.Controls)
                {
                    if (child.Font.Name == font_name) 
                        _controls.Add(child);

                    List<Control> found = FindControlsByFontName(child, font_name);
                    if (found != null)
                    {
                        foreach (var item in found)
                        {
                            _controls.Add(item);             
                        }                                
                    }
                }
            }

            return _controls;
        }
        
        private void openVisSettings(object sender, EventArgs e)
        {
            ChangeVisualSettings();
        }

        private Rectangle GetPaddedRectangle(Control control)
        {
            var rect = control.ClientRectangle;
            var pad = control.Padding;
            return new Rectangle(rect.X + pad.Left,
                                  rect.Y + pad.Top,
                                  rect.Width - (pad.Left + pad.Right),
                                  rect.Height - (pad.Top + pad.Bottom));
        }
        



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
            tp.SetToolTip(visualBox, "Visualiser");
            tp.SetToolTip(plistBtn, "PlayList Window");
            tp.SetToolTip(visBtn, "Visuals Settings");
            tp.SetToolTip(tagfmBtn, "Tag Info Window");
            tp.SetToolTip(volumeKnob, "Adjust volume.");
            tp.SetToolTip(opacityKnob, "Adjust opacity betweem 30% and 100%.");
            helpPlayList = ("To move up (down) selected item, press '+' or 'W'  ('-' or 'S') button.");
            helpSearchBox = ("Type a substring to search, and press F3 to jump to the next occurrence.");
        }

        private void ShutDown(object sender, FormClosingEventArgs fce)
        {
            try
            {
                if ((playListView.Items.Count > 0) && (getSettingsExist))
                {
                    yamp.Default.lastPlayedIndex = (playListView.SelectedIndices.Count > 0 ? playListView.SelectedIndices[0] : 0);
                    yamp.Default.Save();
                    this.savePlayList(startDir + yamp.Default.lastPlayedList);
                }
            }
            catch (Exception x)
            {
                Azure.LibCollection.CS.MBoxHelper.ShowWarnMsg(x, "Warning!");
            }
            finally
            {
                AudioEngine.Clean();
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
                seekBar.Value = CalcSeek((double)maus.X);
            }
        }

        private int CalcSeek(double mouseX)
        {  
            double seek = mouseX/(double)seekBar.Width;
            return (int)((seekSet = seek > 1 ? 1 : seek < 0 ? 0 : seek) * seekBar.Maximum);
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

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(startDir + yamp.Default.lastPlayedList))
                {
                    loadDefaultPlayList(startDir + yamp.Default.lastPlayedList);
                    playList.ChangePlayListSelection(yamp.Default.lastPlayedIndex);
                }
            }
            catch (Exception)
            {
                File.Delete(yamp.Default.lastPlayedList);
            }
            this.blockPanel.Hide();
            this.blockPanel.SendToBack();
        }

        private void plistBtn_Click(object sender, EventArgs e)
        {
            if (this.Width != mainPanel.Width)
                minimizeWnd();

            else
                maximizeWnd();
        }

        private void maximizeWnd()
        {
            
            this.MinimumSize = new Size(withPlWidth, this.MinimumSize.Height);
            this.Width = this.MinimumSize.Width;
            
            //seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 12;
            //fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 36);
            //coverBox.Left = 258;
            yamp.Default.showPLEdit = true;
            yamp.Default.Save();
            mainMenuStrip.Items.Add(searchBox);
        }

        private void minimizeWnd()
        {
            mainMenuStrip.Items.Remove(searchBox);
            this.MinimumSize = new Size(this.mainPanel.Width, this.MinimumSize.Height);
            this.Width = this.MinimumSize.Width;
            
            //seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 6;
            //fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 30);
            //coverBox.Left = 252;
            yamp.Default.showPLEdit = false;
            yamp.Default.Save();
        }

        private void savePlayList(string plPath)
        {
            PlayList.SaveList(plPath, PlayListFormat.yspl, loaded);
        }
        
        private void RemoveLoadingPanel(int i)
        {
            if (i == 0)
            {
                foreach (ListViewItem item in playListView.Items)
                {
                    searchBox.AutoCompleteCustomSource.Add(item.SubItems[1].ToString());
                }
                this.Text = defaultTitle;
                loadingFiles = false;
            }
        }

        private void CreateLoadingPanel(int i)
        {
            if (i == 0)
            {
                if (!loadingFiles)
                {
                    defaultTitle = this.Text;
                    this.Text += " # Loading Files...";
                    loadingFiles = true;
                    interruptLoading = false;
                }
            }
        }

        public void LoadFromDir(string[] directoryName, int counter)
        {
            try
            {
                if (!interruptLoading)
                {
                    CreateLoadingPanel(counter);
                    if (directoryName.Length > 0)
                    {
                        foreach (string item in directoryName)
                        {
                            string[] files;
                            string[] dirs = new string[0];
                            if (File.Exists(item))
                            {
                                files = new string[1];
                                files[0] = item;
                            }
                            else
                            {
                                files = Directory.GetFiles(item);
                                dirs = Directory.GetDirectories(item);
                            }
                            if (dirs.Length > 0)
                            {
                                LoadFromDir(dirs, counter + 1);
                            }

                            //this.LoadMultiFiles(ref files, AudioEngine.getSupFilter, ref loaded, playListView2.Items, ref interruptLoading);
                            PlayList.BuildPlayList(ref files, AudioEngine.getSupFilter, ref loaded, ref interruptLoading);                            
                        }
                    }
                    PlayList.RebuildListView(loaded, playListView.Items);
                    RemoveLoadingPanel(counter);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in FileLoad function: " + ex.Message);
            }
        }

        private void LoadMultiFiles(ref string[] files, string p, ref List<MediaItem> loaded, ListView.ListViewItemCollection listViewItemCollection, ref bool interruptLoading)
        {
            throw new NotImplementedException();
        }

        private void ThreadForPlEdit(Array files)
        {
            //Thread plThread = new Thread(plPTMulti);
            //plThread.Start(files);
            //LoadMultiFiles(files);
        }

        private void playList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void playList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    {
                        RemoveSelected();
                    }
                    break;
                /*
                case Keys.S:
                    {
                        e.Handled = true;
                        SwapSelected(false);
                    }
                    break;
                case Keys.Subtract:
                    SwapSelected(false);
                    break;
                case Keys.W:
                    {
                        e.Handled = true;
                        SwapSelected(true);
                    }
                    break;
                case Keys.Add:
                    SwapSelected(true);
                    break;
                */
                case Keys.Enter:
                    playList_DoubleClick(playListView, null);
                    break;
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
                case Keys.X:
                    {
                        playList_Prev(true);
                        e.Handled = true;
                    } break;
                case Keys.N:
                    {
                        playList_Next(true);
                        e.Handled = true;
                    } break;
                case Keys.J:
                case Keys.F3:
                    {
                        searchBox.Focus();
                    }
                    break;
                default:
                    break;
            }
        }

        public void playList_DoubleClick(object sender, EventArgs e)
        {
            if (playListView.SelectedIndices.Count > 0)
            {
                currentIndex = playListView.SelectedIndices[0];
                this.PlayNewFile(loaded[currentIndex].getFilePath);
            }
        }

        public void playList_Next(bool rePlay)
        {
            if (++currentIndex < loaded.Count)
            {
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                //playList.SelectedIndex = currentIndex;
                playList.ChangePlayListSelection(currentIndex);
            }
            else
            {
                if (rePlay)
                {
                    playList_First();
                }
            }
        }

        public void playList_Prev(bool rePlay)
        {
            if (--currentIndex >= 0)
            {
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                //playList.SelectedIndex = currentIndex;
                playList.ChangePlayListSelection(currentIndex);
            }
            else
            {
                if (rePlay)
                {
                    playList_Last();
                }
            }

        }

        public void playList_First()
        {
            currentIndex = 0;
            if (loaded.Count > 0)
            {
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                //playList.SelectedIndex = currentIndex;
                playList.ChangePlayListSelection(currentIndex);
            }
        }

        public void playList_Last()
        {
            if (loaded.Count > 0)
            {
                currentIndex = loaded.Count - 1;
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                //playList.SelectedIndex = currentIndex;
                playList.ChangePlayListSelection(currentIndex);
            }
        }

        private void playListWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            if (this.getSettingsExist)
            {
                yamp.Default.showPLEdit = false;
                yamp.Default.Save();
            }
        }

        /*
        private void SwapSelected(bool p)
        {
            if (playList.Items.Count > 0)
            {
                int i = 1;
                if (p)
                {
                    i *= (-1);
                }
                int Index = playList.SelectedIndex;          //Selected Index
                object SwapText = playList.SelectedItem;      //Selected Item
                MediaItem SwapMedia = loaded[Index];
                if (Index > -1)
                {               //If something is selected...
                    playList.Items.RemoveAt(Index);                 //Remove it
                    loaded.RemoveAt(Index);
                    if (Index + i > playList.Items.Count)
                    {
                        Index = 0;
                    }
                    else
                    {
                        if (Index + i < 0)
                        {
                            Index = playList.Items.Count;
                        }
                        else
                        {
                            Index += i;
                        }
                    }
                    playList.Items.Insert(Index, SwapText);        //Add it back in one spot up
                    loaded.Insert(Index, SwapMedia);
                    playList.SelectedItem = SwapText;                   //Keep this item selected

                    //correct the index of the currently played file
                    for (int j = 0; j < loaded.Count; j++)
                    {
                        if (loaded[j].getFilePath == currentFile)
                        {
                            currentIndex = j;
                            break;
                        }
                    }

                }
            }
        }
        */

        private void RemoveSelected()
        {
            if (playListView.SelectedIndices.Count > 0)
            {
                Array _selectedI = Array.CreateInstance(typeof(int), playListView.SelectedIndices.Count);
                playListView.SelectedIndices.CopyTo(_selectedI,0);
                Array.Reverse(_selectedI);

                foreach (int i in _selectedI)
                {
                    loaded.RemoveAt(i);
                    searchBox.AutoCompleteCustomSource.Remove(playListView.Items[i].SubItems[1].ToString());
                }
                foreach (ListViewItem listViewItem in playListView.SelectedItems)
                {
                    listViewItem.Remove();
                }
                this.RebuildLoaded();
            }
        }

        private void ResetList()
        {
            //playList.Items.Clear();
            playListView.Items.Clear();
            searchBox.AutoCompleteCustomSource.Clear();
            loaded.Clear();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog loadFiles = new OpenFileDialog())
            {
                loadFiles.Filter = this.openMedia.Filter;
                loadFiles.Multiselect = true;
                if (loadFiles.ShowDialog() == DialogResult.OK)
                {
                    LoadFromDir(loadFiles.FileNames, 0);
                }
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog chooseFolder = new FolderBrowserDialog())
            {
                if (this.getSettingsExist)
                {
                    chooseFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
                    chooseFolder.SelectedPath = yamp.Default.lastFolder;
                    chooseFolder.RootFolder = System.Environment.SpecialFolder.Desktop;

                }
                if (chooseFolder.ShowDialog() == DialogResult.OK)
                {
                    LoadFromDir(new string[] { chooseFolder.SelectedPath }, 0);
                    if (this.getSettingsExist)
                    {
                        yamp.Default.lastFolder = chooseFolder.SelectedPath;
                        yamp.Default.Save();
                    }
                }
            }

        }

        private void playList_MouseEnter(object sender, EventArgs e)
        {
            hintLabel.Text = helpPlayList;
            hintLabel.Visible = true;
        }

        private void playList_MouseLeave(object sender, EventArgs e)
        {
            HideHintLabel();
        }

        private void HideHintLabel()
        {
            hintLabel.Text = "";
            hintLabel.Visible = false;
        }

        private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playListView.SelectedItems.Count > 0)
            {
                RemoveSelected();
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetList();
        }

        public void LoadSingleFile(string file)
        {
            ResetList();
            string[] _file = new string[] { file };
            this.LoadMultiFiles(ref _file, AudioEngine.getSupFilter, ref loaded, playListView.Items, ref interruptLoading);
        }

        private void searchBox_Click(object sender, EventArgs e)
        {
            searchBox.SelectAll();
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    searchFile();
                    break;
                case Keys.Enter:
                    playList_DoubleClick(playListView, null);
                    break;
                default:
                    break;
            }
        }

        private void playListWindow_ResizeEnd(object sender, EventArgs e)
        {
            if (this.getSettingsExist)
            {
                yamp.Default.sizePLEdit = new Point(this.Size.Width, this.Size.Height);
                yamp.Default.Save();
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text.Length > 0)
            {
                searchIndex = 0;
                searchFile();
            }
        }

        private void searchBox_MouseEnter(object sender, EventArgs e)
        {
            hintLabel.Text = helpSearchBox;
            hintLabel.Visible = true;
        }

        private void searchBox_MouseLeave(object sender, EventArgs e)
        {
            HideHintLabel();
        }

        private void playList_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "YAMP Simplified PlayList|*.yspl|M3U PlayList|*.m3u;*.m3u8";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PlayListFormat format;
                FileInfo fInfo = new FileInfo(sfd.FileName);
                switch (fInfo.Extension)
                {
                    case ".m3u":
                        format = PlayListFormat.m3u;
                        break;
                    case ".m3u8":
                        format = PlayListFormat.m3u8;
                        break;
                    case ".yspl":
                        format = PlayListFormat.yspl;
                        break;
                    default:
                        format = PlayListFormat.unsupported;
                        break;
                }
                //this.savePlayList(sfd.FileName, format, loaded);
                PlayList.SaveList(sfd.FileName, format, loaded);
            }
        }



        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "YAMP Simplified PlayList|*.yspl|M3U PlayList|*.m3u;*.m3u8";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fInfo = new FileInfo(ofd.FileName);
                    PlayListFormat format = PlayListFormat.unsupported;
                    switch (fInfo.Extension)
                    {
                        case ".yspl":
                            format = PlayListFormat.yspl;
                            break;
                        case ".m3u":
                            format = PlayListFormat.m3u;
                            break;
                        case ".m3u8":
                            format = PlayListFormat.m3u8;
                            break;
                        default:
                            break;
                    }
                    interruptLoading = true;
                    CreateLoadingPanel(0);
                    this.loadPlayList(ofd.FileName, AudioEngine.getSupFilter,
                        format, ref loaded, playListView.Items, ref interruptLoading);
                    RemoveLoadingPanel(0);
                }
            }
        }

        private void loadPlayList(string path, string supFilters, PlayListFormat format, ref List<MediaItem> loaded, ListView.ListViewItemCollection listViewItemCollection, ref bool interruptLoading)
        {
            PlayList.LoadList(path, supFilters, format, ref loaded, ref interruptLoading);
            PlayList.RebuildListView(loaded, listViewItemCollection);
        }

        internal void loadDefaultPlayList(string plPath)
        {
            CreateLoadingPanel(0);
            this.loadPlayList(plPath, AudioEngine.getSupFilter,
                    PlayListFormat.yspl, ref loaded, playListView.Items, ref interruptLoading);
            RemoveLoadingPanel(0);
        }

        public bool LoadingFiles
        {
            get { return loadingFiles; }
        }

        public bool InterruptLoad
        {
            get { return interruptLoading; }
            set { interruptLoading = value; }
        }

        private void PlayNewFile(string openThis)
        {
            currentFile = openThis;
            coverBox.Image = null;
            AudioEngine.Stop();
            //AudioEngine.Wipe();
            FireWorks.Set("drawFull", false);
            AudioEngine.PlayInitFile(openThis.ToLower());
            if (eq == null)
                eq = new EqualiserEngine(AudioEngine.GetStreamHandler(), 60f, 170f,
                    310f, 600f, 1000f, 3000f, 6000f, 12000f, 14000f, 16000f);
            else
                eq.AttachEQ(AudioEngine.GetStreamHandler());
            UpdateEqLevels();
            setInfoControls();
            seekBar.Maximum = (int)Math.Round(AudioEngine.getCurrentLengthSeconds(), MidpointRounding.AwayFromZero);
            drawFileInfo(openThis);
            playStarted = true;
            mainTimer.Enabled = true;
            //scrollTimer.Enabled = true;
            mainLabel.Switch(true);
            InitTagsForDisplay(openThis);
        }

        private bool setInfoControls()
        {
            try
            {
                Dictionary<string, object> _metaInfo = loaded[currentIndex].UpdateMetaInfo();
                sampleRateLabel.Text = (Convert.ToDouble(_metaInfo["AudioSampleRate"]) / 1000).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                bitRateLabel.Text = _metaInfo["AudioBitrate"].ToString();
                string _channels = AudioEngine.ChannelGetInfo("chanStr");
                chanLabel.Text = (_channels == "Stereo" ? "2.0" : (_channels == "Mono" ? "1.0" : _channels));
                artistBox.Text = _metaInfo["Performers"].ToString();
                titleBox.Text = _metaInfo["Title"].ToString();
                albumBox.Text = _metaInfo["Album"].ToString();

                ListViewItem _lvi = playListView.Items[currentIndex];
                _lvi.SubItems[1].Text = _metaInfo["FileMeta"].ToString();
                _lvi.SubItems[2].Text = _metaInfo["Duration"].ToString();
                _lvi.SubItems[5].Text = _metaInfo["Performers"].ToString();
                _lvi.SubItems[6].Text = _metaInfo["Title"].ToString();
                _lvi.SubItems[7].Text = _metaInfo["Album"].ToString();

                mainLabel.Text = " *** " + _metaInfo["FileMeta"].ToString();
                mainLabel.DuplicateIf();
                coverBox.Image = (_metaInfo["CoverArt"] as Image);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void drawFileInfo(string openThis)
        {
            fileBox.Text = Path.GetFileName(openThis);

        }

        private void InitTagsForDisplay(string openThis)
        {
            tagForm.LoadTags(openThis);
        }

        private void searchFile()
        {
            if (loaded.Count > 0)
            {
                int i;
                for (i = searchIndex; i < loaded.Count; ++i)
                {
                    //if (playList.Items[i].ToString().ToLower().Contains(searchBox.Text.ToLower()))
                    if (playListView.Items[i].SubItems[1].ToString().ToLower().Contains(searchBox.Text.ToLower()))
                    {
                        //searchIndex = playList.SelectedIndex = i;
                        playListView.SelectedIndices.Clear();
                        playListView.Items[i].Selected = true;
                        searchIndex = ++i;
                        break;
                    }
                    else
                    {
                        //playList.SelectedIndex = -1;
                        playListView.SelectedIndices.Clear();
                    }

                }
                if (i >= loaded.Count)
                {
                    i = searchIndex = 0;
                }
            }
        }
        
        private void StartSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (AudioEngine.getStatus() > 0))
            {
                seeking = true;
                seekBar.Value = CalcSeek((double)me.X);
            }
        }

        private void EndSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (seeking))
            {                
                AudioEngine.setPositionSeconds(AudioEngine.getCurrentLengthSeconds() * seekSet);
                seeking = false;
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;
            //scrollTimer.Enabled = false;
            if (openMedia.ShowDialog() == DialogResult.OK)
            {
                currentFile = openMedia.FileName;
                LoadSingleFile(currentFile);
                PlayNewFile(currentFile);
            }
            mainTimer.Enabled = true;
            //scrollTimer.Enabled = true;
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            switch (AudioEngine.getStatus())
            {

                case 1:
                    {
                        currentTimeLabel.Text = AudioEngine.CurrentPositionString();
                        if (!seeking)
                        {
                            seekBar.Value = (int)AudioEngine.getCurrentPositionSeconds();                            
                        }
                        
                    }
                    break;
                case 0:
                    {
                        mainTimer.Enabled = false;
                        NextPlayState();
                    }
                    break;
            }
        }

        private void NextPlayState()
        {
            if (playStarted)
            {
                try
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
                    if (playStarted)
                        mainTimer.Enabled = true;
                }
                catch (Exception x)
                {
                    playStarted = false;
                    MBoxHelper.ShowErrorMsg(x, "Error occured via playlist!");
                }
            }
        }

        private void PlayCurrentFile()
        {
            AudioEngine.Play();
        }

        private void RepeatAll()
        {
            if (loaded.Count == 1)
                PlayCurrentFile();
            else
            {
                if (loaded.Count == this.currentIndex)
                {
                    playList_First();
                }
                else
                    playList_Next(false);
            }
        }

        private void NoRepeat()
        {
            if ((loaded.Count > 1) && (loaded.Count > currentIndex))
                playList_Next(false);
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
            AudioEngine.Stop();
            seekBar.Value = seekBar.Minimum;
            sampleRateLabel.Text = bitRateLabel.Text = currentTimeLabel.Text =
                chanLabel.Text = artistBox.Text = titleBox.Text = albumBox.Text = "";
            visualBox.Image = coverBox.Image = null;
            mainTimer.Enabled = false;
            mainLabel.Switch(false);
            mainLabelToDefault();
        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            AudioEngine.Pause();
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (currentFile != null)
            {
                PlayNewFile(currentFile);
                playList.ChangePlayListSelection(currentIndex);
            }
            else if (currentIndex != -1)
            {
                PlayNewFile(loaded[currentIndex].getFilePath);
                playList.ChangePlayListSelection(currentIndex);
            }
            else
            {
                if (loaded.Count > 0)
                {
                    playList_DoubleClick(playListView, null);
                }
            }
        }

        private void ModifyRepeatOption()
        {
            switch (repeat)
            {
                case repeatOption.none:
                    {

                        repeatCheckBox.ForeColor = Color.FromArgb(128, 204, 128);
                        repeat = repeatOption.actual;
                        repeatCheckBox.Text = "Repeat one";
                    }
                    break;
                case repeatOption.actual:
                    {
                        repeatCheckBox.ForeColor = Color.Goldenrod;
                        repeat = repeatOption.all;
                        repeatCheckBox.Text = "Repeat all";                        
                    }
                    break;
                case repeatOption.all:
                    {
                        repeatCheckBox.ForeColor = Color.FromArgb(192,204,192);
                        repeat = repeatOption.none;
                        repeatCheckBox.Text = "Repeat off";
                    }
                    break;
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            playList_Next(true);
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            playList_Prev(true);
        }

        public bool getSettingsExist
        {
            get
            {
                return (File.Exists(".\\yamp.settings") | File.Exists(".\\" + _asmName + ".exe.config"));
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

        private void visualBox_Click(object sender, EventArgs e)
        {
            FireWorks.SetVisual(FireWorks.GetVSType + 1);
            if (getSettingsExist)
            {
                yamp.Default.VisualSetting = (byte)FireWorks.GetVSType;
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
            return AudioEngine.getSupFilter;
        }

        private void minBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void topSwitchBtn_Click(object sender, EventArgs e)
        {
            SwitchTopMost();
        }

        private void SwitchTopMost()
        {
            this.TopMost = (!this.TopMost);
            if (getSettingsExist)
            {
                yamp.Default.alwaysOnTop = this.TopMost;
                yamp.Default.Save();
            }
            setPinButton();
        }

        private void setPinButton()
        {
            switch (this.TopMost)
            {
                case true: pinMenuItem.Image = Azure.YAMP.Properties.Resources.pinOn; break;
                case false: pinMenuItem.Image = Azure.YAMP.Properties.Resources.pinOff; break;
                default:
                    break;
            }
        }

        private void ChangeVisualSettings()
        {
            visSetWnd visSelect = new visSetWnd((byte)FireWorks.GetVSType, FireWorks.GetTimerInterval(),
                new Color[] {FireWorks.GetColor("base"), FireWorks.GetColor("peak"), 
                    FireWorks.GetColor("hold"), this.seekBar.BarColor}, yamp.Default.UseDigitFont);
            if (visSelect.ShowDialog() == DialogResult.OK)
            {
                FireWorks.SetVisual(visSelect.visMode);
                FireWorks.Set("timerInterval", visSelect.visTime);
                FireWorks.Set("baseColor", visSelect.Controls["btmColorBtn"].BackColor);
                FireWorks.Set("peakColor", visSelect.Controls["topColorBtn"].BackColor);
                FireWorks.Set("holdColor", visSelect.Controls["peakColorBtn"].BackColor);
                this.seekBar.BarColor = visSelect.Controls["seekColorBtn"].BackColor;

                bool useDigitFont = (visSelect.Controls["useDigitFontCB"] as CheckBox).Checked;

                if (yamp.Default.UseDigitFont != useDigitFont)
                {
                    ChangeToDigitFontOrBack(useDigitFont);
                }


                if (this.getSettingsExist)
                {
                    yamp.Default.VisualSetting = visSelect.visMode;
                    yamp.Default.VisTime = FireWorks.GetTimerInterval();
                    yamp.Default.VisColorBottom = FireWorks.GetColor("base");
                    yamp.Default.VisColorTop = FireWorks.GetColor("peak");
                    yamp.Default.VisColorHold = FireWorks.GetColor("hold");
                    yamp.Default.SeekBarColor = this.seekBar.BarColor;
                    yamp.Default.UseDigitFont = useDigitFont;
                    yamp.Default.Save();
                }
            }
        }

        private void SetVolume(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            AudioEngine.channelVolume = volumeKnob.Value;
            SetVolumeText();
            if (!startup && getSettingsExist)
            {
                yamp.Default.VolumeLevel = volumeKnob.Value;
                yamp.Default.Save();
            }
        }

        private void SetVolumeText()
        {
            volLabel.Text = ((int)Math.Round(AudioEngine.channelVolume * 100, MidpointRounding.AwayFromZero)).ToString() + "%";
        }        

        private void SetOpacity(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            tagForm.Opacity = this.Opacity = 0.3F + ((sender as LBSoft.IndustrialCtrls.Knobs.LBKnob).Value / 10F);
            if (!startup && getSettingsExist)
            {
                yamp.Default.opacity = (float)this.Opacity;
                yamp.Default.Save();
            }
        }

        private void visualBox_Paint(object sender, PaintEventArgs e)
        {
            MakeTransparentVisual();

        }

        private void MakeTransparentVisual()
        {
            Image image = (visualBox.Image != null ? new Bitmap(visualBox.Image.Size.Width, visualBox.Image.Size.Height, PixelFormat.Format32bppArgb)
                : new Bitmap(visualBox.ClientRectangle.Width, visualBox.ClientRectangle.Height, PixelFormat.Format32bppArgb));
            using (var g = Graphics.FromImage(image))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(image, 0, 0);
            }
        }

        private void visualBox_Validated(object sender, EventArgs e)
        {
            MakeTransparentVisual();
        }
        
        private void repeatCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            ModifyRepeatOption();
        }

        private void pinMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            SwitchTopMost();
        }

        private void UpdateEqLevels()
        {
            if (eq != null)
            {
                eq.UpdateFX(0, (float)freqBar0.Value);
                eq.UpdateFX(1, (float)freqBar1.Value);
                eq.UpdateFX(2, (float)freqBar2.Value);
                eq.UpdateFX(3, (float)freqBar3.Value);
                eq.UpdateFX(4, (float)freqBar4.Value);
                eq.UpdateFX(5, (float)freqBar5.Value);
                eq.UpdateFX(6, (float)freqBar6.Value);
                eq.UpdateFX(7, (float)freqBar7.Value);
                eq.UpdateFX(8, (float)freqBar8.Value);
                eq.UpdateFX(9, (float)freqBar9.Value);
            }
        }

        private void freqBar_ValueChanged(object sender, EventArgs e)
        {
            int _band = int.Parse((sender as FusionTrackBar).Name.Substring(7,1));
            float _gain = (float)(sender as FusionTrackBar).Value;
            if(eq != null)
                eq.UpdateFX(_band, _gain);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (eq != null)
                eq.Reset();
            freqBar0.Value = 0;
            freqBar1.Value = 0;
            freqBar2.Value = 0;
            freqBar3.Value = 0;
            freqBar4.Value = 0;
            freqBar5.Value = 0;
            freqBar6.Value = 0;
            freqBar7.Value = 0;
            freqBar8.Value = 0;
            freqBar9.Value = 0;
        }

        private void freqLabel_Click(object sender, EventArgs e)
        {
            string _controlName = (sender as Label).Name.Replace("Label", "Bar");
            FusionTrackBar _ft = (eqPanel.Controls.Find(_controlName, false)[0]) as FusionTrackBar;
            _ft.Value = 0;
        }

        private void visBtn_Click(object sender, EventArgs e)
        {
            ChangeVisualSettings();
        }

        internal void PlayNewFileOnDoubleClick()
        {
            this.PlayNewFile(loaded[currentIndex].getFilePath);
        }

        public void playList_DragDrop(object sender, DragEventArgs e)
        {
            LoadFromDir((string[])e.Data.GetData(DataFormats.FileDrop), 0);
        }

        internal void RebuildLoaded()
        {
            int i = 1;
            List<MediaItem> _newLoaded = new List<MediaItem>();

            foreach (ListViewItem item in playListView.Items)
            {                
                if (this.currentFile == item.SubItems[4].Text)
                    this.currentIndex = i - 1;
                item.SubItems[0].Text = i.ToString();
                ++i;
                _newLoaded.Add(MediaItem.Factory(item.SubItems[4].Text));
            }

            loaded.Clear();
            loaded = _newLoaded;
        }

        

        /*public void loadPlayList(string FileName, string supExt, PlayListFormat format, ref List<MediaItem> lContainer, ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            StreamReader srr = File.OpenText(FileName);
            Regex headText = null;
            switch (format)
            {
                case PlayListFormat.yspl:
                    headText = new Regex("#YAMP Simplified PlayList#");
                    break;
                case PlayListFormat.m3u:
                case PlayListFormat.m3u8:
                    headText = new Regex("#*M3U");
                    break;
                case PlayListFormat.unsupported:
                    break;
            }
            if (headText.IsMatch(srr.ReadLine()))
            {
                lContainer.Clear();
                //lCollection.Clear();
                lviCollection.Clear();

                string readIn;
                if (format == PlayListFormat.yspl)
                {
                    readIn = srr.ReadLine();
                }
                //bool ok = true;
                do
                {
                    readIn = srr.ReadLine();
                    if (readIn != null)
                    {
                        if (!readIn.Contains("#End ... Written "))
                        {
                            if (!readIn.Contains("#"))
                            {
                                if (readIn.StartsWith(@"\"))
                                {
                                    readIn = Environment.GetEnvironmentVariable("%SYSTEMDRIVE%",
                                        EnvironmentVariableTarget.Machine) + readIn;
                                }
                                string[] _file = new string[] { readIn };
                                LoadMultiFiles(ref _file, supExt, ref lContainer, lviCollection, ref interrupt);
                            }
                        }
                        else
                            break;
                    }
                    else
                    {
                        break;
                        //ok = false;
                    }
                }
                while (true); //(ok);
            }
            else
            {
                MBoxHelper.ShowWarnMsg("Not a valid YAMP PlayList!",
                    "Error on playlist load!");
            }
            srr.Close();
        }
        
        public bool LoadMultiFiles(ref string[] files, string supExt, ref List<MediaItem> Container, ListView.ListViewItemCollection ListViewCollection, ref bool interrupt)
        {
            try
            {
                if (files != null)
                {
                    foreach (string aElement in files)
                    {
                        if (!interrupt)
                        {
                            if (File.Exists(aElement))
                            {
                                string fileEnding = new FileInfo(aElement.ToLower()).Extension;
                                if (supExt.Contains(fileEnding))
                                {
                                    MediaItem mItem = MediaItem.Factory(aElement);

                                    if (TagReader.fileHasValidTags(mItem.getFilePath))
                                    {
                                        Container.Add(mItem);
                                        //Collection.Add(mItem.getFileName);

                                        string _performers = mItem.MetaInfo("get", "Performers").ToString();
                                        string _title = mItem.MetaInfo("get", "Title").ToString();
                                        string _album = mItem.MetaInfo("get", "Album").ToString();
                                        string _fileMeta = mItem.MetaInfo("get", "FileMeta").ToString();
                                        string _duration = mItem.MetaInfo("get", "Duration").ToString();
                                        string _fileName = mItem.getFileName;
                                        string _filePath = mItem.getFilePath;

                                        string[] _lviData =  
                                        { 
                                            (ListViewCollection.Count +1).ToString(), 
                                            _fileMeta,
                                            _duration,
                                            _fileName,
                                            _filePath,
                                            _performers,
                                            _title,
                                            _album
                                        };

                                        ListViewItem _lvi = new ListViewItem(_lviData);
                                        ListViewCollection.Add(_lvi);
                                    }
                                }
                            }
                            Application.DoEvents();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal void LoadMultiFiles(ref Array files, string p, ref List<MediaItem> loaded, ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            string[] _files = (string[])files;
            LoadMultiFiles(ref _files, p, ref loaded, lviCollection, ref interrupt);
        }
         */
    }
}
