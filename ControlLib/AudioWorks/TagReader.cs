using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TagLib;

namespace Azure.LibCollection.CS.AudioWorks
{
    public static class TagReader
    {
        public static TagLib.Tag Choose(TagLib.Tag first, TagLib.Tag second)
        {
            if (first == null)
            {
                if (second == null)
                {
                    return null;
                }
                else
                {
                    return second;
                }
            }
            else
            {
                return first;
            }

        }

        private static TagLib.Tag GetTag(TagLib.File _file)
        {
            return
            TagReader.Choose(TagReader.Choose(TagReader.getID3(_file)[1], TagReader.getApple(_file)),
                TagReader.Choose(TagReader.getXiph(_file), TagReader.getApe(_file)));
        }

        internal static File GetFileReference(string filePath)
        {
            return TagLib.File.Create(filePath);
        }

        public static string existingTag(string filePath, string param)
        {
            string _retVal = string.Empty;
            using (TagLib.File _file = TagLib.File.Create(filePath))
            {
                switch (param)
                {
                    case "Performers": _retVal = ArrayConv.StringArrayToString(TagReader.GetTag(_file).Performers, ","); break;
                    case "Title": _retVal = TagReader.GetTag(_file).Title; break;
                    case "Album": _retVal = TagReader.GetTag(_file).Album; break;
                    default: return string.Empty;
                }
            }

            return _retVal;
        }

        public static void ReLoad(string filePath)
        {
            List<string> ChannelProps = new List<string>();
            try
            {
                using (TagLib.File _file = TagLib.File.Create(filePath))
                {
                    ChannelProps = GetProps(_file);
                }
            }
            catch (Exception ex)
            {
                ChannelProps.Clear();
                MBoxHelper.ShowWarnMsg(ex.Message + "\n File: " + filePath, "Invalid format header!");
            }
        }



        public static TagLib.Tag[] getID3(TagLib.File _file)
        {

            return new Tag[] {_file.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag,
                _file.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag};
        }

        public static TagLib.Tag getApple(TagLib.File _file)
        {
            return _file.GetTag(TagLib.TagTypes.Apple) as TagLib.Mpeg4.AppleTag;
        }

        public static TagLib.Tag getXiph(TagLib.File _file)
        {
            return _file.GetTag(TagLib.TagTypes.Xiph) as TagLib.Ogg.XiphComment;
        }

        public static TagLib.Tag getApe(TagLib.File _file)
        {
            return _file.GetTag(TagLib.TagTypes.Ape) as TagLib.Ape.Tag;
        }

        public static TagLib.Tag getAsf(TagLib.File _file)
        {
            return _file.GetTag(TagLib.TagTypes.Asf) as TagLib.Asf.Tag;
        }

        public static List<string> GetProps(TagLib.File _file)
        {
            List<string> ChannelProps = new List<string>();
            ChannelProps.Add(((double)_file.Properties.AudioSampleRate / 1000).ToString());
            ChannelProps.Add(_file.Properties.AudioBitrate.ToString());
            ChannelProps.Add(_file.Properties.AudioChannels.ToString());
            ChannelProps.Add(String.Format("{0:D2}:{1:D2}:{2:D2}", (int)_file.Properties.Duration.TotalHours,
                _file.Properties.Duration.Minutes, _file.Properties.Duration.Seconds));
            return ChannelProps;
        }


        public static Image GetAlbumArtwork(int Width, int Height, string filePath)
        {
            Image _img = null;
            using (TagLib.File _file = TagLib.File.Create(filePath))
            {
                TagLib.Tag _theTag = TagReader.existingTag(_file);
                _img = GetAlbumArtwork(Width, Height, _theTag);
            }
            return _img;
        }

        public static Image GetAlbumArtwork(int Width, int Height, TagLib.Tag _theTag)
        {

            Bitmap bmp = new Bitmap(1, 1);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(_theTag.Pictures[0].Data.Data))
            {
                Image coverImg = Image.FromStream(ms);

                int originalW = coverImg.Width;
                int originalH = coverImg.Height;

                int _Width = (Width < 0 ? originalW : Width);
                int _Height = (Height < 0 ? originalH : Height);
                bmp = new Bitmap(_Width, _Height);

                Graphics g = Graphics.FromImage((Image)bmp);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(coverImg, 0, 0, _Width, _Height);

                g.Dispose();
            }
            return (Image)bmp;

        }

        public static Dictionary<string, object> ExtractTag(string filePath)
        {
            var props = new Dictionary<string, object>();
            using (TagLib.File _file = TagLib.File.Create(filePath))
            {
                TagLib.Tag _theTag = TagReader.existingTag(_file);

                string _performers = (_theTag.Performers.Length > 0 ? _theTag.Performers[0] : string.Empty);
                props.Add("Performers", _performers);

                string _title = (_theTag.Title == null ? (new System.IO.FileInfo(filePath)).Name : _theTag.Title);
                props.Add("Title", _title);

                string _fileMeta = string.Format("{0}{1}{2}", _performers, (_performers == string.Empty ? string.Empty : " - "), _title);
                props.Add("FileMeta", _fileMeta);

                props.Add("Album", (_theTag.Album == null ? string.Empty : _theTag.Album));
                props.Add("TrackNumber", _theTag.Track);
                props.Add("AudioSampleRate", _file.Properties.AudioSampleRate);
                props.Add("AudioBitrate", _file.Properties.AudioBitrate);
                props.Add("AudioChannels", _file.Properties.AudioChannels);
                props.Add("Duration", string.Format("{0}:{1:D2}", (int)_file.Properties.Duration.TotalMinutes, _file.Properties.Duration.Seconds));
                /*
                if (_theTag.Pictures.Length > 0)
                    props.Add("CoverArt", TagReader.GetAlbumArtwork(-1,-1,_theTag));
                */
            }
            return props;
        }

        private static TagLib.Tag existingTag(TagLib.File _file)
        {
            return TagReader.Choose(
                    TagReader.Choose(TagReader.getID3(_file)[1],
                TagReader.Choose(
                        TagReader.Choose(TagReader.getApple(_file), TagReader.getXiph(_file)),
                        TagReader.Choose(TagReader.getAsf(_file), TagReader.getApe(_file))
                        )
                    ), TagReader.getID3(_file)[0]);
        }

        public static bool fileHasValidTags(string filePath)
        {
            bool _retVal = false;
            using (TagLib.File _file = TagLib.File.Create(filePath))
            { _retVal = (_file != null); }

            return _retVal;
        }

        /*
        private static TagLib.File fileTag;
        private static List<string> ChannelProps;

        public static bool Init(string filePath)
        {
            try
            {
                {
                    ChannelProps = new List<string>();
                    ReLoad(filePath);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static Tag existingTag()
        {
            return
            TagReader.Choose(TagReader.Choose(TagReader.getID3()[1], TagReader.getApple()),
                TagReader.Choose(TagReader.getXiph(), TagReader.getApe()));
        }

        public static TagLib.Tag[] getID3()
        {

            return getID3(fileTag);
        }

        public static TagLib.Tag getApple()
        {
            return getApple(fileTag);
        }

        public static TagLib.Tag getXiph()
        {
            return getXiph(fileTag);
        }

        public static TagLib.Tag getApe()
        {
            return getApe(fileTag);
        }

        public static TagLib.Tag getAsf()
        {
            return getAsf(fileTag);
        }

        public static Image GetAlbumArtwork(int Width, int Height)
        {
            return GetAlbumArtwork(Width, Height, TagReader.existingTag());
        }

        public static bool hasValidTags
        {
            get
            {
                return (fileTag != null);
            }
        }

        public static List<string> GetProps
        {
            get
            {
                return ChannelProps;
            }
        }

        public static void Clean()
        {
            try
            {
                ChannelProps.Clear();
                fileTag = null;
            }
            catch (Exception)
            {


            }
        }
        */
    }
}
