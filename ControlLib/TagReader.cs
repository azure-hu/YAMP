using System;
using System.Collections.Generic;
using System.Text;

using TagLib;
using System.Drawing;

namespace libIsh
{
    public static class TagReader
    {
        private static TagLib.File fileTag;
        private static List<string> ChannelProps;
        
        public static bool Init(string filePath)
        {
            try
            {
                //if (filePath != null)
                {
                    ChannelProps = new List<string>();
                    //Clean();
                    ReLoad(filePath);
                    return true;
                }
            }
            catch
            {
                //else
                return false;
            }
        }

        public static Tag Choose(Tag first, Tag second)
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

        private static Tag existingTag()
        {
            return
            TagReader.Choose(TagReader.Choose(TagReader.getID3()[1], TagReader.getApple()),
                TagReader.Choose(TagReader.getXiph(), TagReader.getApe()));
        }

        public static string existingTag(string param)
        {
            switch (param)
            {
                case "Performers": return ArrayConv.StringArrayToString(TagReader.existingTag().Performers, ",");
                case "Title": return TagReader.existingTag().Title;
                case "Album" : return TagReader.existingTag().Album;
                default: return "";
            }
        }
        
        public static void ReLoad(string filePath)
        {
            try
            {
                Clean();
                fileTag = TagLib.File.Create(filePath);
                loadBase();
            }
            catch (Exception ex)
            {
                ChannelProps.Clear();
                fileTag = null;
                MBoxHelper.ShowWarnMsg(ex.Message + "\n File: " + filePath, "Invalid format header!");
            }
        }

        private static void loadBase()
        {
            ChannelProps.Add(((double)fileTag.Properties.AudioSampleRate / 1000).ToString());
            ChannelProps.Add(fileTag.Properties.AudioBitrate.ToString());
            ChannelProps.Add(fileTag.Properties.AudioChannels.ToString());
            ChannelProps.Add(String.Format("{0:D2}:{1:D2}:{2:D2}", (int)fileTag.Properties.Duration.TotalHours,
                fileTag.Properties.Duration.Minutes, fileTag.Properties.Duration.Seconds));
        }

        public static TagLib.Tag[] getID3()
        {

            return new Tag[] {fileTag.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag,
                fileTag.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag};
        }

        public static TagLib.Tag getApple()
        {
            return fileTag.GetTag(TagLib.TagTypes.Apple) as TagLib.Mpeg4.AppleTag;
        }

        public static TagLib.Tag getXiph()
        {
            return fileTag.GetTag(TagLib.TagTypes.Xiph) as TagLib.Ogg.XiphComment;
        }
        
        public static TagLib.Tag getApe()
        {
            return fileTag.GetTag(TagLib.TagTypes.Ape) as TagLib.Ape.Tag;
        }

        public static TagLib.Tag getAsf()
        {
            return fileTag.GetTag(TagLib.TagTypes.Asf) as TagLib.Asf.Tag;
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

        public static Image GetAlbumArtwork(int Width, int Height)
        { 
        
            Bitmap bmp = new Bitmap(Width, Height);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(TagReader.existingTag().Pictures[0].Data.Data))
            {
                Image coverImg = Image.FromStream(ms);

                int originalW = coverImg.Width;
                int originalH = coverImg.Height;

                Graphics g = Graphics.FromImage((Image)bmp);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(coverImg, 0, 0, Width, Height);

                g.Dispose();
            }
            return (Image)bmp;

        }
    }
}
