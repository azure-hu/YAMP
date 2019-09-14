using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Azure.LibCollection.CS;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace Azure.YAMP
{
    sealed class PlayList
    {
        private ListView lw;
        private PlayerUI3 p;
        public List<MediaItem> list;
        private int _lastKnownSelected = 0;
        private Guid SelectedGuid;
        private BackgroundWorker _bw;

        public PlayList(ListView listView, PlayerUI3 parent)
        {
            list = new List<MediaItem>();
            p = parent;
            lw = listView;
            lw.DragDrop += this.DragDrop;
            lw.DragEnter += this.DragEnter;
            //lw.DragLeave
            //lw.DragOver
            lw.MouseDoubleClick += this.MouseDoubleClick;
            //lw.MouseEnter
            //lw.MouseHover
            //lw.MouseLeave
            //lw.MouseMove
            lw.MouseUp += this.MouseUp;
            lw.ItemSelectionChanged += this.SelectionChanged;
            _bw = new BackgroundWorker();
            _bw.DoWork += _bw_DoWork;
        }

        private void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            this.RebuildListView();            
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
                lw.SelectedItems.Clear();
                lw.SelectedIndices.Clear();
                p.currentIndex = this.FindSelected();
                this.LoadFromDir(_droppedFiles, true, 0, false);                
                this.SelectNext(_lastKnownSelected);                
                //this.ChangePlayListSelection(_lastKnownSelected);
            }
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hit = lw.HitTest(e.Location.X, e.Location.Y);
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
            foreach (var obj in lw.Items)
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
                ListViewHitTestInfo hit = lw.HitTest(e.Location.X, e.Location.Y);
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
            if (lw.SelectedIndices.Count > 0)
            {
                p.selectedIndex = lw.SelectedIndices[0];
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
            SelectedGuid = this.list[_lastKnownSelected].guid;
            return _lastKnownSelected;
        }

        public int FindSelected()
        {
            if (this.list.Count > 0)
            {
                if (_lastKnownSelected > -1)
                {
                    int index = this.list.FindIndex(x => x.guid == SelectedGuid);
                    _lastKnownSelected = index;
                }
                /*
                if (_lastKnownSelected > -1 && !this.list[this._lastKnownSelected].Selected)
                {
                    for (int i = 0; i < this.list.Count; ++i)
                    {
                        if (this.list[i].Selected)
                        {
                            _lastKnownSelected = i;
                            break;
                        }
                    }
                    _lastKnownSelected = -1;
                }
                */
            }
            else
            {
                _lastKnownSelected = -1;
            }
            return _lastKnownSelected;
        }

        public void ReleaseSelection(int index)
        {
            /*
            this.list[index].Selected = false;
            lw.Items[index].SubItems[5].Text = false.ToString();
            */
            this.SelectedGuid = Guid.Empty;
        }

        public int SetSelection(int index)
        {
            /*
            this.list[index].Selected = true;
            lw.Items[index].SubItems[5].Text = true.ToString();
            */
            this.SelectedGuid = this.list[index].guid;
            return index;
        }

        public void RebuildList()
        {
            int i = 1;
            List<MediaItem> _newLoaded = new List<MediaItem>();

            foreach (ListViewItem item in lw.Items)
            {
                MediaItem _newItem = new MediaItem(item.SubItems[3].Text, item.SubItems[4].Text);
                /*
                if (item.SubItems[5].Text == true.ToString())
                {
                    _newItem.Selected = true;
                    _lastKnownSelected = i - 1;
                }
                else
                    _newItem.Selected = false;
                */
                //item.SubItems[0].Text = i.ToString();
                ++i;
                _newLoaded.Add(_newItem);
            }


            this.list.Clear();
            this.list = _newLoaded;
        }

        public void ChangePlayListSelection(int index)
        {
            lw.SelectedItems.Clear();
            lw.SelectedIndices.Clear();
            int foundIndex = this.list.FindIndex( x => x.guid == SelectedGuid);
            if (index != foundIndex)
                index = foundIndex;
            lw.Items[index].Selected = true;
            //lw.Items[index].Focused = true;
            //lw.Focus();
            lw.EnsureVisible(index);
        }

        public void RebuildListView()
        {
            lw.Items.Clear();
            GC.Collect();
            lw.Invoke((MethodInvoker) delegate {
                //lw.BeginUpdate();
                ListView.ListViewItemCollection lCollection = new ListView.ListViewItemCollection(lw);
                foreach (MediaItem mItem in this.list)
                {
                    ListViewItem _lvi = new ListViewItem(GetListItemData((uint)lCollection.Count + 1, mItem));
                    lCollection.Add(_lvi);
                    Application.DoEvents();
                }
                //lw.EndUpdate();
            });
            
        }

        private string[] GetListItemData(uint index, MediaItem mItem)
        {
            string[] _lviData =     {
                                            mItem.FileMeta,
                                            mItem.Duration,
                                            mItem.FileName,
                                            mItem.FilePath,
                                            mItem.guid.ToString()
                                        };
            return _lviData;
        }

        public static void SaveList(string FileName, PlayListFormat format, List<MediaItem> Container)
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
            foreach (MediaItem writeOut in Container)
            {
                swr.WriteLine(writeOut.FilePath);
                i++;
            }
            if (format == PlayListFormat.yspl)
            {
                swr.WriteLine("#End ... Written " + i + " FilePaths with YAMP!#");
                swr.WriteLine("#Closed at " + DateTime.Now + ":" + DateTime.Now.Millisecond + "#");
            }
            swr.Close();
        }


        public void LoadList(string FileName, string supExt, PlayListFormat format, ref List<MediaItem> lContainer, ref bool interrupt)
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
                            break;
                    }
                    else
                    {
                        break;
                        //ok = false;
                    }
                }
                while (true); //(ok);
                BuildPlayList(_files.ToArray(), supExt, ref lContainer, ref interrupt);
            }
            else
            {
                MBoxHelper.ShowWarnMsg("Not a valid YAMP PlayList!",
                    "Error on playlist load!");
            }
            srr.Close();
        }

        public bool BuildPlayList(string[] files, string supExt, ref List<MediaItem> Container, ref bool interrupt)
        {
            string _exception;
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
                                    MediaItem mItem = new MediaItem(aElement);
                                    //mItem.PlayListOrder = (uint)Container.Count + 1;

                                    if (mItem.HasValidTags)
                                    {
                                        Container.Add(mItem);
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
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

        internal void SaveList(string plPath, PlayListFormat playListFormat)
        {
            PlayList.SaveList(plPath, playListFormat, this.list);
        }

        internal void Release()
        {
            lw.Items.Clear();
            list.Clear();
            _lastKnownSelected = -1;
        }

        public void LoadFromDir(string[] directoryName, bool recursive, int counter, bool interruptLoading)
        {
            try
            {
                if (!interruptLoading)
                {
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
                            if (dirs.Length > 0 && recursive)
                            {
                                LoadFromDir(dirs, recursive, counter + 1, interruptLoading);
                            }

                            //this.LoadMultiFiles(ref files, AudioEngine.getSupFilter, ref loaded, playListView22.Items, ref interruptLoading);
                            //PlayList.BuildPlayList(files, AudioEngine.getSupFilter, ref loaded, ref interruptLoading);
                            this.BuildPlayList(files, AudioEngine.getSupFilter, ref this.list, ref interruptLoading);
                        }
                    }
                }
                if (counter == 0)
                {
                    _bw.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in FileLoad function: " + ex.Message);
            }
        }

        internal void UpdateInfo(int currentIndex, string fileMeta, string duration)
        {
            ListViewItem _lvi = lw.Items[currentIndex];
            _lvi.SubItems[0].Text = fileMeta;
            _lvi.SubItems[1].Text = duration;
        }
    }

    public enum PlayListFormat
    {
        yspl,
        m3u,
        m3u8,
        unsupported
    }


}
