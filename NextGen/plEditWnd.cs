using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using libZoi;
using System.IO;
using System.Threading;

namespace YAMP
{
    public partial class plEditWnd : Form
    {
        private MainWindow myParent;
        private List<MediaItem> loaded;
        private int currentIndex, searchIndex;
        private string helpPlayList, helpSearchBox;
        //private Panel lPnl;
        private bool loadingFiles, interruptLoading;
        private string defaultTitle;
        //private bool dragging;
        //private Point offset;
        //private ParameterizedThreadStart plPTMulti;

        public plEditWnd()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();
            loadingFiles = interruptLoading = false;
            loaded = new List<MediaItem>();
            helpPlayList = ("To move up (down) selected item, press '+' or 'W'  ('-' or 'S') button.");
            helpSearchBox = ("Type a substring to search, and press F3 to jump to the next occurrence.");
            //plPTMulti = new ParameterizedThreadStart(LoadMultiFiles);
        }

        public plEditWnd(MainWindow parent)
        {
            setParent(parent);
            Initialize();   
        }

        public void setParent(MainWindow parent)
        { 
            if((parent != null) && (myParent == null))
                myParent = parent;
        }

        public void playList_DragDrop(object sender, DragEventArgs e)
        {
            LoadFromDir((string[])e.Data.GetData(DataFormats.FileDrop),0);
            
        }

        private void RemoveLoadingPanel(int i)
        {
            if (i == 0)
            {
                /*
                this.Controls.Remove(lPnl);
                lPnl = null;
                */
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
                    /*
                    lPnl = new Panel();
                    Label loadLbl = new Label();
                    loadLbl.AutoSize = true;
                    loadLbl.Location = new Point(this.Width / 3, this.Height / 3);
                    loadLbl.Text = "Loading Files To Playlist...";
                    loadLbl.ForeColor = Color.WhiteSmoke;
                    lPnl.Controls.Add(loadLbl);
                    lPnl.Width = this.Width;
                    lPnl.Height = this.Height;
                    lPnl.BackColor = Color.Transparent;
                    this.Controls.Add(lPnl);
                    lPnl.Top = lPnl.Left = 0;
                    lPnl.BringToFront();
                    */
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

        /*
        private void LoadMultiFiles(Object o)
        {
            Array files = (Array)o;
            if (files != null)
            {
                foreach (string aElement in files)
                {
                    string fileEnding = new FileInfo(aElement).Extension;
                    if (myParent.EngineSuppExts().Contains(fileEnding))
                    {
                        MediaItem mItem = new MediaItem(aElement);
                        TagReader.Init(mItem.getFilePath);
                        if (TagReader.hasValidTags)
                        {
                            loaded.Add(mItem);
                            playList.Items.Add(mItem.getFileName);
                        }
                    }
                    Application.DoEvents();
                }
                //this.Activate();
            }
        }
        */
 
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

                            PLParser.LoadMultiFiles(ref files, myParent.EngineSuppExts(),
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

        public void playList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        public void playList_DoubleClick(object sender, EventArgs e)
        {
            if (playList.SelectedItem != null)
            {
                currentIndex = playList.SelectedIndex;
                myParent.PlayNewFile(loaded[currentIndex].getFilePath);                
            }
        }

        public void playList_Next(bool rePlay)
        {
            if (++currentIndex < loaded.Count)
            {
                myParent.PlayNewFile(loaded[currentIndex].getFilePath);
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
                myParent.PlayNewFile(loaded[currentIndex].getFilePath);
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
                myParent.PlayNewFile(loaded[currentIndex].getFilePath);
                playList.SelectedIndex = currentIndex;
            }
        }

        public void playList_Last()
        {
            if (loaded.Count > 0)
            {
                currentIndex = loaded.Count - 1;
                myParent.PlayNewFile(loaded[currentIndex].getFilePath);
                playList.SelectedIndex = currentIndex;
            }
        }

        private void playListWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            if(myParent.getSettingsExist)
            {
                yamp.Default.showPLEdit = false;
                yamp.Default.Save();
            }
        }

        public void playList_KeyDown(object sender, KeyEventArgs e)
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
                loadFiles.Filter = myParent.openMedia.Filter;
                loadFiles.Multiselect = true;
                if (loadFiles.ShowDialog() == DialogResult.OK)
                {
                    /*
                    CreateLoadingPanel();
                    //LoadMultiFiles(loadFiles.FileNames);
                    string[] selectedFiles = loadFiles.FileNames;
                    PLParser.LoadMultiFiles(ref selectedFiles, myParent.EngineSuppExts(),
                    ref loaded, playList.Items);
                    RemoveLoadingPanel();
                    */
                    LoadFromDir(loadFiles.FileNames, 0);
                }
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog chooseFolder = new FolderBrowserDialog())
            {
                if (myParent.getSettingsExist)
                {
                    chooseFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
                    chooseFolder.SelectedPath = yamp.Default.lastFolder;
                    chooseFolder.RootFolder = System.Environment.SpecialFolder.Desktop;
                    
                }
                if (chooseFolder.ShowDialog() == DialogResult.OK)
                {
                    //string[] allDirs = Directory.GetDirectories(chooseFolder.SelectedPath, "*", SearchOption.AllDirectories);
                    LoadFromDir(new string[]{ chooseFolder.SelectedPath }, 0);
                    if (myParent.getSettingsExist)
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
            if(playList.SelectedItems.Count > 0)
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
            PLParser.LoadMultiFiles(ref _file, myParent.EngineSuppExts(),
                ref loaded, playList.Items, ref interruptLoading);
        }

        public int getFilesCount
        {
            get
            {
                return loaded.Count;
            }            
        }

        public int getCurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        private void searchBox_Click(object sender, EventArgs e)
        {
            searchBox.SelectAll();
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
            if (myParent.getSettingsExist)
            {
                yamp.Default.sizePLEdit = new Point(this.Size.Width, this.Size.Height);
                yamp.Default.Save();
            }
        }

        private void playListWindow_Move(object sender, EventArgs e)
        {

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
                    case ".m3u" :
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
                PLParser.savePlayList(sfd.FileName, format, loaded );
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
                    PLParser.loadPlayList(ofd.FileName, myParent.EngineSuppExts(),
                        format, ref loaded, playList.Items, ref interruptLoading);
                    RemoveLoadingPanel(0);
                    //loadPlayList(ofd.FileName);
                }
            }
        }

        internal void savePlayList(string plPath)
        {
            PLParser.savePlayList(plPath, PLParser.PlFormat.yspl, loaded);
        }

        internal void loadDefaultPlayList(string plPath)
        {
            CreateLoadingPanel(0);
            PLParser.loadPlayList(plPath, myParent.EngineSuppExts(),
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

        /*

        private void _MouseDown(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                dragging = true;
                offset.X = me.X;
                offset.Y = me.Y;
            }
        }

        private void _MouseUp(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }

        private void _MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point
                    (currentScreenPos.X - offset.X,
                     currentScreenPos.Y - offset.Y);
            }
        }

        */
    }
}
