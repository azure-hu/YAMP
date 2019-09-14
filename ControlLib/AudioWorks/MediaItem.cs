using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass.AddOn.Tags;

namespace Azure.LibCollection.CS.AudioWorks
{
    public class MediaItemInfo : IDisposable
    {
        private string _filePath;
        private Dictionary<string, object> _metaInfo;
        private bool _disposed = false;

        public MediaItemInfo(string filePath)
        {
            this._filePath = filePath;
            _metaInfo = TagReader.ExtractTag(this._filePath);
        }

        public Dictionary<string, object> MetaInfo
        {
            get
            {
                return _metaInfo;
            }
        }

        public string Performers
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("Performers"))
                {
                    _info = (_metaInfo["Performers"].ToString());
                }
                return _info;
            }
        }

        public string Title
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("Title"))
                {
                    _info = (_metaInfo["Title"].ToString());
                }
                return _info;
            }
        }

        public string Album
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("Album"))
                {
                    _info = (_metaInfo["Album"].ToString());
                }
                return _info;
            }
        }

        public uint TrackNumber
        {
            get
            {
                uint trackNum = 0;
                if (_metaInfo.ContainsKey("TrackNumber"))
                {
                    trackNum = ((uint)_metaInfo["TrackNumber"]);
                }
                return trackNum;
            }
        }

        public string FileMeta
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("FileMeta"))
                {
                    _info = (_metaInfo["FileMeta"].ToString());
                }
                return _info;
            }
        }

        public string Duration
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("Duration"))
                {
                    _info = (_metaInfo["Duration"].ToString());
                }
                return _info;
            }
        }

        public string AudioSampleRate
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("AudioSampleRate"))
                {
                    _info = (_metaInfo["AudioSampleRate"].ToString());
                }
                return _info;
            }
        }

        public string AudioBitrate
        {
            get
            {
                string _info = string.Empty;
                if (_metaInfo.ContainsKey("AudioBitrate"))
                {
                    _info = (_metaInfo["AudioBitrate"].ToString());
                }
                return _info;
            }
        }

        public System.Drawing.Image CoverArt
        {
            get
            {
                System.Drawing.Image _coverArt = null;
                if (_metaInfo.ContainsKey("CoverArt"))
                {
                    _coverArt = (_metaInfo["CoverArt"] as System.Drawing.Image);
                }
                else
                {
                    _coverArt = TagReader.GetAlbumArtwork(-1, -1, _filePath);
                }
                return _coverArt;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _metaInfo.Clear();
                _metaInfo = null;
                _filePath = null;
                _disposed = true;
            }
        }
    }

    public class MediaItem : IDisposable
    {
        private string _filePath;
        private string _ext;
        private string _fullPath;
        private string _fileName;
        private bool _hasValidTags;
        //private bool _selected = false;
        private string _fileMeta;
        private string _duration;
        private bool _disposed = false;
        public Guid guid;

        //public uint PlayListOrder { get { return _playListOrder; } set { _playListOrder = value; } }

        //private Dictionary<string, object> metaInfo;
        //TAG_INFO mediaTag;
        //string _length;

        /// <summary>
        /// Initializes the MediaItem from an input file.
        /// </summary>
        /// <param name="file">The Input File.</param>
        public MediaItem(string file)
        {
            Reset();
            FullPath = System.IO.Path.GetFullPath(file);
            FileName = System.IO.Path.GetFileNameWithoutExtension(file);
            Extension = System.IO.Path.GetExtension(file);
            FilePath = FullPath.Substring(0, FullPath.Length - (FileName.Length + Extension.Length + 1));
            guid = Guid.NewGuid();
            RefreshInfo();
        }

        public MediaItem(string file, string uid)
        {
            Reset();
            FilePath = file;
            FileName = System.IO.Path.GetFileNameWithoutExtension(file);
            guid = new Guid(uid);
            RefreshInfo();
        }

        public bool RefreshInfo()
        {
            this.HasValidTags = TagReader.fileHasValidTags(this.FullPath);
            if (this.HasValidTags)
            {
                using (MediaItemInfo _mif = new MediaItemInfo(this.FullPath))
                {
                    FileMeta = _mif.FileMeta;
                    Duration = _mif.Duration;
                }
            }
            return this.HasValidTags;
        }

        public string FilePath
        {
            get { return _filePath; }
            private set
            {
                if (value != null)
                {
                    /*
                    if (_filePath == null)
                    {
                        _filePath = string.Empty;
                    }
                    */
                    _filePath = value;
                }
            }
        }

        public string FullPath
        {
            get { return _fullPath; }
            private set
            {
                if (value != null)
                {
                    /*
                    if (_filePath == null)
                    {
                        _filePath = string.Empty;
                    }
                    */
                    _fullPath = value;
                }
            }
        }

        public string Extension
        {
            get { return _ext; }
            private set
            {
                if (value != null)
                {
                    _ext = value;
                }
            }
        }

        public string FileName
        {
            get { return _fileName; }
            private set
            {
                if (value != null)
                {
                    /*
                    if (_fileName == null)
                    {
                        _fileName = string.Empty;
                    }
                    */
                    _fileName = value;
                }
            }
        }

        public bool HasValidTags
        {
            get { return _hasValidTags; }
            private set
            {
                _hasValidTags = value;
            }
        }

        public string FileMeta
        {
            get { return _fileMeta; }
            private set
            {
                if (value != null)
                {
                    /*
                    if (_fileMeta == null)
                    {
                        _fileMeta = string.Empty;
                    }
                    */
                    _fileMeta = value;
                }
            }
        }

        public string Duration
        {
            get { return _duration; }
            private set
            {
                if (value != null)
                {
                    /*
                    if (_duration == null)
                    {
                        _duration = string.Empty;
                    }
                    */
                    _duration = value;
                }
            }
        }

        /*
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value;}
        }
        */

        public string Length
        {
            get
            {
                string _length = "";
                double seconds = this.LengthInSec;
                if (seconds != 0)
                {
                    TimeSpan t = TimeSpan.FromSeconds(seconds);
                    _length = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                }
                return _length;
            }
        }

        /*
        public TAG_INFO MediaTag
        {
            get { return mediaTag; }
        }
        */

        public double LengthInSec
        {
            get
            {
                TAG_INFO mediaTag = MediaUtils.GetTagFromFile(FilePath);
                if (mediaTag != null)
                {
                    return mediaTag.duration;
                }
                else
                    return MediaUtils.GetLengthFromFile(FilePath);
            }
        }

        public object GetMetaInfo(string key)
        {
            using (MediaItemInfo mii = new MediaItemInfo(this.FullPath))
            {
                if (mii.MetaInfo.ContainsKey(key))
                    return mii.MetaInfo[key];
                return null;
            }
        }

        private void Reset()
        {
            _fileMeta = string.Empty;
            _duration = string.Empty;
            _fileName = string.Empty;
            _filePath = string.Empty;
            //_coverArt = null;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Reset();
                _disposed = true;
            }
        }

        public static int CompareByFileNameAndTrackNum(MediaItem x, MediaItem y)
        {
            if (x.FilePath == y.FilePath)
            {
                if (x.HasValidTags && y.HasValidTags)
                {
                    uint xNum = (uint)x.GetMetaInfo("TrackNumber");
                    uint yNum = (uint)y.GetMetaInfo("TrackNumber");

                    return xNum.CompareTo(yNum);

                }
                else
                {
                    return x.FileName.CompareTo(y.FileName);
                }
            }
            else
            {
                return CompareByFileName(x, y);
            }
        }

        public static int CompareByFileName(MediaItem x, MediaItem y)
        {
            return x.FullPath.CompareTo(y.FullPath);
        }
    }
}
