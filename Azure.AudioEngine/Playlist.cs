using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Azure.MediaUtils
{
    public sealed class Playlist
    {
        #region variables

        private SortedDictionary<UInt64, PlaylistEntry> list;
        private SortedDictionary<UInt64, UInt64> order;
        private UInt64 currentIndex;
        private bool disposed;

        #endregion

        #region properties

        public List<PlaylistEntry> FileList { get { return list.Select(x => x.Value).ToList(); } }
        public int Count { get { return list.Count; }  }

        #endregion

        #region constructor / destructor
        public Playlist(String[] files)
        {
            SetupPlaylist(files);
        }

        public Playlist(String directoryPath, String fileExtensions, Boolean allDirectories = false)
        { 
            SetupPlaylist(Utils.GetSupportedFilesPath(directoryPath, fileExtensions, allDirectories).ToArray());        
        }


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Private implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                this.order.Clear();
                this.list.Clear();
                this.currentIndex = 0;
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion

        private void SetupPlaylist(String[] files)
        {
            list = new SortedDictionary<UInt64, PlaylistEntry>();
            order = new SortedDictionary<UInt64, UInt64>();

            UInt64 index;
            for (int i = 0; i < files.Length; i++)
            {
                index = Convert.ToUInt64(i);
                list.Add(index, new PlaylistEntry(files[i]));
                order.Add(index, index);
            }

            currentIndex = 0;
        }
    }
}
