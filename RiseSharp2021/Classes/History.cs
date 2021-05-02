using System;
using System.IO;

namespace SimpleWR2.Classes
{
    class History : IDisposable
    {
        string m_outFile;
        string m_historyDir;
        DateTime today;

        public History(string historyDir)
        {
            if (!Directory.Exists(historyDir))
            {
                Directory.CreateDirectory(historyDir);
            }
            this.m_historyDir = historyDir;
            InitializeHistoryFile();

        }

        private void InitializeHistoryFile()
        {
            if (m_outFile == null || today.Date != DateTime.Now.Date)
            {
                today = DateTime.Now;
                m_outFile = Path.Combine(this.m_historyDir, $"{today.ToString("yyyy-MM-dd")}.txt");
                CreateFile();
            }
        }

        public void ChangeChannel(channel_type channel)
        {
            AppendNewEntry(string.Format("{0}Current Channel: {1}{0}", Environment.NewLine, channel.name));
        }

        public void Save(string onlineInfoConcat)
        {
            AppendNewEntry($"{DateTime.Now.ToString("HH:mm:ss")} # {onlineInfoConcat}");
        }

        private void AppendNewEntry(string newEntry)
        {
            InitializeHistoryFile();
            using (StreamWriter sw = File.AppendText(m_outFile))
            {
                sw.WriteLine(newEntry);
            }
        }

        private void CreateFile()
        {
            if (!File.Exists(m_outFile))
            {
                File.Create(m_outFile);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.m_outFile = null;
                    this.m_historyDir = null;
                    today = default(DateTime);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~History() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
