using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
using Un4seen.Bass.AddOn.Fx;

#endregion

namespace Azure.LibCollection.CS.AudioWorks
{
    public static class AudioEngine
    {
        #region variables

        private static int stream;
        private static string supDefault;
        private static string supAll;
        private static Dictionary<int, string> plugins = new Dictionary<int, string>();
        private static FileStream _fs;
        private static StreamMode streamMode;
        private static float chVol;
        private static string _currentFile;
        private static bool recOnlineStream;
        private static string urlOnlineStream;
        private static int defDevice;
        private static Dictionary<int, BASS_DEVICEINFO> _outDevices = new Dictionary<int, BASS_DEVICEINFO>();
        private static string libPath;
        public static string LibraryPath { get { return libPath; } }

        private static BASSActive InternalStatus
        {
            get { return Bass.BASS_ChannelIsActive(stream); }
        }

        public static short ChannelStatus
        {
            get
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
        }

        #region letoltesi proba
        private static IntPtr dlPointer;
        private static DOWNLOADPROC tryDownload;
        private static byte[] _data; // local data buffer
        private static string saveFile = "output";
        private static IntPtr _handle;
        private static string bassVersion;

        #endregion

        #endregion variables

        #region methods

        /// <summary>
        /// Initializes AudioEngine to get ready for use.
        /// </summary>
        /// <param name="LibPath">Path of DLL files.</param>
        public static bool Init(string LibPath, IntPtr hwnd)
        {
            defDevice = -1;
            return InitCore(LibPath, hwnd);
        }

        public static Dictionary<int, string> Init2(string LibPath, IntPtr hwnd)
        {
            defDevice = -1;
            return Init2(LibPath, hwnd, defDevice);
        }

        private static Dictionary<int, string> Init2(string LibPath, IntPtr hwnd, int device)
        {
            if (InitCore(LibPath, hwnd))
            {
                Dictionary<int, string> outputs = AudioEngine.GetAvailableOutputs();
                if (_outDevices.ContainsKey(device))
                {
                    defDevice = device;
                }
                return outputs;
            }
            else
                return null;
        }

        private static bool InitCore(string LibPath, IntPtr hwnd)
        {
            bool flag;
            try
            {
                //BassNet.Registration("flashmark@ymail.com", "2X1652019342222");
                BassNet.Registration("zollai@outlook.com", "2X3924291824822");
                BassNet.OmitCheckVersion = true;
                libPath = LibPath;
                _handle = hwnd;
                
                bool libLoaded = Bass.LoadMe(libPath);
                
                if (!libLoaded)
                {
                    throw new Exception(string.Concat("Audio Library Not Found!", Environment.NewLine, Bass.BASS_ErrorGetCode().ToString()));
                }
                
                bassVersion = Bass.BASS_GetVersion().ToString();
                if (!Bass.BASS_Init(defDevice, 44100, BASSInit.BASS_DEVICE_LATENCY | BASSInit.BASS_DEVICE_CPSPEAKERS, _handle))
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 2000);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 2000);

                plugins = Bass.BASS_PluginLoadDirectory(libPath);
                supDefault = AudioEngine.ConcatForFilter(Bass.SupportedStreamName, Bass.SupportedStreamExtensions);
                supAll = string.Concat(Utils.BASSAddOnGetSupportedFileFilter(plugins, "All supported formats", true), "|", "All|*.*");                
                //supAll = string.Concat(supAll, "|", Utils.BASSAddOnGetPluginFileFilter(plugins, null));
                flag = true;
            }
            catch (Exception exception)
            {
                //MBoxHelper.ShowErrorMsg(exception, "BASS Init Error!");
                flag = false;
                throw new Exception(exception.Message, exception.InnerException);
            }
            return flag;
        }

        private static void QueryOutputDevices(out int _defDevice)
        {
            _defDevice = -1;
            if (_outDevices.Count > 0)
            {
                _outDevices.Clear();
            }
            BASS_DEVICEINFO[] _devArr = Bass.BASS_GetDeviceInfos();
            for (int n = 0; n < _devArr.Length; n++)
            {
                if (_devArr[n].IsEnabled)
                {
                    _outDevices.Add(n, _devArr[n]);
                }

                if (_devArr[n].IsDefault)
                {
                    _defDevice = n;
                }
            }
        }

        public static Dictionary<int, string> GetAvailableOutputs()
        {
            int _defDevice = -1;
            AudioEngine.QueryOutputDevices(out _defDevice);
            Dictionary<int, string> _outputs = new Dictionary<int, string>();
            foreach (var _out in _outDevices)
            {
                _outputs.Add(_out.Key, _out.Value.name);
            }
            if (defDevice == -1 && _defDevice != -1)
            {
                defDevice = _defDevice;
            }
            return _outputs;
        }

        public static KeyValuePair<int, string> GetActiveOutput()
        {
            KeyValuePair<int, string> _active = new KeyValuePair<int, string>(-1, "<system default>");
            foreach (var _out in _outDevices)
            {
                if (_out.Key == defDevice)
                {
                    _active = new KeyValuePair<int, string>(_out.Key, _out.Value.name);
                    break;
                }
            }
            return _active;
        }

        public static KeyValuePair<int, string> GetDefaultOutput()
        {
            KeyValuePair<int, string> _default = new KeyValuePair<int, string>(-1, "<system default>");
            foreach (var _out in _outDevices)
            {
                if (_out.Value.IsDefault)
                {
                    _default = new KeyValuePair<int, string>(_out.Key, _out.Value.name);
                    break;
                }
            }
            return _default;
        }

        public static bool SwitchOutputDevice(int device)
        {
            if (!_outDevices[device].IsInitialized)
            {
                if (!Bass.BASS_Init(device, 44100, BASSInit.BASS_DEVICE_LATENCY | BASSInit.BASS_DEVICE_CPSPEAKERS, _handle))
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
            }

            bool _success = Bass.BASS_SetDevice(device);
            if (AudioEngine.InternalStatus != BASSActive.BASS_ACTIVE_STOPPED)
            {
                _success = _success && Bass.BASS_ChannelSetDevice(stream, device);
            }
            _outDevices[defDevice] = Bass.BASS_GetDeviceInfo(defDevice);
            _outDevices[device] = Bass.BASS_GetDeviceInfo(device);
            if (_success)
            {
                defDevice = device;
            }
            return _success;
        }

        internal enum Format
        {
            Default, PluginSupported, UnSupported
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string ProcessorArchitecture
        {
            get
            {
                switch (Utils.Is64Bit)
                {
                    case true: return "x64";
                    case false: return "x86";
                    default: return "unknown"; // that's weird :-)
                }
            }
        }

        internal static double GetLengthFromFile(string fullPath)
        {
            return 0; // need to be implemeted ...
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

        public static float masterVolume
        {
            get
            {
                return Bass.BASS_GetVolume();
            }
            set
            {
                if (!Bass.BASS_SetVolume(value))
                {
                    if (value < 0)
                        Bass.BASS_SetVolume(0F);
                    else
                        Bass.BASS_SetVolume(1F);
                }
            }
        }

        public static float channelVolume
        {
            get
            {
                /*float _chVol = -1f;
                if(Bass.BASS_ChannelGetAttribute(AudioEngine.GetStreamHandler(), BASSAttribute.BASS_ATTRIB_VOL, ref _chVol))
                    chVol = _chVol;*/
                return chVol;
            }
            set
            {
                float _chVol = -1f;
                if (value < 0f)
                    _chVol = 0f;
                else
                {
                    if (value > 1f)
                        _chVol = 1f;
                    else
                        _chVol = value;
                }

                Bass.BASS_ChannelSetAttribute(AudioEngine.GetStreamHandler(), BASSAttribute.BASS_ATTRIB_VOL, _chVol);
                chVol = _chVol;
            }
        }

        public static bool Pause()
        {
            try
            {
                if (AudioEngine.InternalStatus == BASSActive.BASS_ACTIVE_PAUSED)
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

        /*
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
                    //Bass.BASS_Start();
                    Bass.BASS_ChannelUpdate(stream, 0);
                    AudioEngine.channelVolume = chVol;
                    int _tries = 0;
                    BASSError _err;
                    do
                    {
                        if (!Bass.BASS_ChannelPlay(stream, true))
                        {
                            CreateFileStream(_currentFile);
                            _tries++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while(_tries < 3);
                    
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
        */

        public static bool Play()
        {
            bool flag;
            try
            {
                if (AudioEngine.InternalStatus != BASSActive.BASS_ACTIVE_PAUSED)
                {
                    Bass.BASS_ChannelUpdate(AudioEngine.stream, 0);
                    AudioEngine.channelVolume = AudioEngine.chVol;
                    int _tries = 0;
                    do
                    {
                        if (Bass.BASS_ChannelPlay(AudioEngine.stream, true))
                        {
                            break;
                        }
                        else
                        {
                            switch (AudioEngine.streamMode)
                            {
                                case StreamMode.File:
                                    {
                                        AudioEngine.CreateFileStream(AudioEngine._currentFile);
                                        break;
                                    }
                                case StreamMode.Online:
                                    {
                                        AudioEngine.InitOnlineStream();
                                        break;
                                    }
                            }
                            _tries++;
                        }
                    }
                    while (_tries < 3);
                }
                else
                {
                    Bass.BASS_ChannelPlay(AudioEngine.stream, false);
                }
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception(string.Concat("BASS Error on Play:", Bass.BASS_ErrorGetCode()));
                }
                if (FireWorks.IsReady)
                {
                    FireWorks.StartDraw(AudioEngine.stream);
                }
                flag = true;
            }
            catch (Exception exception)
            {
                MBoxHelper.Whoops(exception.Message);
                flag = false;
            }
            return flag;
        }

        public static bool Stop()
        {
            try
            {
                if (Bass.BASS_ChannelIsActive(stream) != BASSActive.BASS_ACTIVE_STOPPED)
                {
                    Bass.BASS_ChannelStop(stream);
                }

                if (FireWorks.IsReady)
                    FireWorks.StopDraw();
                //Bass.BASS_Stop();
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
            if (fm != null)
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
                baInd = AudioEngine.InternalStatus;
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

        public static void SetOutput(string p)
        {
            AudioEngine.saveFile = p;
        }

        #region internet stream

        //public static void PlayInitUrl(string StrUrl, bool Record)
        //{
        //    BASSFlag flag = BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_AUTOFREE;
        //    if (!Record)
        //    {
        //        stream = Bass.BASS_StreamCreateURL(StrUrl, 0, flag, null, IntPtr.Zero);
        //        if (stream == 0)
        //        {
        //            stream = BassAac.BASS_AAC_StreamCreateURL(StrUrl, 0, flag, null, IntPtr.Zero);
        //        }
        //    }
        //    else
        //    {
        //        tryDownload = new DOWNLOADPROC(MyDownload);
        //        stream = Bass.BASS_StreamCreateURL(StrUrl, 0, flag, tryDownload, dlPointer);
        //        if (stream == 0)
        //        {
        //            stream = BassAac.BASS_AAC_StreamCreateURL(StrUrl, 0, flag, tryDownload, dlPointer);
        //        }
        //    }
        //    if (stream == 0)
        //    {
        //        stream = BassWma.BASS_WMA_StreamCreateURL(StrUrl, 0, 0, flag);
        //    }
        //    Play();
        //}

        //private static void MyDownload(IntPtr buffer, int length, IntPtr user)
        //{

        //    if (_fs == null)
        //    {
        //        // create the file
        //        _fs = File.OpenWrite(saveFile);
        //        //_fs.WriteTimeout = 3000;
        //    }
        //    if (buffer == IntPtr.Zero)
        //    {
        //        // finished downloading
        //        _fs.Flush();
        //        _fs.Close();
        //    }
        //    else
        //    {
        //        // increase the data buffer as needed
        //        if (_data == null || _data.Length < length)
        //            _data = new byte[length];
        //        // copy from managed to unmanaged memory
        //        System.Runtime.InteropServices.Marshal.Copy(buffer, _data, 0, length);
        //        // write to file
        //        _fs.Write(_data, 0, length);
        //    }

        //}

        private static void MyDownload(IntPtr buffer, int length, IntPtr user)
        {
            if (AudioEngine._fs == null)
            {
                AudioEngine._fs = File.OpenWrite(AudioEngine.saveFile);
            }
            if (!(buffer == IntPtr.Zero))
            {
                if ((AudioEngine._data == null ? true : (int)AudioEngine._data.Length < length))
                {
                    AudioEngine._data = new byte[length];
                }
                Marshal.Copy(buffer, AudioEngine._data, 0, length);
                AudioEngine._fs.Write(AudioEngine._data, 0, length);
            }
            else
            {
                AudioEngine._fs.Flush();
                AudioEngine._fs.Close();
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
            _currentFile = filePath;
            CreateFileStream(_currentFile);

            if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
            {
                MessageBox.Show(Bass.BASS_ErrorGetCode().ToString(), "Engine Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Play();
        }

        private static void CreateFileStream(string filePath)
        {
            BASS_INFO info = Bass.BASS_GetInfo();
            BASSFlag _flag = (info.speakers < 3 ? BASSFlag.BASS_AAC_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 : BASSFlag.BASS_DEFAULT)
                | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE;

            Format _fmt = GetFormat(filePath);
            switch (_fmt)
            {
                case Format.UnSupported: MBoxHelper.ShowErrorMsg("Unsupported File Format!", "Format Error!");
                    break;
                case Format.Default:
                case Format.PluginSupported:
                    {
                        stream = Bass.BASS_StreamCreateFile(filePath, 0, 0, _flag);
                        AudioEngine.channelVolume = chVol;
                    } break;

            }
        }

        private static Format GetFormat(string p)
        {
            if (Utils.BASSAddOnIsFileSupported(plugins, p))
            {
                string _ext = Path.GetExtension(p);
                if (Bass.SupportedStreamExtensions.Contains(_ext))
                    return Format.Default;
                else
                    return Format.PluginSupported;
            }
            else
                return Format.UnSupported;
        }

        public static long LengthInBytes
        {
            get { return Bass.BASS_ChannelGetLength(stream); /* length in bytes */}
        }

        public static long PositionInBytes
        {
            get { return Bass.BASS_ChannelGetPosition(stream); /* position in bytes */}
            set
            {
                Bass.BASS_ChannelSetPosition(stream, value, BASSMode.BASS_POS_BYTES);
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("Engine - File Positioning Error!");
                }
            }
        }

        public static double TotalTime
        {
            get { return Bass.BASS_ChannelBytes2Seconds(stream, AudioEngine.LengthInBytes); /* the total time length */}
        }

        public static double ElapsedTime
        {
            get { return Bass.BASS_ChannelBytes2Seconds(stream, AudioEngine.PositionInBytes); /* the elapsed time length */}
        }

        public static double RemainingTime
        {
            get
            {
                double remainingtime = AudioEngine.TotalTime - AudioEngine.ElapsedTime;
                return remainingtime;
            }
        }

        public static TimeSpan CurrentPosition
        {
            get
            {
                return TimeSpan.FromSeconds(AudioEngine.ElapsedTime);
            }
        }



        public static string ElapsedTimeString { get { return Utils.FixTimespan(AudioEngine.ElapsedTime, "MMSS"); } }

        public static string RemainingTimeString { get { return Utils.FixTimespan(AudioEngine.RemainingTime, "MMSS"); } }


        /*
        public static string CurrentPositionString()
        {
            return (String.Format("{0}:{1:D2}", AudioEngine.CurrentPosition.TotalMinutes, AudioEngine.CurrentPosition.Seconds));

            //Utils.FixTimespan(elapsedtime,"MMSS")
        }
        */
        /*
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
            catch (Exception)
            {
                return false;
            }
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

        public static bool setPositionSeconds(double newPos)
        {
            return setPosition(Bass.BASS_ChannelSeconds2Bytes(stream, Math.Floor(newPos)));
        }

        public static double getCurrentPositionSeconds()
        {
            return Math.Floor(Bass.BASS_ChannelBytes2Seconds(stream, getCurrentPosition()));
        }
        */

        public static string ChannelGetInfo(string param)
        {
            BASS_CHANNELINFO _info = Bass.BASS_ChannelGetInfo(stream);
            string _return = string.Empty;
            switch (param)
            {
                case "chan": _return = _info.chans.ToString(); break;
                case "freq": _return = _info.freq.ToString(); break;
                case "chanStr": _return = Utils.ChannelNumberToString(_info.chans); break;
                case "elapsedTime": _return = Utils.FixTimespan(AudioEngine.ElapsedTime, "MMSS"); break;
                case "remainingTime": _return = Utils.FixTimespan(AudioEngine.RemainingTime, "MMSS"); break;
                case "totalTime": _return = Utils.FixTimespan(AudioEngine.TotalTime, "MMSS"); break;
                default:
                    break;
            }
            return _return;
        }

        #region online stuffs

        public static string[] GetOnlineInfo()
        {
            string item;
            string[] strArrays;
            int i;
            string[] tags1 = Bass.BASS_ChannelGetTagsICY(AudioEngine.stream);
            string[] tags2 = Bass.BASS_ChannelGetTagsMETA(AudioEngine.stream);
            string[] tags3 = Bass.BASS_ChannelGetTagsHTTP(AudioEngine.stream);
            List<string> tags = new List<string>();
            if (tags1 != null)
            {
                strArrays = tags1;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    item = strArrays[i];
                    if (item.Contains("StreamTitle='"))
                    {
                        tags.Add(item.Replace("StreamTitle='", "").Replace("';", ""));
                    }
                }
            }
            if (tags2 != null)
            {
                strArrays = tags2;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    item = strArrays[i];
                    if (item.Contains("StreamTitle='"))
                    {
                        tags.Add(item.Replace("StreamTitle='", "").Replace("';", ""));
                    }
                }
            }
            if (tags3 != null)
            {
                strArrays = tags3;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    item = strArrays[i];
                    if (item.Contains("icy-description:"))
                    {
                        tags.Add(string.Concat(item.Replace("icy-description:", " ("), " )"));
                    }
                }
            }
            return tags.ToArray();
        }

        public static void PlayInitUrl(string StrUrl, bool Record)
        {
            AudioEngine.InitOnlineStream(StrUrl, Record);
            AudioEngine.Play();
        }

        public static void InitOnlineStream(string StrUrl, bool Record)
        {
            AudioEngine.urlOnlineStream = StrUrl;
            AudioEngine.recOnlineStream = Record;
            AudioEngine.streamMode = StreamMode.Online;
            AudioEngine.InitOnlineStream();
        }

        private static void InitOnlineStream()
        {
            DOWNLOADPROC dOWNLOADPROC;
            BASSFlag flag = BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_MIXER_NORAMPIN | BASSFlag.BASS_MIDI_SINCINTER | BASSFlag.BASS_MUSIC_AUTOFREE | BASSFlag.BASS_MUSIC_SINCINTER;
            if (AudioEngine.recOnlineStream)
            {
                dOWNLOADPROC = new DOWNLOADPROC(AudioEngine.MyDownload);
            }
            else
            {
                dOWNLOADPROC = null;
            }
            AudioEngine.tryDownload = dOWNLOADPROC;
            IntPtr user = (AudioEngine.recOnlineStream ? AudioEngine.dlPointer : IntPtr.Zero);
            AudioEngine.stream = Bass.BASS_StreamCreateURL(AudioEngine.urlOnlineStream, 0, flag, AudioEngine.tryDownload, user);
            BASSChannelType bASSChannelType = Bass.BASS_ChannelGetInfo(AudioEngine.stream).ctype;
            if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_MP4)
            {
                if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
                {
                    if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_MF)
                    {
                        switch (bASSChannelType)
                        {
                            case BASSChannelType.BASS_CTYPE_UNKNOWN:
                                {
                                    break;
                                }
                            case BASSChannelType.BASS_CTYPE_SAMPLE:
                                {
                                    break;
                                }
                            case BASSChannelType.BASS_CTYPE_RECORD:
                                {
                                    break;
                                }
                            default:
                                {
                                    if (bASSChannelType == BASSChannelType.BASS_CTYPE_MUSIC_MO3)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        switch (bASSChannelType)
                                        {
                                            case BASSChannelType.BASS_CTYPE_STREAM:
                                                {
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_SAMPLE | BASSChannelType.BASS_CTYPE_STREAM:
                                                {
                                                    return;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                                                {
                                                    if (!AudioEngine.saveFile.EndsWith(".ogg"))
                                                    {
                                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".ogg");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP1:
                                                {
                                                    if (!AudioEngine.saveFile.EndsWith(".mp1"))
                                                    {
                                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mp1");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP2:
                                                {
                                                    if (!AudioEngine.saveFile.EndsWith(".mp2"))
                                                    {
                                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mp2");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP3:
                                                {
                                                    if (!AudioEngine.saveFile.EndsWith(".mp3"))
                                                    {
                                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mp3");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
                                                {
                                                    if (!AudioEngine.saveFile.EndsWith(".wav"))
                                                    {
                                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".wav");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_CA:
                                                {
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MF:
                                                {
                                                    break;
                                                }
                                            default:
                                                {
                                                    return;
                                                }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    else if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_CD)
                    {
                        switch (bASSChannelType)
                        {
                            case BASSChannelType.BASS_CTYPE_STREAM_WMA:
                                {
                                    if (!AudioEngine.saveFile.EndsWith(".wma"))
                                    {
                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".wma");
                                    }
                                    break;
                                }
                            case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3:
                                {
                                    if (!AudioEngine.saveFile.EndsWith(".mp3"))
                                    {
                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mp3");
                                    }
                                    break;
                                }
                            default:
                                {
                                    if (bASSChannelType == BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                        }
                    }
                }
                else if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_APE)
                {
                    switch (bASSChannelType)
                    {
                        case BASSChannelType.BASS_CTYPE_STREAM_WV:
                            {
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
                            {
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
                            {
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_WV_LH:
                            {
                                break;
                            }
                        default:
                            {
                                if (bASSChannelType == BASSChannelType.BASS_CTYPE_STREAM_OFR)
                                {
                                    break;
                                }
                                else if (bASSChannelType == BASSChannelType.BASS_CTYPE_STREAM_APE)
                                {
                                    if (!AudioEngine.saveFile.EndsWith(".ape"))
                                    {
                                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".ape");
                                    }
                                    break;
                                }
                                else
                                {
                                    return;
                                }
                            }
                    }
                }
                else if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG)
                {
                    switch (bASSChannelType)
                    {
                        case BASSChannelType.BASS_CTYPE_STREAM_MIXER:
                            {
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_SPLIT:
                            {
                                break;
                            }
                        default:
                            {
                                switch (bASSChannelType)
                                {
                                    case BASSChannelType.BASS_CTYPE_STREAM_FLAC:
                                        {
                                            if (!AudioEngine.saveFile.EndsWith(".flac"))
                                            {
                                                AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".flac");
                                            }
                                            break;
                                        }
                                    case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG:
                                        {
                                            break;
                                        }
                                    default:
                                        {
                                            return;
                                        }
                                }
                                break;
                            }
                    }
                }
                else if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_MPC)
                {
                    switch (bASSChannelType)
                    {
                        case BASSChannelType.BASS_CTYPE_STREAM_AAC:
                            {
                                if (!AudioEngine.saveFile.EndsWith(".aac"))
                                {
                                    AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".aac");
                                }
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_MP4:
                            {
                                if (!AudioEngine.saveFile.EndsWith(".mp4"))
                                {
                                    AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mp4");
                                }
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
                else if (!AudioEngine.saveFile.EndsWith(".mpc"))
                {
                    AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".mpc");
                }
            }
            else if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_VIDEO)
            {
                if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_ALAC)
                {
                    if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_SPX)
                    {
                        if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_MIDI)
                        {
                            if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_ALAC)
                            {
                                return;
                            }
                            if (!AudioEngine.saveFile.EndsWith(".alac"))
                            {
                                AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".alac");
                            }
                        }
                    }
                }
                else if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_TTA)
                {
                    if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_AC3)
                    {
                        if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_VIDEO)
                        {
                            return;
                        }
                    }
                    else if (!AudioEngine.saveFile.EndsWith(".ac3"))
                    {
                        AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".ac3");
                    }
                }
            }
            else if (bASSChannelType <= BASSChannelType.BASS_CTYPE_STREAM_AIX)
            {
                if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_OPUS)
                {
                    if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_DSD)
                    {
                        switch (bASSChannelType)
                        {
                            case BASSChannelType.BASS_CTYPE_STREAM_ADX:
                                {
                                    break;
                                }
                            case BASSChannelType.BASS_CTYPE_STREAM_AIX:
                                {
                                    break;
                                }
                            default:
                                {
                                    return;
                                }
                        }
                    }
                }
            }
            else if (bASSChannelType <= BASSChannelType.BASS_CTYPE_MUSIC_IT)
            {
                switch (bASSChannelType)
                {
                    case BASSChannelType.BASS_CTYPE_STREAM_TEMPO:
                        {
                            break;
                        }
                    case BASSChannelType.BASS_CTYPE_STREAM_REVERSE:
                        {
                            break;
                        }
                    default:
                        {
                            switch (bASSChannelType)
                            {
                                case BASSChannelType.BASS_CTYPE_MUSIC_MOD:
                                    {
                                        break;
                                    }
                                case BASSChannelType.BASS_CTYPE_MUSIC_MTM:
                                    {
                                        break;
                                    }
                                case BASSChannelType.BASS_CTYPE_MUSIC_S3M:
                                    {
                                        break;
                                    }
                                case BASSChannelType.BASS_CTYPE_MUSIC_XM:
                                    {
                                        break;
                                    }
                                case BASSChannelType.BASS_CTYPE_MUSIC_IT:
                                    {
                                        break;
                                    }
                                default:
                                    {
                                        return;
                                    }
                            }
                            break;
                        }
                }
            }
            else if (bASSChannelType != BASSChannelType.BASS_CTYPE_STREAM_WAV)
            {
                switch (bASSChannelType)
                {
                    case BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM:
                        {
                            if (!AudioEngine.saveFile.EndsWith(".wav"))
                            {
                                AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".wav");
                            }
                            break;
                        }
                    case BASSChannelType.BASS_CTYPE_RECORD | BASSChannelType.BASS_CTYPE_STREAM | BASSChannelType.BASS_CTYPE_STREAM_OGG | BASSChannelType.BASS_CTYPE_STREAM_WAV:
                        {
                            return;
                        }
                    case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT:
                        {
                            if (!AudioEngine.saveFile.EndsWith(".wav"))
                            {
                                AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".wav");
                            }
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            else if (!AudioEngine.saveFile.EndsWith(".wav"))
            {
                AudioEngine.saveFile = string.Concat(AudioEngine.saveFile, ".wav");
            }
            return;
        }
        #endregion

        public static int[] GetChannelLevels()
        {
            int[] levels = new int[2];
            int level = Bass.BASS_ChannelGetLevel(AudioEngine.GetStreamHandler());
            levels[0] = Utils.LowWord32(level);
            levels[1] = Utils.HighWord32(level);
            return levels;
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
            if (AudioEngine.ChannelStatus != 0)
            {
                Bass.BASS_ChannelStop(stream);
                Bass.BASS_StreamFree(stream);
                Bass.BASS_Stop();
            }

            foreach (var _oDev in _outDevices)
            {
                if (_oDev.Value.IsInitialized)
                {
                    Bass.BASS_SetDevice(_oDev.Key);
                    Bass.BASS_Free();
                }
            }

            Bass.BASS_PluginFree(0);
            Bass.FreeMe();
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
    }

    public class EqualiserEngine
    {
        private float[,] eqMatrix;
        private int _fxEQ;
        private bool _attached;
        private BASS_BFX_PEAKEQ eq;
        //private BASS_BFX_COMPRESSOR2 _cmp;

        public EqualiserEngine(int stream, params float[] frequencies)
        {
            eqMatrix = new float[frequencies.Length, 2];

            AttachEQ(stream);

            //_cmp = new BASS_BFX_COMPRESSOR2();
            //_cmp.fThreshold = 0.3f;
            //_cmp.fAttack = 1.0f;
            //_cmp.fRelease = 10.0f;

            eq = new BASS_BFX_PEAKEQ();
            eq.lChannel = BASSFXChan.BASS_BFX_CHANALL;

            for (int i = 0; i < frequencies.Length; i++)
            {
                eqMatrix[i, 0] = frequencies[i];
                eqMatrix[i, 1] = 0f;

                eq.lBand = i;
                eq.fCenter = frequencies[i];
                Bass.BASS_FXSetParameters(_fxEQ, eq);
                UpdateFX(i, 0f);
            }
        }

        public bool AttachEQ(int stream)
        {
            //Bass.BASS_ChannelSetFX(stream, BASSFXType.BASS_FX_BFX_COMPRESSOR2, 1);
            _fxEQ = Bass.BASS_ChannelSetFX(stream, BASSFXType.BASS_FX_BFX_PEAKEQ, 0);
            _attached = (_fxEQ != 0);
            return _attached;
        }

        public bool DetachEQ(int stream)
        {
            _attached = Bass.BASS_ChannelRemoveFX(stream, _fxEQ);
            return _attached;
        }

        public void Reset(int band /*= -1*/, float value /*= 0f*/)
        {
            if (band == -1)
            {
                for (int i = 0; i < (eqMatrix.Length / 2); i++)
                {
                    UpdateFX(i, value);
                }
            }
            else
            {
                UpdateFX(band, value);
            }
        }

        public void UpdateFX(int band, float gain)
        {
            eqMatrix[band, 1] = gain;
            if (_attached)
            {
                BASS_BFX_PEAKEQ _eq = new BASS_BFX_PEAKEQ();
                // get values of the selected band
                _eq.lBand = band;
                Bass.BASS_FXGetParameters(_fxEQ, _eq);
                _eq.fGain = gain;
                //_eq.fGain = gain + (_cmp.fThreshold * ((1 / _cmp.fRatio) - 1));
                Bass.BASS_FXSetParameters(_fxEQ, _eq);
            }
        }

    }
}
