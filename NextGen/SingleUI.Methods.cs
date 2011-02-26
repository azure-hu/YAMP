using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using libZoi;
using System.IO;
using System.Diagnostics;

namespace YAMP
{
    public partial class SingleUI : Form
    {
        private void savePlayList(string plPath)
        {
            PLParser.savePlayList(plPath, PLParser.PlFormat.yspl, loaded);
        }


        public void playList_DragDrop(object sender, DragEventArgs e)
        {
            LoadFromDir((string[])e.Data.GetData(DataFormats.FileDrop), 0);
        }

        private void RemoveLoadingPanel(int i)
        {
            if (i == 0)
            {
                foreach (var item in playList.Items)
                {
                    searchBox.AutoCompleteCustomSource.Add(item.ToString());
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

        private void LoadToPlEdit()
        {
            playList.Items.Clear();
            searchBox.AutoCompleteCustomSource.Clear();
            foreach (MediaItem item in loaded)
            {
                playList.Items.Add(item.getFileName);
                searchBox.AutoCompleteCustomSource.Add(item.getFileName);
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

                            PLParser.LoadMultiFiles(ref files, BassEngine.getSupFilter,
                                ref loaded, playList.Items, ref interruptLoading);
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
                case Keys.Enter:
                    playList_DoubleClick(playList, null);
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
            if (playList.SelectedItem != null)
            {
                currentIndex = playList.SelectedIndex;
                this.PlayNewFile(loaded[currentIndex].getFilePath);
            }
        }

        public void playList_Next(bool rePlay)
        {
            if (++currentIndex < loaded.Count)
            {
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                playList.SelectedIndex = currentIndex;
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
                playList.SelectedIndex = currentIndex;
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
                playList.SelectedIndex = currentIndex;
            }
        }

        public void playList_Last()
        {
            if (loaded.Count > 0)
            {
                currentIndex = loaded.Count - 1;
                this.PlayNewFile(loaded[currentIndex].getFilePath);
                playList.SelectedIndex = currentIndex;
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
                }
            }
        }

        private void RemoveSelected()
        {
            if (playList.SelectedIndex >= 0)
            {
                loaded.Remove(loaded[playList.SelectedIndex]);
                searchBox.AutoCompleteCustomSource.Remove(playList.SelectedItem.ToString());
                playList.Items.Remove(playList.SelectedItem);
            }
        }

        private void ResetList()
        {
            playList.Items.Clear();
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
            if (playList.SelectedItems.Count > 0)
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
            PLParser.LoadMultiFiles(ref _file, BassEngine.getSupFilter,
                ref loaded, playList.Items, ref interruptLoading);
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
                    playList_DoubleClick(playList, null);
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
                PLParser.PlFormat format;
                FileInfo fInfo = new FileInfo(sfd.FileName);
                switch (fInfo.Extension)
                {
                    case ".m3u":
                        format = PLParser.PlFormat.m3u;
                        break;
                    case ".m3u8":
                        format = PLParser.PlFormat.m3u8;
                        break;
                    case ".yspl":
                        format = PLParser.PlFormat.yspl;
                        break;
                    default:
                        format = PLParser.PlFormat.unsupported;
                        break;
                }
                PLParser.savePlayList(sfd.FileName, format, loaded);
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
                    PLParser.PlFormat format = PLParser.PlFormat.unsupported;
                    switch (fInfo.Extension)
                    {
                        case ".yspl":
                            format = PLParser.PlFormat.yspl;
                            break;
                        case ".m3u":
                            format = PLParser.PlFormat.m3u;
                            break;
                        case ".m3u8":
                            format = PLParser.PlFormat.m3u8;
                            break;
                        default:
                            break;
                    }
                    interruptLoading = true;
                    CreateLoadingPanel(0);
                    PLParser.loadPlayList(ofd.FileName, BassEngine.getSupFilter,
                        format, ref loaded, playList.Items, ref interruptLoading);
                    RemoveLoadingPanel(0);
                }
            }
        }

        internal void loadDefaultPlayList(string plPath)
        {
            CreateLoadingPanel(0);
            PLParser.loadPlayList(plPath, BassEngine.getSupFilter,
                    PLParser.PlFormat.yspl, ref loaded, playList.Items, ref interruptLoading);
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
                    if (playList.Items[i].ToString().ToLower().Contains(searchBox.Text.ToLower()))
                    {
                        searchIndex = playList.SelectedIndex = i;
                        searchIndex++;
                        break;
                    }
                    else
                    {
                        playList.SelectedIndex = -1;
                    }

                }
                if (i >= loaded.Count)
                {
                    i = searchIndex = 0;
                }
            }
        }
                       
        private void startSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (BassEngine.getStatus() > 0))
            {
                mainTimer.Enabled = !(seeking = true);
            }
        }

        private void endSeeking(object sender, MouseEventArgs me)
        {
            if ((me.Button == MouseButtons.Left) && (seeking))
            {
                mainTimer.Enabled = !(seeking = false);
                BassEngine.setPositionSeconds(BassEngine.getCurrentLengthSeconds() * seekSet);
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
                if (loaded.Count > 0)
                {
                    playList_DoubleClick(playList, null);
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
                        tagForm.Opacity = this.Opacity = (UpOrDn ? 1.00F : 0.40F);
                    }
                    break;
            }
            tagForm.Opacity = this.Opacity;
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
    }
}
