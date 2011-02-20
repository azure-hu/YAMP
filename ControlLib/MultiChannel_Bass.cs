using System;
using System.Collections.Generic;
using System.Text;

#region Bass.Net

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Aac;
using Un4seen.Bass.AddOn.Ac3;
using Un4seen.Bass.AddOn.Ape;
using Un4seen.Bass.AddOn.Flac;
using Un4seen.Bass.AddOn.Mpc;
using Un4seen.Bass.AddOn.Wma;
using Un4seen.Bass.AddOn.Tags;
using Un4seen.Bass.Misc;
using System.Windows.Forms;

#endregion

namespace libDomingo
{
    public static class Bass_Multi
    {
        private static int[] handleBlock;
        private static Visuals[] vis;
        private static int handleCount;

        /// <summary>
        /// Initializes the engine; all libraries should be located in application's folder.
        /// </summary>
        /// <param name="handle_Count">Number of streams you want to handle</param>
        public static void Init(int handle_Count)
        {
            Init(handle_Count, ".");
        }

        /// <summary>
        /// Initializes the engine; all libraries should be located in a typed folder.
        /// </summary>
        /// <param name="handle_Count">Number of streams you want to handle</param>
        /// <param name="LibPath">Library folder for DLL's</param>
        public static void Init(int handle_Count, string LibPath)
        {
            try
            {
                handleCount = handle_Count;
                BassNet.Registration("flashmark@ymail.com", "2X1652019342222");
                if (Bass.LoadMe(LibPath))
                {
                    if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT | BASSInit.BASS_DEVICE_LATENCY
                        | BASSInit.BASS_DEVICE_3D | BASSInit.BASS_DEVICE_CPSPEAKERS, IntPtr.Zero))
                    {
                        throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                    }
                    handleBlock = new int[handleCount];
                    vis = new Visuals[handleCount];
                    for (int i = 0; i < handleCount; ++i)
                    {
                        if (vis[i] == null)
                        {
                            vis[i] = new Visuals();
                        }
                    }
                }
                else
                {
                    throw new Exception("Audio Library Not Found!");
                }


                BassAac.LoadMe(LibPath);
                BassWma.LoadMe(LibPath);
                BassAc3.LoadMe(LibPath);
                BassApe.LoadMe(LibPath);
                BassMpc.LoadMe(LibPath);
                BassFlac.LoadMe(LibPath);
                
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 2000);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_FLOATDSP, true);
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 2);
            }
            catch (Exception Ex)
            {
                MBoxHelper.ShowErrorMsg(Bass.BASS_ErrorGetCode() + ": " + Ex.Message, "BASS Init Error!");
            }
        }


        public static int streamHandler
        {
            get { return stream; }
        }

        public static float masterVolume
        {
            get
            {
                return (Bass.BASS_GetVolume() * 100);
            }
            set
            {
                Bass.BASS_SetVolume(value/100);
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
            catch (Exception ex)
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
                    Bass.BASS_ChannelPlay(stream, true);
                }

                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Play");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Stop()
        {
            try
            {
                Bass.BASS_ChannelStop(stream);
                Bass.BASS_Stop();
                if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
                {
                    throw new Exception("BASS Error on Stop");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ShowStatus(Form fm, string formTitle, string StrUrl, string StrName, int TimeOut)
        {
            fm.Text = formTitle + " # Connecting...";
            int i = 0;
            do
            {
                PlayInitUrl(StrUrl);
                Thread.Sleep(100);
                Application.DoEvents();
                ++i;
            }
            while ((InternalStatus() != BASSActive.BASS_ACTIVE_PLAYING) && (i < TimeOut * 10));
            if (InternalStatus() == BASSActive.BASS_ACTIVE_PLAYING)
            {
                fm.Text = formTitle + " # Playing: " + StrName;
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

        public static void PlayInitUrl(string StrUrl)
        {
            stream = Bass.BASS_StreamCreateURL(StrUrl, 0, BASSFlag.BASS_SAMPLE_FLOAT 
                | BASSFlag.BASS_STREAM_AUTOFREE, null, IntPtr.Zero);
            if (stream == 0)
            {
                stream = BassAac.BASS_AAC_StreamCreateURL(StrUrl, 0, BASSFlag.BASS_SAMPLE_FLOAT
                    | BASSFlag.BASS_STREAM_AUTOFREE, null, IntPtr.Zero);
            }
            OpenPlay();
        }

        public static void PlayInitFile(string filePath)
        {
            string fileEnding = filePath.Substring(filePath.Length - 3);

            switch (fileEnding)
            {
                case "aac": stream = BassAac.BASS_AAC_StreamCreateFile(filePath, 0, 0, 
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "m4a":
                case "m4b":
                case "mp4": stream = BassAac.BASS_MP4_StreamCreateFile(filePath, 0, 0,
                      BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "ac3": stream = BassAc3.BASS_AC3_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "ape": stream = BassApe.BASS_APE_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "flac": stream = BassFlac.BASS_FLAC_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "mpp":
                case "mp+":
                case "mpc": stream = BassMpc.BASS_MPC_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
                case "wma": stream = BassWma.BASS_WMA_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;                  
                default: stream = Bass.BASS_StreamCreateFile(filePath, 0, 0,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_AUTOFREE); break;
            }
            OpenPlay();
        }

        private static void OpenPlay()
        {
            if (Bass.BASS_ErrorGetCode() != BASSError.BASS_OK)
            {
                MessageBox.Show(Bass.BASS_ErrorGetCode().ToString(), "Engine Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Thread.Sleep(20);
                Bass.BASS_Start();

                Bass.BASS_ChannelUpdate(stream, 0);
                Bass.BASS_ChannelPlay(stream, true);
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
            catch(Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Clean(int hIndex)
        {
            return Bass.BASS_StreamFree(handleBlock[hIndex]);
        }

        /// <summary>
        /// Use only if you want to shut down BassEngine.
        /// </summary>
        public static void ShutDown()
        {         
            int i = 0;
            bool ok = true;
            while (i < handleCount && ok)
            {
                ok = Clean(handleBlock[i]);
                if (ok)
                {
                    handleBlock[i] = 0;
                    ++i;
                }
                else 
                {
                    //throw new Exception("ShutDown failed!");
                }

            }

            BassFlac.FreeMe();
            BassMpc.FreeMe();
            BassWma.FreeMe();
            BassApe.FreeMe();
            BassAc3.FreeMe();
            BassAac.FreeMe();
            Bass.BASS_Free();

            vis = null;
        }

        public static void Rewind(int hIndex)
        {
            Bass.BASS_ChannelSetPosition(handleBlock[hIndex], 0.0);
        }

        
        public static void Wipe(int hIndex)
        {
            Bass.BASS_StreamFree(handleBlock[hIndex]);
            GC.Collect();
        }
    }
}
