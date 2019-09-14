using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace Azure.MediaUtils
{
    public class AudioFile : IDisposable
    {
        public enum ExtraAttribute
        {
            Album, TrackNumber, AlbumArtist, Year, Genre, Channels, BitRate, SampleRate
        }

        #region variables
        protected readonly String filePath;
        protected Boolean disposed = false;
        protected DateTimeOffset timeOffset;
        protected Boolean metaRead;
        protected String title;
        protected String artist;
        protected Double durationSeconds = 0;
        #endregion

        #region properties
        public String FilePath { get { return filePath; } }
        public Int64 FileSizeInBytes { get { return (new FileInfo(this.FilePath).Length); } }
        public String Title
        {
            get
            {
                if (!metaRead)
                    this.DoRefresh();
                return this.title;
            }
        }

        public String Artist
        {
            get
            {
                if (!metaRead)
                    this.DoRefresh();
                return this.artist;
            }
        }

        public Double DurationSeconds
        {
            get
            {
                if (!metaRead)
                    this.DoRefresh();
                return this.durationSeconds;
            }
        }


        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(this.DurationSeconds);
            }
        }

        public String DurationText
        {
            get
            {
                TimeSpan duration = this.Duration;
                return String.Format("{0:0}:{1:D2}", duration.TotalMinutes, duration.Seconds);
            }
        }
		/*
        public string ArtistTitleText
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.Artist))
                {
                    return this.Title;
                }
                return String.Format("{0} - {1}", this.Artist, this.Title);
            }
        }
		*/
		public Dictionary<ExtraAttribute, String> ExtraAttributes
        {
            get
            {
                Utils.Prepare();
                Dictionary<ExtraAttribute, String> attribs = new Dictionary<ExtraAttribute, string>();
                TAG_INFO tagInfo = BassTags.BASS_TAG_GetFromFile(this.FilePath, true, true);
                attribs.Add(ExtraAttribute.Album, tagInfo.album);
                attribs.Add(ExtraAttribute.TrackNumber, tagInfo.track);
                attribs.Add(ExtraAttribute.AlbumArtist, tagInfo.albumartist);
                attribs.Add(ExtraAttribute.Year, tagInfo.year);
                attribs.Add(ExtraAttribute.Genre, tagInfo.genre);
				attribs.Add(ExtraAttribute.BitRate, tagInfo.bitrate.ToString());
				BASS_CHANNELINFO chInfo = tagInfo.channelinfo;
				attribs.Add(ExtraAttribute.Channels, chInfo.chans.ToString());
				attribs.Add(ExtraAttribute.SampleRate, chInfo.freq.ToString());
				return attribs;
            }
        }


        public System.Drawing.Image AlbumArtwork
        {
            get
            {
                TAG_INFO tagInfo = BassTags.BASS_TAG_GetFromFile(this.FilePath, true, true);
                return tagInfo.PictureGetImage(0);
            }
        }

        public System.Windows.Media.Imaging.BitmapImage AlbumArtwork2
        {
            get
            {
                return Utils.BitmapImageFromImage(AlbumArtwork);
            }
        }

        #endregion

        #region constructor / destructor

        public AudioFile(string path, bool eagerMode = false)
        {
            this.filePath = path;
            this.metaRead = eagerMode;
            if (eagerMode)
            {
                this.CheckIfRefreshNeeded();
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Private implementation of Dispose pattern.
        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                this.timeOffset = DateTimeOffset.MinValue;
                this.title = null;
                this.artist = null;
                this.durationSeconds = 0;
            }

            disposed = true;
        }

        #endregion

        public Boolean CheckIfRefreshNeeded()
        {
            DateTimeOffset lastWrite = (new FileInfo(this.FilePath)).LastWriteTimeUtc;
            if (timeOffset == default(DateTimeOffset) || lastWrite > timeOffset)
            {
                timeOffset = lastWrite;
                DoRefresh();
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void DoRefresh()
        {
            Utils.Prepare();
            TAG_INFO tagInfo = BassTags.BASS_TAG_GetFromFile(this.FilePath, true, true);
            this.title = (String.IsNullOrWhiteSpace(tagInfo.title) ? new FileInfo(this.FilePath).Name : tagInfo.title);
            this.artist = tagInfo.artist;
            this.durationSeconds = tagInfo.duration;
            if (!this.metaRead)
                this.metaRead = true;
        }

    }
}
