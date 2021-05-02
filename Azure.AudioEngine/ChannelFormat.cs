using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;

namespace Azure.MediaUtils
{
    public static class ChannelFormat
    {
        public static string GetExtension(AudioEngine ae)
        {
            switch (ae.ChannelInfo.ctype)
            {
                case BASSChannelType.BASS_CTYPE_STREAM_OGG: return ".ogg";
                case BASSChannelType.BASS_CTYPE_STREAM_MP1: return ".mp1";
                case BASSChannelType.BASS_CTYPE_STREAM_MP2: return ".mp2";
                case BASSChannelType.BASS_CTYPE_STREAM_MP3: return ".mp3";
                case BASSChannelType.BASS_CTYPE_STREAM_WV:
                case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
                case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
                case BASSChannelType.BASS_CTYPE_STREAM_WV_LH: return ".wv";
                case BASSChannelType.BASS_CTYPE_STREAM_WMA:
                case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3: return ".wma";
                case BASSChannelType.BASS_CTYPE_STREAM_FLAC:
                case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG: return ".flac";
                case BASSChannelType.BASS_CTYPE_STREAM_OFR: return ".ofr";
                case BASSChannelType.BASS_CTYPE_STREAM_APE: return ".ape";
                case BASSChannelType.BASS_CTYPE_STREAM_MPC: return ".mpc";
                case BASSChannelType.BASS_CTYPE_STREAM_AAC: return ".aac";
                case BASSChannelType.BASS_CTYPE_STREAM_MP4: return ".m4a";
                case BASSChannelType.BASS_CTYPE_STREAM_SPX: return ".spx";
                case BASSChannelType.BASS_CTYPE_STREAM_ALAC: return ".alac";
                case BASSChannelType.BASS_CTYPE_STREAM_TTA: return ".tta";
                case BASSChannelType.BASS_CTYPE_STREAM_AC3: return ".ac3";
                case BASSChannelType.BASS_CTYPE_STREAM_OPUS: return ".opus";
                case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
                case BASSChannelType.BASS_CTYPE_STREAM_WAV:
                case BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM:
                case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT: return ".wav";
                default: return string.Empty;
            }
        }

    }
}
