using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure.MediaUtils
{
    public sealed class PlaylistEntry : AudioFile
    {
        Dictionary<AudioFile.ExtraAttribute, String> extraData;
		Guid guid;
        #region properties

        public String Album
        {
            get
            {
                RefreshExtraData();
                return extraData[AudioFile.ExtraAttribute.Album];
            }
        }

        public String AlbumArtist
        {
            get
            {
                RefreshExtraData();
                return extraData[AudioFile.ExtraAttribute.AlbumArtist];
            }
        }

        public String Genre
        {
            get
            {
                RefreshExtraData();
                return extraData[AudioFile.ExtraAttribute.Genre];
            }
        }


        public String TrackNumber
        {
            get
            {
                RefreshExtraData();
                return extraData[AudioFile.ExtraAttribute.TrackNumber];
            }
        }

        public String Year
        {
            get
            {
                RefreshExtraData();
                return extraData[AudioFile.ExtraAttribute.Year];
            }
        }

		public Guid GUID { get { return this.guid; } set { this.guid = value; } }

		private void RefreshExtraData()
        {
            if (extraData == null || base.CheckIfRefreshNeeded())
                extraData = base.ExtraAttributes;
        }

        #endregion

        #region constructor / destructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="eagerMode">If "true", metadata will be read immediately. (Lasts longer!)</param>
        public PlaylistEntry(String filePath, Boolean eagerMode = false) : base(filePath, eagerMode)
        {
			GUID = Guid.NewGuid();
        }

		/// <summary>
		///
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="eagerMode">If "true", metadata will be read immediately. (Lasts longer!)</param>
		public PlaylistEntry(String filePath, Guid guid, Boolean eagerMode = false) : base(filePath, eagerMode)
		{
			GUID = guid;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="eagerMode">If "true", metadata will be read immediately. (Lasts longer!)</param>
		public PlaylistEntry(String filePath, String guidString, Boolean eagerMode = false) : base(filePath, eagerMode)
		{
			GUID = Guid.Parse(guidString);
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
                // Free any other managed objects here.
                //
                if (extraData.Count > 0)
                    extraData.Clear();
                base.Dispose(disposing);
            }

            disposed = true;
        }
        #endregion

    }
}
