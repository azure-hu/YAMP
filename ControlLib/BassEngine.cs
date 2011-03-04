using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;

#region using Bass.Net

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Aac;
using Un4seen.Bass.AddOn.Ac3;
using Un4seen.Bass.AddOn.Alac;
using Un4seen.Bass.AddOn.Ape;
using Un4seen.Bass.AddOn.Flac;
using Un4seen.Bass.AddOn.Mpc;
using Un4seen.Bass.AddOn.Wma;
using Un4seen.Bass.AddOn.Wv;
using Un4seen.Bass.AddOn.Tags;
using Un4seen.Bass.Misc;
using Un4seen.Bass.AddOn.Tta;
using Un4seen.Bass.AddOn.Spx;
using Un4seen.Bass.AddOn.Ofr;

#endregion

namespace libZoi
{
    public static class BassEngine
    {
        #region variables

        private static int stream;
        private static string supDefault;
        private static string supAll;
        private static Dictionary<int, string> plugins = new Dictionary<int, string>();
        private static FileStream _fs;

        #region letoltesi proba
        private static IntPtr dlPointer;
        private static DOWNLOADPROC tryDownload;
        private static byte[] _data; // local data buffer
        private static string saveFile = "output";
        #endregion

        #endregion variables

        #region methods

        /// <summary>
        /// Initializes BassEngine to get ready for use.
        /// </summary>
        /// <param name="LibPath">Path of DLL files.</param>
        public static void Init(string LibPath)
        {
            try
            {
                BassNet.Registration("flashmark@ymail.com", "2X1652019342222");
                if (Bass.LoadMe(LibPath))
                {
                    if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT | 
                        BASSInit.BASS_DEVICE_LATENCY | BASSInit.BASS_DEVICE_CPSPEAKERS, IntPtr.Zero))
                    {
                        throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                    }
                    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 2000);
                    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, true);
                    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 2000);
                    supDefault = ConcatForFilter(Bass.SupportedStreamName, Bass.SupportedStreamExtensions);
                    supAll = "All|*.*";
                        
                }
                else
                {
                    throw new Exception("Audio Library Not Found!");
                }


                plugins = Bass.BASS_PluginLoadDirectory(LibPath);
                supAll += "|" + Utils.BASSAddOnGetPluginFileFilter(plugins, null);
                
            }
            catch (Exception Ex)
            {
                MBoxHelper.ShowErrorMsg(Ex, "BASS Init Error!");
            }
        }

        private static string ConcatForFilter(string name, string exts)
        {
            if (exts != null)
                return name + "|" + exts;
            else
                return name;
        }

        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public static string getSupFilter
        {
            get
            {
                return supAll;
            }
        }

        public static int GetStreamHandler()
        {
            return stream;
        }

        public static double masterVolume
        {
            get
            {
                return (double)Math.Floor((Bass.BASS_GetVolume() * 100));
            }
            set
            {
                double newVol = Math.Floor(value) / 100;
                if (!Bass.BASS_SetVolume((float)newVol))
                {
                    if (newVol < 0)
                        Bass.BASS_SetVolume(0F);
                    else
                        Bass.BASS_SetVolume(1F);
                }
            }
        }

        public static bool Pause()
        {
            try
            {
                if (InternalStatus() == BASSActive.BASS_ACTIVE_PAUSED)
                {
                    Bass.BASS_ChannelPlay(stream, false);
                }
                else
                {
                    Bass.BASS_ChannelPause(stream);
                }

                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Pause");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Play(string filePath)
        {
            if (!Play())
            {
                PlayInitFile(filePath);
            }
        }

        public static bool Play()
        {
            try
            {
                if (InternalStatus() == BASSActive.BASS_ACTIVE_PAUSED)
                {
                    Bass.BASS_ChannelPlay(stream, false);
                }
                else
                {
                    Bass.BASS_Start();
                    Bass.BASS_ChannelUpdate(stream, 0);
                    if(!Bass.BASS_ChannelPlay(stream, true))
                        MessageBox.Show(Bass.BASS_ErrorGetCode().ToString()); ;
                }

                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Play:" + Bass.BASS_ErrorGetCode());
                }

                if (FireWorks.IsReady)
                    FireWorks.StartDraw(stream);
                return true;
            }
            catch (Exception x)
            {
                MBoxHelper.Whoops(x.Message);
                return false;
            }
        }

        public static bool Stop()
        {
            try
            {
                Bass.BASS_ChannelStop(stream);
                if(FireWorks.IsReady)
                    FireWorks.StopDraw();
                Bass.BASS_Stop();
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Stop");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ListenInternetStream(Form fm, string formTitle, string StrUrl, string StrName, 
            int TimeOut, bool Record, string fileToRec)
        {
            if(fm != null)
                fm.Text = formTitle + " # Connecting...";
            int i = 0;
            BASSActive baInd;
            do
            {
                saveFile = fileToRec;
                PlayInitUrl(StrUrl, Record);
                //Thread.Sleep(1000);
                Application.DoEvents();
                ++i;
                baInd = InternalStatus();
            }
            while ((baInd == BASSActive.BASS_ACTIVE_STOPPED) && (i < TimeOut));
            if (baInd != BASSActive.BASS_ACTIVE_STOPPED)
            {
                if (FireWorks.IsReady)
                    FireWorks.StartDraw(stream);
                if (fm != null)
                    fm.Text = formTitle + (Record ? " # Recording: " : " # Playing: ") + StrName;
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int getStatus()
        {
            switch (Bass.BASS_ChannelIsActive(stream))
            {
                case BASSActive.BASS_ACTIVE_PLAYING: return 1;

                case BASSActive.BASS_ACTIVE_PAUSED: return -1;

                case BASSActive.BASS_ACTIVE_STALLED: return -2;

                case BASSActive.BASS_ACTIVE_STOPPED: return 0;

                default: return 0;
            }
        }

        private static BASSActive InternalStatus()
        {
            return Bass.BASS_ChannelIsActive(stream);
        }

        #region internet stream

        public static void PlayInitUrl(string StrUrl, bool Record)
        {
            BASSFlag flag = BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_AUTOFREE;
            if (!Record)
            {
                stream = Bass.BASS_StreamCreateURL(StrUrl, 0, flag, null, IntPtr.Zero);
                if (stream == 0)
                {
                    stream = BassAac.BASS_AAC_StreamCreateURL(StrUrl, 0, flag, null, IntPtr.Zero);
                }
            }
            else
            {
                tryDownload = new DOWNLOADPROC(MyDownload);
                stream = Bass.BASS_StreamCreateURL(StrUrl, 0, flag, tryDownload, dlPointer);
                if (stream == 0)
                {
                    stream = BassAac.BASS_AAC_StreamCreateURL(StrUrl, 0, flag, tryDownload, dlPointer);
                }
            }
            if (stream == 0)
            {
                stream = BassWma.BASS_WMA_StreamCreateURL(StrUrl, 0, 0, flag);
            }
            Play();
        }
  
        private static void MyDownload(IntPtr buffer, int length, IntPtr user)
        {
            
            if (_fs == null)
            {
                // create the file
                _fs = File.OpenWrite(saveFile);
                //_fs.WriteTimeout = 3000;
            }
            if (buffer == IntPtr.Zero)
            {
                // finished downloading
                _fs.Flush();
                _fs.Close();
            }
            else
            {
                // increase the data buffer as needed
                if (_data == null || _data.Length < length)
                    _data = new byte[length];
                // copy from managed to unmanaged memory
                System.Runtime.InteropServices.Marshal.Copy(buffer, _data, 0, length);
                // write to file
                _fs.Write(_data, 0, length);
            }

        }

        public static bool ShutDownNet()
        {
            try
            {
                Stop();
                if (_fs != null)
                {
                    _fs.Flush();
                    _fs.Close();
                    
                    dlPointer = IntPtr.Zero;
                    tryDownload = null;
                    _data = null;
                    saveFile = "output";
                }
                
                return true;
            }
            catch 
            {
                _fs = null;
                return false;
            }
        }

        #endregion

        public static void PlayInitFile(string filePath)
        {
            BASS_INFO info = Bass.BASS_GetInfo();
            BASSFlag flag = (info.speakers < 3 ? BASSFlag.BASS_AAC_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 : BASSFlag.BASS_DEFAULT)
                | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_PRESCAN;

            Format _fmt = GetFormat(filePath);
            BASSFlag _flag = BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE;
            switch (_fmt)
            {
                case Format.UNSUP: MBoxHelper.ShowErrorMsg("Unsupported File Format!", "Format Error!");
                    break;
                case Format.Default: stream = Bass.BASS_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.AAC: stream = BassAac.BASS_AAC_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.MP4: stream = BassAac.BASS_MP4_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.AC3: stream = BassAc3.BASS_AC3_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.ALAC: stream = BassAlac.BASS_ALAC_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.APE: stream = BassApe.BASS_APE_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.FLAC: stream = BassFlac.BASS_FLAC_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.MPC: stream = BassMpc.BASS_MPC_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.FROG: stream = BassOfr.BASS_OFR_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.SPX: stream = BassSpx.BASS_SPX_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.TTA: stream = BassTta.BASS_TTA_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.WMA: stream = BassWma.BASS_WMA_StreamCreateFile(filePath, 0, 0, _flag); break;
                case Format.WV: stream = BassWv.BASS_WV_StreamCreateFile(filePath, 0, 0, _flag); break;
            }
            if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
            {
                MessageBox.Show(Bass.BASS_ErrorGetCode().ToString(), "Engine Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Play();
        }

        private static Format GetFormat(string p)
        {
            if (Utils.BASSAddOnIsFileSupported(plugins, p))
            {
                string _ext = Path.GetExtension(p);
                if (Bass.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.Default;
                }
                if (BassAac.SupportedStreamExtensions.Contains(_ext))
                {
                    if(_ext.StartsWith(".m"))
                        return Format.MP4;
                    return Format.AAC;
                }
                if (BassWma.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.WMA;
                }
                if (BassFlac.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.FLAC;
                }
                if (BassAlac.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.ALAC;
                }
                if (BassAc3.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.AC3;
                }
                if (BassApe.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.APE;
                }
                if (BassMpc.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.MPC;
                }
                if (BassOfr.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.FROG;
                }
                if (BassSpx.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.SPX;
                }
                if (BassTta.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.TTA;
                }
                if (BassWv.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.WV;
                }
            }
            return Format.UNSUP;
        }

        public static long getCurrentPosition()
        {
            return Bass.BASS_ChannelGetPosition(stream);
        }

        public static long getCurrentLength()
        {
            return Bass.BASS_ChannelGetLength(stream);
        }

        public static double getCurrentLengthSeconds()
        {
            return Math.Round(Bass.BASS_ChannelBytes2Seconds(stream, getCurrentLength()));
        }

        public static bool setPosition(long newPos)
        {
            try
            {
                Bass.BASS_ChannelSetPosition(stream, newPos, BASSMode.BASS_POS_BYTES);
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("Engine - File Positioning Error!");
                }
                else
                {
                  return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static bool setPositionSeconds(double newPos)
        {
            return setPosition(Bass.BASS_ChannelSeconds2Bytes(stream, Math.Floor(newPos)));
        }

        public static double getCurrentPositionSeconds()
        {
            return Math.Floor(Bass.BASS_ChannelBytes2Seconds(stream, getCurrentPosition()));
        }

        public static TimeSpan CurrentPosition()
        {
           return TimeSpan.FromSeconds(getCurrentPositionSeconds());
        }

        public static string CurrentPositionString()
        {
            return (String.Format("{0:D2}:{1:D2}:{2:D2}", (int)CurrentPosition().TotalHours,
                CurrentPosition().Minutes, CurrentPosition().Seconds));
        }

        public static bool setRepeat()
        {
            try
            {
                
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Position Reset!");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Use only if you want to shut down BassEngine.
        /// </summary>
        public static void Clean()
        {            
            Bass.BASS_StreamFree(stream);
            Bass.BASS_PluginFree(0);
            Bass.BASS_Free();
            
            stream = 0;
            FireWorks.StopDraw();
            supAll = supDefault = null;
        }

        public static void Rewind()
        {
            Bass.BASS_ChannelSetPosition(stream, 0.0);
        }

        #endregion methods

        public static void Wipe()
        {
            Bass.BASS_StreamFree(stream);
            System.Threading.Thread.Sleep(100);
        }

        internal static TAG_INFO GetTagFromFile(string _file)
        {
            return BassTags.BASS_TAG_GetFromFile(_file);
        }

        internal enum Format
        {
            Default, AAC, MP4, AC3, ALAC, APE, FLAC, MPC, FROG, SPX, TTA, WMA, WV, UNSUP
        }

    }
}
