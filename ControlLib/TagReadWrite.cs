using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Un4seen.Bass;
using Un4seen.Bass.AddOn;
using Un4seen.Bass.AddOn.Tags;

namespace libZoi
{
    [DefaultPropertyAttribute("Title")]
    public class Bass_TagReadWrite
    {
        string              _album;
        string              _albumartist;
        string              _artist;
        string              _bpm;
        //string              _comment;
        string              _composer;
        string              _disc;
        string              _genre;
        string              _publisher;
        string              _rating;
        string              _title;
        string              _track;
        string              _year;
        string              _filename;
        TAG_INFO            ID_BasicTags;
        string[]            ID_Array;



        public Bass_TagReadWrite(string FileName)
        {
            ID_BasicTags = new TAG_INFO();
            Update(FileName);
            ReadTags();
        }

        public Bass_TagReadWrite(int handle, string FileName)
        {
            ID_Array = new string[Bass.BASS_ChannelGetTagsMP4(handle).Length];
            Update(handle, FileName);
        }

        private void ReadTags()
        {
            if (ID_BasicTags != null)
            {
                _album          = ID_BasicTags.album.ToString();
                _albumartist    = ID_BasicTags.albumartist.ToString();
                _artist         = ID_BasicTags.artist.ToString();
                _bpm            = ID_BasicTags.bpm.ToString();
                //_comment        = ID_BasicTags.comment.ToString();
                _composer       = ID_BasicTags.composer.ToString();
                _disc           = ID_BasicTags.disc.ToString();
                _genre          = ID_BasicTags.genre.ToString();
                _publisher      = ID_BasicTags.publisher.ToString();
                _rating         = ID_BasicTags.rating.ToString();
                _title          = ID_BasicTags.title.ToString();
                _track          = ID_BasicTags.track.ToString();
                _year           = ID_BasicTags.year.ToString();
                _filename       = ID_BasicTags.filename;
            }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Artist of the track")]
        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Title of the track")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Publishing year of the track")]
        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Genre of the track")]
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Title of the album")]
        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }


        [CategoryAttribute("Basic Info"), DescriptionAttribute("Track no. on the Album")]
        public string Track
        {
            get { return _track; }
            set { _track = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Disc no. of the Album")]
        public string Disc
        {
            get { return _disc; }
            set { _disc = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Track Speed in Beats/Minute")]
        public string BPM
        {
            get { return _bpm; }
            set { _bpm = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Artist of the album")]
        public string AlbumArtist
        {
            get { return _albumartist; }
            set { _albumartist = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Composer of the track")]
        public string Composer
        {
            get { return _composer; }
            set { _composer = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Publisher of the track")]
        public string Publisher
        {
            get { return _publisher; }
            set { _publisher = value; }
        }

        [CategoryAttribute("Basic Info"), DescriptionAttribute("Rating of the track")]
        public string Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }


        /*
        [CategoryAttribute("Basic Info"), DescriptionAttribute("Simple Comment...")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        */

        //[CategoryAttribute("Basic Info"), DescriptionAttribute("Path of the file"), ReadOnlyAttribute(true)]
        public string Path
        {
            get { return _filename; }
        }

        public void Update(string FileName)
        {
            ID_BasicTags = BassTags.BASS_TAG_GetFromFile(FileName);
            ReadTags();
        }

        public void Update(int handle, string FileName)
        {
            if (FileName.EndsWith("mp4") || FileName.EndsWith("m4a") || FileName.EndsWith("aac"))
            {
                ID_Array = Bass.BASS_ChannelGetTagsMP4(handle);
            }
        }
    }
}
