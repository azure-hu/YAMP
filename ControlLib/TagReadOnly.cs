using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Un4seen.Bass;
using Un4seen.Bass.AddOn;
using Un4seen.Bass.AddOn.Tags;

namespace libIsh
{
    [DefaultPropertyAttribute("Duration")]
    public class Bass_TagReadOnly
    {
        int                 _bitrate;
        BASS_CHANNELINFO    _channelinfo;  
        string              _copyright;
        double              _duration;
        string              _encodedby;
        string              _filename;
        string              _grouping;
        string              _isrc;
        string              _mood;
        float               _replaygain_track_gain;
        float               _replaygain_track_peak;
        BASSTag             _tagType;


        public Bass_TagReadOnly(string FileName)
        {
            _bitrate               = BassTags.BASS_TAG_GetFromFile(FileName).bitrate;
            _channelinfo           = BassTags.BASS_TAG_GetFromFile(FileName).channelinfo;
            _copyright             = BassTags.BASS_TAG_GetFromFile(FileName).copyright;
            _duration              = BassTags.BASS_TAG_GetFromFile(FileName).duration;
            _encodedby             = BassTags.BASS_TAG_GetFromFile(FileName).encodedby;
            _filename              = BassTags.BASS_TAG_GetFromFile(FileName).filename;
            _grouping              = BassTags.BASS_TAG_GetFromFile(FileName).grouping;
            _isrc                  = BassTags.BASS_TAG_GetFromFile(FileName).isrc;
            _mood                  = BassTags.BASS_TAG_GetFromFile(FileName).mood;
            _replaygain_track_gain = BassTags.BASS_TAG_GetFromFile(FileName).replaygain_track_gain;
            _replaygain_track_peak = BassTags.BASS_TAG_GetFromFile(FileName).replaygain_track_peak;
            _tagType               = BassTags.BASS_TAG_GetFromFile(FileName).tagType;
        } 
    }
}
