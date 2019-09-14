using System;
using System.Collections.Generic;
using System.Text;

using TagLib;

namespace DomClassLib
{
    public class TagReader :IDisposable
    {
        private TagLib.File fileTag;
        private List<string> ChannelProps = new List<string>();

        public TagReader()
        {
            ChannelProps = new List<string>();
        }
                
        public void ReadTags(string filePath)
        {
            ChannelProps.Clear();
            fileTag = TagLib.File.Create(filePath);

            ChannelProps.Add(((double)fileTag.Properties.AudioSampleRate/1000).ToString());
            ChannelProps.Add(fileTag.Properties.AudioBitrate.ToString());
            ChannelProps.Add(fileTag.Properties.AudioChannels.ToString());
            ChannelProps.Add(String.Format("{0:D2}:{1:D2}:{2:D2}", (int)fileTag.Properties.Duration.TotalHours, 
                fileTag.Properties.Duration.Minutes, fileTag.Properties.Duration.Seconds));

            TagLib.Id3v1.Tag id3v1 = fileTag.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag;
            TagLib.Id3v2.Tag id3v2 = fileTag.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
            TagLib.Mpeg4.AppleTag apple = fileTag.GetTag(TagLib.TagTypes.Apple) as TagLib.Mpeg4.AppleTag;
            TagLib.Ape.Tag ape = fileTag.GetTag(TagLib.TagTypes.Ape) as TagLib.Ape.Tag;
            TagLib.Asf.Tag asf = fileTag.GetTag(TagLib.TagTypes.Asf) as TagLib.Asf.Tag;
            TagLib.Ogg.XiphComment ogg = fileTag.GetTag(TagLib.TagTypes.Xiph) as TagLib.Ogg.XiphComment;
        }

        public List<string> GetTags
        {
            get
            {
                return ChannelProps;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            ChannelProps.Clear();
            fileTag = null;
        }

        #endregion
    }
}
