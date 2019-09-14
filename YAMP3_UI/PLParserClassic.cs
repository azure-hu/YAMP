using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Azure.LibCollection.CS;

namespace Azure.YAMP
{
    public static class PLParserClassic
    {
        public enum PlFormat
        {
            yspl,
            m3u,
            m3u8,
            unsupported
        }
        
        static public void savePlayList(string FileName, PlFormat format, List<MediaItem> Container)
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            StreamWriter swr = File.CreateText(FileName);
            switch (format)
            {
                case PlFormat.yspl:
                    {
                        swr.WriteLine("#YAMP Simplified PlayList#");
                        swr.WriteLine("#Started at " + DateTime.Now + ":" + DateTime.Now.Millisecond + "#");
                    }
                    break;
                case PlFormat.m3u:
                case PlFormat.m3u8:
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
                swr.WriteLine(writeOut.getFilePath);
                i++;
            }
            if (format == PlFormat.yspl)
            {
                swr.WriteLine("#End ... Written " + i + " FilePaths with YAMP!#");
                swr.WriteLine("#Closed at " + DateTime.Now + ":" + DateTime.Now.Millisecond + "#");
            }
            swr.Close();
        }

        public static void loadPlayList(string FileName, string supExt, PlFormat format,
            ref List<MediaItem> lContainer, //ListBox.ObjectCollection lCollection, 
            ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            StreamReader srr = File.OpenText(FileName);
            Regex headText = null;
            switch (format)
            {
                case PlFormat.yspl:
                    headText = new Regex("#YAMP Simplified PlayList#");
                    break;
                case PlFormat.m3u:
                case PlFormat.m3u8:
                    headText = new Regex("#*M3U");
                    break;
                case PlFormat.unsupported:
                    break;
            }
            if ( headText.IsMatch( srr.ReadLine() ) )
            {
                lContainer.Clear();
                //lCollection.Clear();
                lviCollection.Clear();

                string readIn;
                if (format == PlFormat.yspl)
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
                                if(readIn.StartsWith(@"\"))
                                {
                                    readIn = Environment.GetEnvironmentVariable("%SYSTEMDRIVE%",
                                        EnvironmentVariableTarget.Machine) + readIn;
                                }
                                string[] _file = new string[] { readIn };
                                LoadMultiFiles(ref _file, supExt, ref lContainer, /*lCollection,*/ lviCollection, ref interrupt);
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

        public static bool LoadMultiFiles(ref string[] files, string supExt,
            ref List<MediaItem> Container, /*ListBox.ObjectCollection Collection,*/
            ListView.ListViewItemCollection ListViewCollection, ref bool interrupt)
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

        internal static void LoadMultiFiles(ref Array files, string p, ref List<MediaItem> loaded,
            /*ListBox.ObjectCollection objectCollection, */ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            string[] _files = (string[])files;
            LoadMultiFiles(ref _files, p, ref loaded, /*objectCollection,*/ lviCollection, ref interrupt);
        }
    }
}
