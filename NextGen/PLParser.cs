using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using libIsh;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace YAMP
{
    public static class PLParser
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
            ref List<MediaItem> lContainer, ListBox.ObjectCollection lCollection, ref bool interrupt)
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
                lCollection.Clear();
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
                                LoadMultiFiles(ref _file, supExt, ref lContainer, lCollection, ref interrupt);
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
            ref List<MediaItem> Container, ListBox.ObjectCollection Collection, ref bool interrupt)
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
                                    MediaItem mItem = new MediaItem(aElement);
                                    TagReader.Init(mItem.getFilePath);
                                    if (TagReader.hasValidTags)
                                    {
                                        Container.Add(mItem);
                                        Collection.Add(mItem.getFileName);
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
            ListBox.ObjectCollection objectCollection, ref bool interrupt)
        {
            string[] _files = (string[])files;
            LoadMultiFiles(ref _files, p, ref loaded, objectCollection, ref interrupt);
        }
    }
}
