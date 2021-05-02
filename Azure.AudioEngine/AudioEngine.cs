namespace Azure.MediaUtils
{
    using Azure.LibCollection.CS;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Un4seen.Bass;
    using Un4seen.BassWasapi;
    using BassUtils = Un4seen.Bass.Utils;

    public sealed class AudioEngine : IDisposable
    {
        #region enumerations

        public enum PlaybackState
        {
            Stopped = (UInt16)BASSActive.BASS_ACTIVE_STOPPED,
            Playing = (UInt16)BASSActive.BASS_ACTIVE_PLAYING,
            Stalled = (UInt16)BASSActive.BASS_ACTIVE_STALLED,
            Paused = (UInt16)BASSActive.BASS_ACTIVE_PAUSED
        }

        public enum StreamMode
        {
            File,
            Online
        }

        internal enum Format
        {
            Default, PluginSupported, UnSupported
        }

        #endregion enumerations

        #region variables

        private Int32 streamHandle;
        private Int32 currDevID;
        private IntPtr appHandle;
        private Dictionary<Int32, String> plugins;
        private PlaybackState state;
        private Visualizer vis;

        private FileStream fileStream;
        private StreamMode streamMode;
        private float channelVolume;
        private string currentFile;
        private bool recOnlineStream;
        private string urlOnlineStream;

        #region letoltesi proba

        private IntPtr dlPointer;
        private DOWNLOADPROC tryDownload;
        private byte[] _data; // local data buffer
        private string saveFile = "output";
        private IntPtr _handle;

        #endregion


        public readonly BASSWASAPIVolume wasapiCurve = BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION;

        // Flag: Has Dispose already been called?
        private Boolean disposed = false;
        private float wasapiVolume;

        #endregion variables

        #region constants

        private const Int32 frequency = 44100;
        private const Int32 bufferLength = 5000;

        #endregion constants

        #region properties

        /// <summary>
        /// Handle of a "current" used stream, like a file descriptor.
        /// </summary>
        public Int32 stream { get { return this.streamHandle; } private set { this.streamHandle = value; } }

        /// <summary>
        /// Current output device ID. (-1, for system default device)
        /// </summary>
        public Int32 CurrentDevice { get { return currDevID; } set { this.SwitchOutputDevice(value); } }

        /// <summary>
        /// Handle of the main window, where the engine is used. Use IntPtr.Zero, if no windows used.
        /// </summary>
        public IntPtr Hwnd { get { return this.appHandle; } private set { this.appHandle = value; } }

        /// <summary>
        /// Store of the loaded plugins.
        /// </summary>
        public Dictionary<Int32, String> Plugins { get { return this.plugins; } private set { this.plugins = value; } }

        /// <summary>
        /// Flag of current playback state
        /// </summary>
        public PlaybackState CurrentState { get { return (PlaybackState)((Int32)Bass.BASS_ChannelIsActive(stream)); } private set { this.state = value; } }

        public String SupportedFileExtensions
        {
            get { return BassUtils.BASSAddOnGetSupportedFileExtensions(this.Plugins, true); }
        }

        public String SupportedFileFilter
        {
            get { return BassUtils.BASSAddOnGetSupportedFileFilter(this.Plugins, "All supported Audio Files", true); }
        }

        public bool WasapiAvailable
        {
            get { return this.plugins.ContainsValue("basswasapi.dll"); }
        }

        #endregion

        #region constructor / destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="deviceID">ID of the audio device, where the playback will be done. Use GetAvailablePlaybackDevices() to get all available device ID's.</param>
        /// <param name="modulePath">Directory of "bass.dll" library.</param>
        /// <param name="handle">Handle of the main window, where the engine is used. Use IntPtr.Zero, if no windows used.</param>
        /// <param name="usePlugins">Set "true", if BASS plugins will be used.</param>
        /// <param name="pluginPath">Directory of BASS plugins. Use "null", if the plugins, and the main module are in the same directory.</param>
        public AudioEngine(Int32 deviceID, String modulePath, IntPtr handle, bool usePlugins = false, String pluginPath = null)
        {
            Utils.Prepare();
            this.Hwnd = handle;
            bool libLoaded = Utils.LoadBass(modulePath);

            if (!libLoaded)
            {
                throw new Exception(string.Concat("Audio Library Not Found!", Environment.NewLine, Bass.BASS_ErrorGetCode().ToString()));
            }

            if (!Bass.BASS_Init(deviceID, AudioEngine.frequency, BASSInit.BASS_DEVICE_LATENCY | BASSInit.BASS_DEVICE_DEFAULT, this.Hwnd))
            {
                throw new Exception(Bass.BASS_ErrorGetCode().ToString());
            }
            this.CurrentDevice = deviceID;
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, AudioEngine.bufferLength);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, AudioEngine.bufferLength);

            this.Plugins = Bass.BASS_PluginLoadDirectory(usePlugins ? (String.IsNullOrEmpty(pluginPath) ? modulePath : pluginPath) : null);

        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Private implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.CurrentState != PlaybackState.Stopped)
                {
                    Bass.BASS_ChannelStop(this.stream);
                    Bass.BASS_StreamFree(this.stream);
                    Bass.BASS_Stop();
                }
                var devInfos = Un4seen.Bass.Bass.BASS_GetDeviceInfos();
                for (int i = 0; i < devInfos.Length; ++i)
                {
                    if (devInfos[i].IsInitialized)
                    {
                        Bass.BASS_SetDevice(i);
                        Bass.BASS_Free();
                    }
                }
                devInfos = null;
                Bass.BASS_SetDevice(-1);
                Bass.BASS_Free();
                Bass.BASS_PluginFree(0);
                Bass.FreeMe();
                this.stream = 0;

                vis.StopDraw();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion


        private Dictionary<int, BASS_DEVICEINFO> QueryOutputDevices(out int _defDevice)
        {
            _defDevice = -1;
            Dictionary<int, BASS_DEVICEINFO> _outDevices = new Dictionary<int, BASS_DEVICEINFO>();
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
            return _outDevices;
        }

        public Dictionary<int, string> GetAvailableOutputs()
        {
            int _defDevice = -1;
            Dictionary<int, string> _outputs = new Dictionary<int, string>();
            foreach (var _out in this.QueryOutputDevices(out _defDevice))
            {
                _outputs.Add(_out.Key, _out.Value.name);
            }
            if (this.CurrentDevice == -1 && _defDevice != -1)
            {
                this.CurrentDevice = _defDevice;
            }
            return _outputs;
        }

        public KeyValuePair<int, string> GetActiveOutput()
        {
            KeyValuePair<int, string> _active = new KeyValuePair<int, string>(-1, "<system default>");
            int _defDevice = -1;
            foreach (var _out in this.QueryOutputDevices(out _defDevice))
            {
                if (_out.Key == this.CurrentDevice)
                {
                    _active = new KeyValuePair<int, string>(_out.Key, _out.Value.name);
                    break;
                }
            }
            return _active;
        }

        public KeyValuePair<int, string> GetDefaultOutput()
        {
            KeyValuePair<int, string> _default = new KeyValuePair<int, string>(-1, "<system default>");
            int _defDevice = -1;
            foreach (var _out in this.QueryOutputDevices(out _defDevice))
            {
                if (_out.Value.IsDefault)
                {
                    _default = new KeyValuePair<int, string>(_out.Key, _out.Value.name);
                    break;
                }
            }
            return _default;
        }

        /// <summary>
        /// Switching playback device.
        /// </summary>
        /// <param name="device">Index of the new device. Use -1 for system default. For all available devices, use Utils.QueryOutputDevices(out int) method.</param>
        /// <param name="useFallback">If "true" and switching failed, then sets to system default.</param>
        /// <returns>Returns "true", when change was successful. Returns "false" even if fallback to system device is enabled.</returns>
        public Boolean SwitchOutputDevice(Int32 device, Boolean useFallback = true)
        {
            BASS_DEVICEINFO deviceInfo = Un4seen.Bass.Bass.BASS_GetDeviceInfo(device);
            if (deviceInfo != null && !deviceInfo.IsInitialized)
            {
                if (!Bass.BASS_Init(device, AudioEngine.frequency, BASSInit.BASS_DEVICE_LATENCY | BASSInit.BASS_DEVICE_CPSPEAKERS, Hwnd))
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
            }

            Boolean isOk = Bass.BASS_SetDevice(device);
            if (isOk && this.CurrentState != PlaybackState.Stopped)
            {
                isOk = Bass.BASS_ChannelSetDevice(this.stream, device);
            }
            if (isOk)
            {
                this.currDevID = device;
            }
            else
            {
                Bass.BASS_ChannelSetDevice(this.stream, -1);
                this.currDevID = -1;
            }
            return isOk;
        }

        private Format GetFormat(string p)
        {
            if (BassUtils.BASSAddOnIsFileSupported(plugins, p))
            {
                string _ext = Path.GetExtension(p);
                if (Bass.SupportedStreamExtensions.Contains(_ext))
                {
                    return Format.Default;
                }
                else
                {
                    return Format.PluginSupported;
                }
            }
            else
            {
                return Format.UnSupported;
            }
        }

        #region playback functions
        public void PlayInitFile(string filePath)
        {
            this.CurrentFile = filePath;
            CreateFileStream(this.CurrentFile);

            if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
            {
                MessageBox.Show(Bass.BASS_ErrorGetCode().ToString(), "Engine Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Play();
        }

        private void CreateFileStream(string filePath)
        {
            BASS_INFO info = Bass.BASS_GetInfo();
            BASSFlag _flag = (info.speakers < 3 ? BASSFlag.BASS_AAC_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 : BASSFlag.BASS_DEFAULT)
                | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE;

            Format _fmt = this.GetFormat(filePath);
            switch (_fmt)
            {
                case Format.UnSupported:
                    MBoxHelper.ShowErrorMsg("Unsupported File Format!", "Format Error!");
                    break;
                case Format.Default:
                case Format.PluginSupported:
                    {
                        this.stream = Bass.BASS_StreamCreateFile(filePath, 0, 0, _flag);
                    }
                    break;

            }
        }

        public bool Play()
        {
            bool flag;
            try
            {
                if (this.CurrentState != PlaybackState.Paused)
                {
                    Bass.BASS_ChannelUpdate(this.stream, 0);
                    int _tries = 0;
                    do
                    {
                        if (Bass.BASS_ChannelPlay(this.stream, true))
                        {
                            if (!this.WasapiAvailable)
                            {
                                this.ChannelVolume = this.channelVolume;
                            }

                            break;
                        }
                        else
                        {
                            switch (this.streamMode)
                            {
                                case StreamMode.File:
                                    {
                                        this.CreateFileStream(this.CurrentFile);
                                        break;
                                    }
                                case StreamMode.Online:
                                    {
                                        this.InitOnlineStream();
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
                    Bass.BASS_ChannelPlay(this.stream, false);
                }
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception(string.Concat("BASS Error on Play:", Bass.BASS_ErrorGetCode()));
                }
                if (vis.IsReady)
                {
                    vis.StartDraw(this.stream);
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

        public void Play(string filePath)
        {
            if (!Play())
            {
                PlayInitFile(filePath);
            }
        }

        public bool Stop()
        {
            try
            {
                if (Bass.BASS_ChannelIsActive(stream) != BASSActive.BASS_ACTIVE_STOPPED)
                {
                    Bass.BASS_ChannelStop(stream);
                }

                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Stop");
                }

                if (vis.IsReady)
                {
                    vis.StopDraw();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Pause()
        {
            try
            {
                if (this.CurrentState == PlaybackState.Paused)
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

        public void Rewind()
        {
            Bass.BASS_ChannelSetPosition(this.stream, 0.0);
        }

        public bool ListenInternetStream(Form fm, string formTitle, string StrUrl, string StrName,
            int TimeOut, bool Record, string fileToRec)
        {
            if (fm != null)
            {
                fm.Text = formTitle + " # Connecting...";
            }

            int i = 0;
            PlaybackState baInd;
            do
            {
                this.saveFile = fileToRec;
                this.PlayInitUrl(StrUrl, Record);
                //Thread.Sleep(1000);
                Application.DoEvents();
                ++i;
                baInd = this.CurrentState;
            }
            while ((baInd == PlaybackState.Stopped) && (i < TimeOut));
            if (baInd != PlaybackState.Stopped)
            {
                if (fm != null)
                {
                    fm.Text = formTitle + (Record ? " # Recording: " : " # Playing: ") + StrName;
                }

                if (vis.IsReady)
                {
                    vis.StartDraw(stream);
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        public void SetOutput(string p)
        {
            this.saveFile = p;
        }

        private void MyDownload(IntPtr buffer, int length, IntPtr user)
        {
            if (this.fileStream == null)
            {
                this.fileStream = File.OpenWrite(this.saveFile);
            }
            if (!(buffer == IntPtr.Zero))
            {
                if ((this._data == null ? true : (int)this._data.Length < length))
                {
                    this._data = new byte[length];
                }
                Marshal.Copy(buffer, this._data, 0, length);
                this.fileStream.Write(this._data, 0, length);
            }
            else
            {
                this.fileStream.Flush();
                this.fileStream.Close();
            }
        }

        public bool ShutDownNet()
        {
            try
            {
                Stop();
                if (this.fileStream != null)
                {
                    this.fileStream.Flush();
                    this.fileStream.Close();

                    dlPointer = IntPtr.Zero;
                    tryDownload = null;
                    _data = null;
                    saveFile = "output";
                }

                return true;
            }
            catch
            {
                this.fileStream = null;
                return false;
            }
        }

        #endregion

        #region engine settings

        public float masterVolume
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
                    {
                        Bass.BASS_SetVolume(0F);
                    }
                    else
                    {
                        Bass.BASS_SetVolume(1F);
                    }
                }
            }
        }

        public float ChannelVolume
        {
            get
            {
                return this.channelVolume;
            }
            set
            {
                if (value != this.ChannelVolume)
                {
                    if (value < 0f)
                    {
                        this.channelVolume = 0f;
                    }
                    else
                    {
                        if (value > 1f)
                        {
                            this.channelVolume = 1f;
                        }
                        else
                        {
                            this.channelVolume = value;
                        }
                    }
                }

                Bass.BASS_ChannelSetAttribute(this.stream, BASSAttribute.BASS_ATTRIB_VOL, this.ChannelVolume);
            }
        }

        public float WasapiVolume
        {
            get
            {
                return BassWasapi.BASS_WASAPI_GetVolume(wasapiCurve);
            }
            set
            {
                if (!BassWasapi.BASS_WASAPI_SetVolume(wasapiCurve, value))
                {
                    if (value < 0)
                    {
                        BassWasapi.BASS_WASAPI_SetVolume(wasapiCurve, 0F);
                    }
                    else
                    {
                        BassWasapi.BASS_WASAPI_SetVolume(wasapiCurve, 1F);
                    }
                }
            }
        }

        public float PlaybackVolume
        {
            get {
                if (this.WasapiAvailable)
                {
                    return this.WasapiVolume;
                }
                else
                {
                    return this.ChannelVolume;
                }
            }
            set {
                if (this.WasapiAvailable)
                {
                    this.WasapiVolume = value;
                }
                else
                {
                    this.ChannelVolume = value;
                }
            }
        }



        #endregion

        #region utility functions, methods, properties

        public long LengthInBytes
        {
            get { return Bass.BASS_ChannelGetLength(this.stream); /* length in bytes */}
        }

        public long PositionInBytes
        {
            get { return Bass.BASS_ChannelGetPosition(this.stream); /* position in bytes */}
            set
            {
                Bass.BASS_ChannelSetPosition(stream, value, BASSMode.BASS_POS_BYTES);
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("Engine - File Positioning Error!");
                }
            }
        }

        public double TotalTime
        {
            get { return Bass.BASS_ChannelBytes2Seconds(stream, this.LengthInBytes); /* the total time length */}
        }

        public double ElapsedTime
        {
            get { return Bass.BASS_ChannelBytes2Seconds(stream, this.PositionInBytes); /* the elapsed time length */}
        }

        public double RemainingTime
        {
            get
            {
                double remainingtime = this.TotalTime - this.ElapsedTime;
                return remainingtime;
            }
        }

        public TimeSpan CurrentPosition
        {
            get
            {
                return TimeSpan.FromSeconds(this.ElapsedTime);
            }
        }

        public string ElapsedTimeString { get { return BassUtils.FixTimespan(this.ElapsedTime, "MMSS"); } }

        public string RemainingTimeString { get { return BassUtils.FixTimespan(this.RemainingTime, "MMSS"); } }

        public string CurrentFile { get { return this.currentFile; } private set { this.currentFile = value; } }

        public Visualizer Visuals { get { return this.vis; } set { vis = value; } }

        public string GetChannelInfo(string param)
        {
            switch (param)
            {
                case "chan": return this.ChannelInfo.chans.ToString();
                case "freq": return this.ChannelInfo.freq.ToString();
                case "chanStr": return BassUtils.ChannelNumberToString(this.ChannelInfo.chans);
                case "elapsedTime": return BassUtils.FixTimespan(this.ElapsedTime, "MMSS");
                case "remainingTime": return BassUtils.FixTimespan(this.RemainingTime, "MMSS");
                case "totalTime": return BassUtils.FixTimespan(this.TotalTime, "MMSS");
                default:
                    return null;
            }
        }

        internal BASS_CHANNELINFO ChannelInfo { get { return Bass.BASS_ChannelGetInfo(this.stream); } }

        public int[] GetChannelLevels()
        {
            int[] levels = new int[2];
            int level = Bass.BASS_ChannelGetLevel(this.stream);
            levels[0] = BassUtils.LowWord32(level);
            levels[1] = BassUtils.HighWord32(level);
            return levels;
        }

        #endregion

        #region online stuffs

        public string[] GetOnlineInfo()
        {
            string item;
            string[] strArrays;
            int i;
            string[] tags1 = Bass.BASS_ChannelGetTagsICY(this.stream);
            string[] tags2 = Bass.BASS_ChannelGetTagsMETA(this.stream);
            string[] tags3 = Bass.BASS_ChannelGetTagsHTTP(this.stream);
            List<string> tags = new List<string>();
            if (tags1 != null)
            {
                strArrays = strArrays = SplitTags(tags1, ';');
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    item = strArrays[i];
                    if (item.Contains("StreamTitle='"))
                    {
                        tags.Add(item.Substring(0, item.Length - 1).Replace("StreamTitle='", string.Empty));
                    }
                }
            }
            if (tags2 != null)
            {
                strArrays = SplitTags(tags2, ';');
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    item = strArrays[i];
                    if (item.Contains("StreamTitle='"))
                    {
                        tags.Add(item.Substring(0, item.Length - 1).Replace("StreamTitle='", string.Empty));
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

        private static String[] SplitTags(String[] tags, char splitChar)
        {
            List<string> _tmp = new List<string>();
            foreach (var tag in tags)
            {
                _tmp.AddRange(tag.Split(splitChar));
            }
            return _tmp.ToArray();
        }

        public void PlayInitUrl(string StrUrl, bool Record)
        {
            this.InitOnlineStream(StrUrl, Record);
            this.Play();
        }

        public void InitOnlineStream(string StrUrl, bool Record)
        {
            this.urlOnlineStream = StrUrl;
            this.recOnlineStream = Record;
            this.streamMode = StreamMode.Online;
            this.InitOnlineStream();
        }

        private void InitOnlineStream()
        {
            DOWNLOADPROC dOWNLOADPROC;
            BASSFlag flag = BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_MIXER_NORAMPIN | BASSFlag.BASS_MIDI_SINCINTER | BASSFlag.BASS_MUSIC_AUTOFREE | BASSFlag.BASS_MUSIC_SINCINTER;
            if (this.recOnlineStream)
            {
                dOWNLOADPROC = new DOWNLOADPROC(this.MyDownload);
            }
            else
            {
                dOWNLOADPROC = null;
            }
            this.tryDownload = dOWNLOADPROC;
            IntPtr user = (this.recOnlineStream ? this.dlPointer : IntPtr.Zero);
            this.stream = Bass.BASS_StreamCreateURL(this.urlOnlineStream, 0, flag, this.tryDownload, user);
            BASSChannelType bASSChannelType = Bass.BASS_ChannelGetInfo(this.stream).ctype;
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
                                                    if (!this.saveFile.EndsWith(".ogg"))
                                                    {
                                                        this.saveFile = string.Concat(this.saveFile, ".ogg");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP1:
                                                {
                                                    if (!this.saveFile.EndsWith(".mp1"))
                                                    {
                                                        this.saveFile = string.Concat(this.saveFile, ".mp1");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP2:
                                                {
                                                    if (!this.saveFile.EndsWith(".mp2"))
                                                    {
                                                        this.saveFile = string.Concat(this.saveFile, ".mp2");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_MP3:
                                                {
                                                    if (!this.saveFile.EndsWith(".mp3"))
                                                    {
                                                        this.saveFile = string.Concat(this.saveFile, ".mp3");
                                                    }
                                                    break;
                                                }
                                            case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
                                                {
                                                    if (!this.saveFile.EndsWith(".wav"))
                                                    {
                                                        this.saveFile = string.Concat(this.saveFile, ".wav");
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
                                    if (!this.saveFile.EndsWith(".wma"))
                                    {
                                        this.saveFile = string.Concat(this.saveFile, ".wma");
                                    }
                                    break;
                                }
                            case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3:
                                {
                                    if (!this.saveFile.EndsWith(".mp3"))
                                    {
                                        this.saveFile = string.Concat(this.saveFile, ".mp3");
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
                                    if (!this.saveFile.EndsWith(".ape"))
                                    {
                                        this.saveFile = string.Concat(this.saveFile, ".ape");
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
                                            if (!this.saveFile.EndsWith(".flac"))
                                            {
                                                this.saveFile = string.Concat(this.saveFile, ".flac");
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
                                if (!this.saveFile.EndsWith(".aac"))
                                {
                                    this.saveFile = string.Concat(this.saveFile, ".aac");
                                }
                                break;
                            }
                        case BASSChannelType.BASS_CTYPE_STREAM_MP4:
                            {
                                if (!this.saveFile.EndsWith(".mp4"))
                                {
                                    this.saveFile = string.Concat(this.saveFile, ".mp4");
                                }
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
                else if (!this.saveFile.EndsWith(".mpc"))
                {
                    this.saveFile = string.Concat(this.saveFile, ".mpc");
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
                            if (!this.saveFile.EndsWith(".alac"))
                            {
                                this.saveFile = string.Concat(this.saveFile, ".alac");
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
                    else if (!this.saveFile.EndsWith(".ac3"))
                    {
                        this.saveFile = string.Concat(this.saveFile, ".ac3");
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
                            if (!this.saveFile.EndsWith(".wav"))
                            {
                                this.saveFile = string.Concat(this.saveFile, ".wav");
                            }
                            break;
                        }
                    case BASSChannelType.BASS_CTYPE_RECORD | BASSChannelType.BASS_CTYPE_STREAM | BASSChannelType.BASS_CTYPE_STREAM_OGG | BASSChannelType.BASS_CTYPE_STREAM_WAV:
                        {
                            return;
                        }
                    case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT:
                        {
                            if (!this.saveFile.EndsWith(".wav"))
                            {
                                this.saveFile = string.Concat(this.saveFile, ".wav");
                            }
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            else if (!this.saveFile.EndsWith(".wav"))
            {
                this.saveFile = string.Concat(this.saveFile, ".wav");
            }
            return;
        }
        #endregion
    }
}
