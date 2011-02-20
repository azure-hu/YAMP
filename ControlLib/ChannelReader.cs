using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace libZoi
{
    public class ChannelReader
    {
        string[] strLinks;
        string[] strNames;
        long streams = 0;
        string strFile;

        public ChannelReader(string StreamListFile)
        {
            ReInit(StreamListFile);
        }

        public void ReInit(string StreamListFile)
        {
            strFile = StreamListFile;
            ReInit();
        }

        public void ReInit()
        {
            if (streams > 0)
            {
                CleanUp();
            }
            CountStreams(strFile);
            FillUpStreams(strFile);
        }

        private void CleanUp()
        {
            for (long i = 0; i < streams; ++i)
            {
                strLinks[i] = strNames[i] = null;
            }

            strLinks = null;
            strNames = null;
            streams = 0;
        }

        private bool CountStreams(string FileName)
        {
            streams = 0;

            using (StreamReader r = new StreamReader(FileName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    if (line == "")
                    {
                        CleanUp();
                        return false;
                    }
                    streams++;
                }
            }
            if (streams % 2 == 0)
            {
                streams = streams / 2;
                return true;
            }
            else
            {
                streams = 0;
                return false;
            }
        }

        private void FillUpStreams(string FileName)
        {
            StreamReader x = new StreamReader(FileName);
            strLinks = new string[streams];
            strNames = new string[streams];
            for (int i = 0; i < streams; ++i)
            {
                strNames[i] = x.ReadLine();
                strLinks[i] = x.ReadLine();
            }
            x.Close();
        }

        public string GetURL(int index)
        {
            return strLinks[index];
        }

        public string[] Links
        {
            get { return strLinks; }
        }

        public string[] Names
        {
            get { return strNames; }
        }

        public void Add(string Channel, string URL)
        { 
            
        }

        public string StreamFile
        {
            get { return strFile; }
        }

        public long Size
        {
            get { return streams; }
        }
    }
}
