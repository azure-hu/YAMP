﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Un4seen.Bass;

namespace DomCtrlLib
{
    class NowPlaying
    {
        #region InteropServices
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }
        private const int WM_COPYDATA = 0x4A;
        private static COPYDATASTRUCT data;
        #endregion

        private void UpdateOnPlayerStateChange(int handle)
        {
            BASSActive bsa = Bass.BASS_ChannelIsActive(handle);
            switch (bsa)
                {
                    case BASSActive.BASS_ACTIVE_STOPPED:
                        SetMSNMusic(false, "", "", "");
                        break;
                    case BASSActive.BASS_ACTIVE_PLAYING:
                        SetMSNMusic(true, file.Name, file.Artist, file.Album);
                        break;
                    case BASSActive.BASS_ACTIVE_PAUSED:
                        SetMSNMusic(true, file.Name, file.Artist, file.Album);
                        break;
                    default:
                        SetMSNMusic(false, "", "", "");
                        break;
                }
        }

        private static IntPtr VarPtr(object e)
        {
            GCHandle GC = GCHandle.Alloc(e, GCHandleType.Pinned);
            IntPtr gc = GC.AddrOfPinnedObject();
            GC.Free();
            return gc;
        }
        private void SetMSNMusic(bool enable, string title, string artist, string album)
        {
            string category = "Music";
            string buffer = "\\0" + category + "\\0" + (enable ? "1" : "0") + "\\0{0}-{1}\\0" + title + "\\0" + artist + "\\0" + album + "\\0\\0\0";
            int handle = 0;
            IntPtr handlePtr = new IntPtr(handle);

            data.dwData = (IntPtr)0x0547;
            data.lpData = VarPtr(buffer);
            data.cbData = buffer.Length * 2;

            // Call method to update IM's - PlayingNow
            handlePtr = FindWindowEx(IntPtr.Zero, handlePtr, "MsnMsgrUIManager", null);
            if (handlePtr.ToInt32() > 0)
                SendMessage(handlePtr, WM_COPYDATA, IntPtr.Zero, VarPtr(data));
        }
    }
}
