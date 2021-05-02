using Azure.MediaUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Azure.YAMP
{
    sealed class PlayList
    {
        private ListView lv;
        private PlayerUI3 p;
        public List<PlaylistEntry> list;
        private int _lastKnownSelected = 0;
        private Guid SelectedGuid;
        private BackgroundWorker _bw;
        private delegate void PlaylistViewBuilderDelegate(PlaylistEditMode editMode);
        private readonly PlaylistViewBuilderDelegate playlistViewBuilderDelegate;


        public PlayList(ListView listView, PlayerUI3 parent)
        {
            list = new List<PlaylistEntry>();
            p = parent;
            lv = listView;
            lv.DragDrop += this.DragDrop;
            lv.DragEnter += this.DragEnter;
            //lw.DragLeave
            //lw.DragOver
            lv.MouseDoubleClick += this.MouseDoubleClick;
            //lw.MouseEnter
            //lw.MouseHover
            //lw.MouseLeave
            //lw.MouseMove
            lv.MouseUp += this.MouseUp;
            lv.ItemSelectionChanged += this.SelectionChanged;
            playlistViewBuilderDelegate = PlaylistViewBuilder2;
            _bw = new BackgroundWorker();
            _bw.DoWork += BuildPlaylistInBackground;
        }

        private void PlaylistViewBuilder2(PlaylistEditMode editMode)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var progress = p.playlistLoadingBar;
            progress.Value = 0;
            progress.Maximum = this.list.Count;
            progress.Step = 1;
            lv.BeginUpdate();
            ListView.ListViewItemCollection lCollection = null;
            if (editMode == PlaylistEditMode.ClearAll || editMode == PlaylistEditMode.LoadList)
            {
                lv.Items.Clear();
                GC.Collect();
                lCollection = new ListView.ListViewItemCollection(lv);
            }
            else
            {
                lCollection = lv.Items;
            }
            if (editMode == PlaylistEditMode.AddFiles || editMode == PlaylistEditMode.LoadList)
            {
                progress.Visible = true;
                List<ListViewItem> items = new List<ListViewItem>();
                foreach (PlaylistEntry mItem in this.list)
                {
                    ListViewItem _lvi = new ListViewItem(GetListItemData(mItem));
                    items.Add(_lvi);
                    progress.PerformStep();
                    Application.DoEvents();
                }

                lCollection.AddRange(items.ToArray());
                progress.Visible = false;
            }
            sw.Stop();
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv.EndUpdate();
        }

        private void PlaylistViewBuilder(PlaylistEditMode editMode)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var progress = p.playlistLoadingBar;
            progress.Value = 0;
            progress.Maximum = this.list.Count;
            progress.Step = 1;
            lv.BeginUpdate();
            ListView.ListViewItemCollection lCollection = null;
            if (editMode == PlaylistEditMode.ClearAll || editMode == PlaylistEditMode.LoadList)
            {
                lv.Items.Clear();
                GC.Collect();
                lCollection = new ListView.ListViewItemCollection(lv);
            }
            else
            {
                lCollection = lv.Items;
            }
            lv.EndUpdate();

            lv.BeginUpdate();
            if (editMode == PlaylistEditMode.AddFiles || editMode == PlaylistEditMode.LoadList)
            {
                progress.Visible = true;
                List<ListViewItem> items = new List<ListViewItem>();
                foreach (PlaylistEntry mItem in this.list)
                {
                    /*
                    bool found = false;
                    foreach (ListViewItem lvItem in lCollection)
                    {
                        if (lvItem.SubItems[5].Text == mItem.GUID.ToString())
                        {
                            found = true; break;
                        }
                    }

                    if (!found)
                    */
                    {
                        ListViewItem _lvi = new ListViewItem(GetListItemData(mItem));
                        items.Add(_lvi);
                    }
                    progress.PerformStep();
                    SetProgressBarText(progress, null);
                    Application.DoEvents();
                }
                lCollection.AddRange(items.ToArray());
                progress.Visible = false;
            }
            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv.EndUpdate();
        }

        public List<string> LoadFromDir(string[] directoryName, bool recursive, int counter, PlaylistEditMode editMode)
        {
            List<string> filePaths = new List<string>();
            HashSet<string> filePathSet = new HashSet<string>();
            try
            {
                if (directoryName.Length > 0)
                {

                    foreach (string item in directoryName)
                    {
                        string[] dirs = Directory.GetDirectories(item);
                        if (dirs.Length > 0 && recursive)
                        {
                            filePaths.AddRange(LoadFromDir(dirs, recursive, counter + 1, PlaylistEditMode.Nothing));
                            filePathSet.UnionWith(LoadFromDir(dirs, recursive, counter + 1, PlaylistEditMode.Nothing));
                        }
                        else
                        {
                            string[] files;
                            if (File.Exists(item))
                            {
                                files = new string[1];
                                files[0] = item;
                            }
                            else
                            {
                                files = Directory.GetFiles(item);
                            }

                            filePaths.AddRange(files);
                            filePathSet.UnionWith(files);
                        }
                    }
                }
                if (counter == 0)
                {
                    if (!_bw.IsBusy)
                    {
                        _bw.RunWorkerAsync(new object[] { filePaths, editMode });
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in FileLoad function: " + ex.Message);
            }
            return filePaths;
        }


        private void BuildPlaylistInBackground(object sender, DoWorkEventArgs e)
        {
            try
            {
                lv.Invoke((MethodInvoker)delegate
                {
                    p.loadingLabel.Show();
                    Thread.Sleep(1);
                });
                object[] _params = (e.Argument as object[]);
                List<string> _fileList = _params[0] as List<string>;
                PlaylistEditMode _editMode = (_params[1] as PlaylistEditMode?).Value;
                this.BuildPlayList(_fileList, ref this.list);
                this.RebuildListView(_editMode);
                lv.Invoke((MethodInvoker)delegate
                {
                    p.loadingLabel.Hide();
                });
            }
            catch (Exception x)
            {
                Azure.LibCollection.CS.MBoxHelper.ShowErrorMsg(x, "Error on building playlist!");
            }
        }

        public bool BuildPlayList(List<string> files, ref List<PlaylistEntry> Container)
        {
            string _exception;
            try
            {
                if (files != null)
                {
                    var filtered = Utils.FilterFiles(files, this.p.SupportedFileExtensions);

                    foreach (string aElement in filtered)
                    {
                        Container.Add(new PlaylistEntry(aElement));
                    }
                }
                return true;
            }
            catch (Exception x)
            {
                _exception = x.ToString();
                return false;
            }
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            string[] _droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_droppedFiles == null)
            {
                p.dragHappened = true;
            }
            else
            {
                lv.SelectedItems.Clear();
                lv.SelectedIndices.Clear();
                p.currentIndex = this.FindSelected();
                this.LoadFromDir(_droppedFiles, true, 0, PlaylistEditMode.AddFiles);

                this.SelectNext(_lastKnownSelected);
            }
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hit = lv.HitTest(e.Location.X, e.Location.Y);
            if (hit.Item != null)
            {
                int index = GetHitIndex(hit);
                this.SelectNext(index);
                p.PlayNewFileOnDoubleClick();
            };
        }

        private int GetHitIndex(ListViewHitTestInfo hit)
        {
            int index = 0;
            foreach (var obj in lv.Items)
            {
                if (obj == hit.Item)
                {
                    p.currentIndex = index;
                    break;
                }
                index++;
            }
            return index;
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (p.dragHappened)
            {
                int index = 0;
                ListViewHitTestInfo hit = lv.HitTest(e.Location.X, e.Location.Y);
                if (hit.Item != null)
                {
                    index = GetHitIndex(hit);
                }

                if (this.FindSelected() < 0)
                {
                    this.SelectNext(index);
                }
                p.dragHappened = false;
                this.RebuildList();
                p.currentIndex = this.FindSelected();
                ChangePlayListSelection(_lastKnownSelected);
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (lv.SelectedIndices.Count > 0)
            {
                p.selectedIndex = lv.SelectedIndices[0];
            }
        }

        public int SelectNext(bool forward)
        {
            int index = FindSelected();
            this.ReleaseSelection(index);
            index += (forward ? 1 : -1);
            if (index > this.list.Count)
            {
                index = 0;
            }
            if (index < 0)
            {
                index = (this.list.Count - 1);
            }
            _lastKnownSelected = this.SetSelection(index);
            return _lastKnownSelected;
        }

        public int SelectNext(int newIndex)
        {
            int index = FindSelected();
            if (index > 0)
            {
                this.ReleaseSelection(index);
            }
            if (newIndex > this.list.Count || newIndex < 0)
            {
                newIndex = 0;
            }
            _lastKnownSelected = this.SetSelection(newIndex);
            SelectedGuid = this.list[_lastKnownSelected].GUID;
            return _lastKnownSelected;
        }

        public int FindSelected()
        {
            if (this.list.Count > 0)
            {
                if (_lastKnownSelected > -1)
                {
                    int index = this.list.FindIndex(x => x.GUID == SelectedGuid);
                    _lastKnownSelected = index;
                }
            }
            else
            {
                _lastKnownSelected = -1;
            }
            return _lastKnownSelected;
        }

        public void ReleaseSelection(int index)
        {
            this.SelectedGuid = Guid.Empty;
        }

        public int SetSelection(int index)
        {
            this.SelectedGuid = this.list[index].GUID;
            return index;
        }

        public void RebuildList()
        {
            int i = 1;
            List<PlaylistEntry> _newLoaded = new List<PlaylistEntry>();

            foreach (ListViewItem item in lv.Items)
            {
                PlaylistEntry _newItem = new PlaylistEntry(item.SubItems[3].Text, item.SubItems[4].Text);
                ++i;
                _newLoaded.Add(_newItem);
            }


            this.list.Clear();
            this.list = _newLoaded;
        }

        public void ChangePlayListSelection(int index)
        {
            lv.SelectedItems.Clear();
            lv.SelectedIndices.Clear();
            /*
            int foundIndex = this.list.FindIndex( x => x.guid == SelectedGuid);
            if (index != foundIndex)
                index = foundIndex;
            */
            lv.Invoke((MethodInvoker)delegate
            {
                if (lv.Items.Count > index)
                {
                    lv.Items[index].Selected = true;
                    lv.EnsureVisible(index);
                }
            });
            //lw.Items[index].Focused = true;
            //lw.Focus();

        }

        public void RebuildListView(PlaylistEditMode editMode)
        {
            lv.Invoke(playlistViewBuilderDelegate, editMode);
        }

        private string[] GetListItemData(PlaylistEntry mItem)
        {
            string[] _lviData =     {
                                            mItem.Artist,
                                            mItem.Title,
                                            mItem.DurationText,
                                            mItem.FilePath,
                                            mItem.GUID.ToString()
                                        };
            return _lviData;
        }

        /// <summary>
        /// Adds text into a System.Windows.Forms.ProgressBar
        /// </summary>
        /// <param name="Target">The target progress bar to add text into</param>
        /// <param name="Text">The text to add into the progress bar.
        /// Leave null or empty to automatically add the percent.</param>
        private void SetProgressBarText
            (
            System.Windows.Forms.ProgressBar Target, //The target progress bar
            string Text //The text to show in the progress bar
            )
        {

            //Make sure we didn't get a null progress bar
            if (Target == null) { throw new ArgumentException("Null Target"); }

            //Now we can get to the real code

            //Check to see if we are to add in the percent
            if (string.IsNullOrEmpty(Text))
            {
                Text = string.Format("{0} / {1}", Target.Value, Target.Maximum);
            }

            //Now we can add in the text

            //gr will be the graphics object we use to draw on Target
            using (Graphics gr = Target.CreateGraphics())
            {
                Font TextFont = new Font(FontFamily.GenericSansSerif, 8F);
                gr.DrawString(Text, TextFont, new SolidBrush(Color.Black),
                    //Where we will draw it
                    new PointF(
                        Target.Width / 2 - (gr.MeasureString(Text, //Centered
                        TextFont).Width / 2.0F),
                    // Y Location (This is the same regardless of Location)
                    Target.Height / 2 - (gr.MeasureString(Text,
                        TextFont).Height / 2.0F)));
            }
        }

        public static void SaveList(string FileName, PlayListFormat format, IEnumerable<String> filePaths)
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            StreamWriter swr = File.CreateText(FileName);
            switch (format)
            {
                case PlayListFormat.yspl:
                    {
                        swr.WriteLine("#YAMP Simplified PlayList#");
                        swr.WriteLine("#Started at " + DateTime.Now + ":" + DateTime.Now.Millisecond + "#");
                    }
                    break;
                case PlayListFormat.m3u:
                case PlayListFormat.m3u8:
                    {
                        swr.WriteLine("#EXTM3U");
                    }
                    break;
                default:
                    {
                        swr.WriteLine("# WTF File Extension you want to use??? #");
                    }
                    break;
            }
            int i = 0;
            foreach (string filePath in filePaths)
            {
                swr.WriteLine(filePath);
                i++;
            }
            if (format == PlayListFormat.yspl)
            {
                swr.WriteLine("#End ... Written " + i + " FilePaths with YAMP!#");
                swr.WriteLine("#Closed at " + DateTime.Now + ":" + DateTime.Now.Millisecond + "#");
            }
            swr.Close();
        }


        public void LoadList(string FileName, string supExt, PlayListFormat format, ref List<PlaylistEntry> lContainer/*, bool interrupt*/)
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

            List<string> _files = new List<string>();

            if (headText.IsMatch(srr.ReadLine()))
            {
                lContainer.Clear();

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
                                    readIn = Environment.GetEnvironmentVariable("%SYSTEMDRIVE%", EnvironmentVariableTarget.Machine) + readIn;
                                }
                                _files.Add(readIn);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                        //ok = false;
                    }
                }
                while (true); //(ok);
                if (!_bw.IsBusy)
                {
                    _bw.RunWorkerAsync(new object[] { _files, PlaylistEditMode.LoadList });
                }

                //BuildPlayList(_files, supExt, ref lContainer, interrupt);
            }
            else
            {
                Azure.LibCollection.CS.MBoxHelper.ShowWarnMsg("Not a valid YAMP PlayList!", "Error on playlist load!");
            }
            srr.Close();
        }

        internal void SaveList(string plPath, PlayListFormat playListFormat)
        {
            PlayList.SaveList(plPath, playListFormat, this.list.Select(x => x.FilePath));
        }

        internal void Release()
        {
            lv.Items.Clear();
            list.Clear();
            _lastKnownSelected = -1;
        }

        internal void UpdateInfo(int currentIndex, string artist, string title, string duration)
        {
            ListViewItem _lvi = lv.Items[currentIndex];
            _lvi.SubItems[0].Text = artist;
            _lvi.SubItems[1].Text = title;
            _lvi.SubItems[2].Text = duration;
        }
    }

    public enum PlayListFormat
    {
        yspl,
        m3u,
        m3u8,
        unsupported
    }

    public enum PlaylistEditMode
    {
        AddFiles, LoadList, RemoveFiles, ClearAll, Nothing
    }
}