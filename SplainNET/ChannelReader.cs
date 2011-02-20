using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace libSplain
{
    public class ChannelReader
    {
        string[] strLinks;
        string[] strNames;
        long streams;
        string strFile;

        public ChannelReader(string StreamListFile)
        {
            strFile = StreamListFile;
            CountStreams(strFile);
            FillUpStreams(strFile);
        }

        private bool CountStreams(string FileName)
        {
            streams = 0;

            using (StreamReader r = new StreamReader(FileName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
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
    }
}
