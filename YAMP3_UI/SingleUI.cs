using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Azure.LibCollection.CS;

namespace Azure.YAMP
{
    public partial class SimpleUI : Form
    {
        public SimpleUI()
        {
            startDir = System.IO.Directory.GetCurrentDirectory()+ "\\";
            InitializeComponent();
            
            InitializeIcons(titIco, Azure.YAMP.Properties.Resources.TitleIcon, Color.FromArgb(128,128,128));
            InitializeIcons(artIco, Azure.YAMP.Properties.Resources.ArtistIcon, Color.FromArgb(128, 128, 128));
            InitializeIcons(albIco, Azure.YAMP.Properties.Resources.AlbumIcon, Color.FromArgb(128, 128, 128));
            mainLabelToDefault();
            playStarted = false;

            if (!AudioEngine.Init(string.Format("{0}\\AudioLib_{1}", AudioEngine.AssemblyDirectory, AudioEngine.ProcessorArchitecture), this.Handle))
            {
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
                //this.Opacity = yamp.Default.opacity;
                if (!yamp.Default.showPLEdit)
                    minimizeWnd();
            }            
            setToolTips();
            SetVolumeText();
            volumeKnob.Value = AudioEngine.masterVolume;

            opacityKnob.Value = (float)(yamp.Default.opacity - 0.3) * 10F;

            loadingFiles = interruptLoading = false;
            loaded = new List<MediaItem>();

            playListView.ListViewItemSorter = new ListViewItemComparer();
            playListView.Sorting = SortOrder.Ascending;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

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
        private int currentIndex, searchIndex;
        private string helpPlayList, helpSearchBox;
        private bool dragHappened;
        private const int cGrip = 16;

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
                    yamp.Default.lastPlayedIndex = playListView.SelectedIndices[0];
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
                MessBroadcast.StopMSNBroadCast();
                TagReader.Clean();
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
                    ChangePlayListSelection(yamp.Default.lastPlayedIndex);
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
            this.MinimumSize = new Size((this.playListView.Location.X + this.playListView.MinimumSize.Width) + 4, this.MinimumSize.Height);
            this.Width = this.MinimumSize.Width;
            
            //seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 12;
            //fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 36);
            //coverBox.Left = 258;
            yamp.Default.showPLEdit = true;
            yamp.Default.Save();
        }

        private void minimizeWnd()
        {
            this.MinimumSize = new Size(this.mainSkin.Width, this.MinimumSize.Height);
            this.Width = this.MinimumSize.Width;
            
            //seekBar.Left = titIco.Left = artIco.Left = albIco.Left = fileLabel.Left = 6;
            //fileBox.Left = 2 + (titleBox.Left = artistBox.Left = albumBox.Left = 30);
            //coverBox.Left = 252;
            yamp.Default.showPLEdit = false;
            yamp.Default.Save();
        }

        private void playListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hit = playListView.HitTest(e.Location);
            if (hit.Item != null)
            {
                currentIndex = playListView.Items.IndexOf(hit.Item);
                this.PlayNewFile(loaded[currentIndex].getFilePath);
            };
        }


        private void playListView_DragDrop(object sender, DragEventArgs e)
        {
            string[] _droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            if (_droppedFiles.Length < 1)
                dragHappened = true;
            else
            {
                playListView.SelectedItems.Clear();
                playListView.SelectedIndices.Clear();
                LoadFromDir(_droppedFiles, 0);
                SortPlayListByIndexColumn();
            }
        }

        private void SortPlayListByIndexColumn()
        {
            playListView.Sort();

            ChangePlayListSelection(currentIndex);
        }

        private void playListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragHappened)
            {
                int i = 1;
                List<MediaItem> _newLoaded = new List<MediaItem>();
                
                foreach (ListViewItem item in playListView.Items)
                {
                    if (currentFile == item.SubItems[4].Text)
                        currentIndex = i - 1;
                    item.SubItems[0].Text = i.ToString();
                    ++i;
                    _newLoaded.Add(MediaItem.Factory(item.SubItems[4].Text));
                }
                
                loaded.Clear();
                loaded = _newLoaded;
                dragHappened = false;
            }
        }

        private void savePlayList(string plPath)
        {
            PLParserClassic.savePlayList(plPath, PLParserClassic.PlFormat.yspl, loaded);
        }


        public void playList_DragDrop(object sender, DragEventArgs e)
        {
            LoadFromDir((string[])e.Data.GetData(DataFormats.FileDrop), 0);
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

        private void LoadFromDir(string[] directoryName, int counter)
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

                            PLParserClassic.LoadMultiFiles(ref files, AudioEngine.getSupFilter,
                                ref loaded, playListView.Items, ref interruptLoading);
                        }
                    }
                    RemoveLoadingPanel(counter);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in FileLoad function: " + ex.Message);
            }
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
            if (playListView.SelectedItems.Count > 0)
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
                ChangePlayListSelection(currentIndex);
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
                ChangePlayListSelection(currentIndex);
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
                ChangePlayListSelection(currentIndex);
            }
        }

        public void playList_Last()
        {
            if (loaded.Count > 0)
            {
                currentIndex = loaded.Count - 1;
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                //playList.SelectedIndex = currentIndex;
                ChangePlayListSelection(currentIndex);
            }
        }

        private void ChangePlayListSelection(int index)
        {
            playListView.SelectedItems.Clear();
            playListView.SelectedIndices.Clear();
            playListView.Items[index].Selected = true;
            playListView.Items[index].Focused = true;
            playListView.Select();
            playListView.EnsureVisible(index);
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
            //LoadMultiFiles(new string[] { file });
            string[] _file = new string[] { file };
            PLParserClassic.LoadMultiFiles(ref _file, AudioEngine.getSupFilter,
                ref loaded, playListView.Items, ref interruptLoading);
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
                PLParserClassic.PlFormat format;
                FileInfo fInfo = new FileInfo(sfd.FileName);
                switch (fInfo.Extension)
                {
                    case ".m3u":
                        format = PLParserClassic.PlFormat.m3u;
                        break;
                    case ".m3u8":
                        format = PLParserClassic.PlFormat.m3u8;
                        break;
                    case ".yspl":
                        format = PLParserClassic.PlFormat.yspl;
                        break;
                    default:
                        format = PLParserClassic.PlFormat.unsupported;
                        break;
                }
                PLParserClassic.savePlayList(sfd.FileName, format, loaded);
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
                    PLParserClassic.PlFormat format = PLParserClassic.PlFormat.unsupported;
                    switch (fInfo.Extension)
                    {
                        case ".yspl":
                            format = PLParserClassic.PlFormat.yspl;
                            break;
                        case ".m3u":
                            format = PLParserClassic.PlFormat.m3u;
                            break;
                        case ".m3u8":
                            format = PLParserClassic.PlFormat.m3u8;
                            break;
                        default:
                            break;
                    }
                    interruptLoading = true;
                    CreateLoadingPanel(0);
                    PLParserClassic.loadPlayList(ofd.FileName, AudioEngine.getSupFilter,
                        format, ref loaded, playListView.Items, ref interruptLoading);
                    RemoveLoadingPanel(0);
                }
            }
        }

        internal void loadDefaultPlayList(string plPath)
        {
            CreateLoadingPanel(0);
            PLParserClassic.loadPlayList(plPath, AudioEngine.getSupFilter,
                    PLParserClassic.PlFormat.yspl, ref loaded, playListView.Items, ref interruptLoading);
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
            MessBroadcast.StopMSNBroadCast();
            if (TagReader.Init(openThis))
            {
                currentFile = openThis;
                coverBox.Image = null;
                setInfoControls();
                AudioEngine.Stop();
                AudioEngine.Wipe();
                FireWorks.Set("drawFull", false);
                AudioEngine.PlayInitFile(openThis.ToLower());
                seekBar.Maximum = (int)Math.Round(AudioEngine.getCurrentLengthSeconds(), MidpointRounding.AwayFromZero);
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

        private void drawFileInfo(string openThis)
        {
            fileBox.Text = Path.GetFileName(openThis);

        }

        private void InitTagsForDisplay(string openThis)
        {
            tagForm.LoadTags(openThis);
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
                mainTimer.Enabled = !(seeking = true);
                seekBar.Value = CalcSeek((double)me.X);
            }
        }

        private void EndSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (seeking))
            {
                mainTimer.Enabled = !(seeking = false);
                AudioEngine.setPositionSeconds(AudioEngine.getCurrentLengthSeconds() * seekSet);
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            mainTimer.Enabled = false;
            if (openMedia.ShowDialog() == DialogResult.OK)
            {
                currentFile = openMedia.FileName;
                LoadSingleFile(currentFile);
                PlayNewFile(currentFile);
            }
            mainTimer.Enabled = true;
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            switch (AudioEngine.getStatus())
            {

                case 1:
                    {
                        currentTimeLabel.Text = "Now : " + AudioEngine.CurrentPositionString();
                        seekBar.Value = (int)AudioEngine.getCurrentPositionSeconds();
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
            AudioEngine.Pause();
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (currentFile != null)
            {
                PlayNewFile(currentFile);
                ChangePlayListSelection(currentIndex);
            }
            else if (currentIndex != -1)
            {
                PlayNewFile(loaded[currentIndex].getFilePath);
                ChangePlayListSelection(currentIndex);
            }
            else
            {
                if (loaded.Count > 0)
                {
                    playList_DoubleClick(playListView, null);
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
                        repLabel.Text = "Repeat all";
                    }
                    break;
                case repeatOption.all:
                    {
                        repLabel.ForeColor = Color.SkyBlue;
                        repeat = repeatOption.actual;
                        repLabel.Text = "Repeat one";
                    }
                    break;
                case repeatOption.actual:
                    {
                        repLabel.ForeColor = Color.Gray;
                        repeat = repeatOption.none;
                        repLabel.Text = "Repeat off";
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
            this.TopMost = (!this.TopMost);
            if (getSettingsExist)
            {
                yamp.Default.alwaysOnTop = this.TopMost;
                yamp.Default.Save();
            }
        }

        private void colorSetBtn_Click(object sender, EventArgs e)
        {
            visSetWnd visSelect = new visSetWnd((byte)FireWorks.GetVSType, FireWorks.GetTimerInterval(),
                new Color[] {FireWorks.GetColor("base"), FireWorks.GetColor("peak"), 
                    FireWorks.GetColor("hold"), this.seekBar.ForeColor}, false);
            if (visSelect.ShowDialog() == DialogResult.OK)
            {
                FireWorks.SetVisual(visSelect.visMode);
                FireWorks.Set("timerInterval", visSelect.visTime);
                FireWorks.Set("baseColor", visSelect.Controls["btmColorBtn"].BackColor);
                FireWorks.Set("peakColor", visSelect.Controls["topColorBtn"].BackColor);
                FireWorks.Set("holdColor", visSelect.Controls["peakColorBtn"].BackColor);
                this.seekBar.ForeColor = visSelect.Controls["seekColorBtn"].BackColor;

                if (this.getSettingsExist)
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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);

                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void SetVolume(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            AudioEngine.masterVolume = (volumeKnob as LBSoft.IndustrialCtrls.Knobs.LBKnob).Value;
            SetVolumeText();
        }

        private void SetVolumeText()
        {
            volLabel.Text = "Volume: " + ((int)Math.Round(AudioEngine.masterVolume * 100, MidpointRounding.AwayFromZero)).ToString() + "%";
        }

        private void playListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void SetOpacity(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            tagForm.Opacity = this.Opacity = 0.3F + ((sender as LBSoft.IndustrialCtrls.Knobs.LBKnob).Value / 10F);
            if (getSettingsExist)
            {
                yamp.Default.opacity = (float)this.Opacity;
                yamp.Default.Save();
            }
        }
    }    
}
