using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Un4seen.Bass;
using Un4seen.Bass.AddOn;
using Un4seen.Bass.AddOn.Tags;
using TagLib;

namespace libIsh
{
    [DefaultPropertyAttribute("Title")]
    public class TagProperty
    {
        string              _album;
        string              _albumartist;
        string              _artist;
        string              _bpm;
        string              _comment;
        string              _composer;
        string              _disc;
        string              _genre;
        string              _title;
        string              _track;
        string              _year;  
        //string              _copyright;
        //string              _grouping;

        public TagProperty(Tag initedTag)
        {
            if (initedTag != null)
            {
                string sep = " , ";
                _album = (initedTag.Album != null ? initedTag.Album.ToString() : "");
                _albumartist = ArrayConv.StringArrayToString(initedTag.AlbumArtists, sep);
                _artist = ArrayConv.StringArrayToString(initedTag.Performers, sep);
                _bpm = (initedTag.BeatsPerMinute > 0 ? initedTag.BeatsPerMinute.ToString() : "");
                _comment = (initedTag.Comment != null ? initedTag.Comment.ToString() : "");
                _composer = ArrayConv.StringArrayToString(initedTag.Composers, sep);
                //_copyright      = initedTag.Copyright.ToString();
                _disc = (initedTag.Disc > 0 ? initedTag.Disc.ToString() : "");
                _genre = ArrayConv.StringArrayToString(initedTag.Genres, sep);
                //_grouping       = initedTag.Grouping.ToString();
                _title = (initedTag.Title != null ? initedTag.Title.ToString() : "");
                _track = (initedTag.Track > 0 ? initedTag.Track.ToString() : ""); 
                _year = (initedTag.Year > 0 ? initedTag.Year.ToString() : "");
            }
            else
            {
                _album = _albumartist = _artist = _bpm = _comment = _composer = _disc =
                    _genre = _track = _year = "";
                _title = "Unsupported Tag";
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
        
        [CategoryAttribute("Basic Info"), DescriptionAttribute("Simple Comment...")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public bool FormatSupports
        {
            get
            {
                return (_title != "Unsupported Tag" ? true : false);
            }
        }
    }
}
