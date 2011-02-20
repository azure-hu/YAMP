using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass.AddOn.Tags;

namespace libZoi
{
    public class MediaItem
    {
        private string fullPath;
        private String fileName;
        //TAG_INFO mediaTag;
        //string _length;

        /// <summary>
        /// Initializes the MediaItem from an input file.
        /// </summary>
        /// <param name="file">The Input File.</param>
        public MediaItem(string file)
        {
            fullPath = file;
            fileName = System.IO.Path.GetFileNameWithoutExtension(file);
            fileName.Replace('_',' ');
            Refresh();
        }

        /// <summary>
        /// Gets the short filename.
        /// </summary>
        public string getFileName 
        {
            get
            {
                return fileName;
            }
        }

        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string getFilePath 
        {
            get
            {
                return fullPath;
            }
        }

        public void Refresh()
        {
            /*
            mediaTag = BassEngine.GetTagFromFile(fullPath);
            if (mediaTag != null)
            {
                if ((mediaTag.artist == "") && (mediaTag.title == ""))
                {
                    mediaTag.artist = "";
                    mediaTag.title = fileName;
                }
                double seconds = mediaTag.duration;
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                _length = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }
             */
        }
		
		public string Length
		{
			get
			{
                string _length = "";
                TAG_INFO mediaTag = BassEngine.GetTagFromFile(fullPath);
                if (mediaTag != null)
                {
                    double seconds = mediaTag.duration;
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
    }
}
