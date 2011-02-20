using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Fusionbird.FusionToolkit.FusionTrackBar;

namespace DomCtrlLib
{
    public partial class BassSetFm : Form
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private mainFm myParent = null;

        public BassSetFm(Form parentF)
        {
            this.myParent = (mainFm)parentF;
            InitializeComponent();
            timeoutBar.TickStyle = TickStyle.Both;
            opacBar.Value = app.Default.Opacity;
            getCurrWaveOut();
            SetStyle(ControlStyles.Opaque, true);
            this.Opacity = (double)app.Default.Opacity / 100;
            timeoutBar.Value = app.Default.TimeOut;
            
        }

        private void getCurrWaveOut()
        {
            uint CurrVol = 0;
            waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            waveBar.Value = CalcVol / (ushort.MaxValue / 100);
        }

        private void opacBar_Scroll(object sender, EventArgs e)
        {
            app.Default.Opacity = opacBar.Value;
            myParent.Opacity = this.Opacity = (double)opacBar.Value / 100;
            app.Default.Save();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        { 
            e.Cancel = true;
            this.Visible = app.Default.ShowSets = false;
            app.Default.Save();
        }

        private void waveBar_Scroll(object sender, EventArgs e)
        {
            setWaveOut();
        }

        private void setWaveOut()
        {
            int NewVolume = ((ushort.MaxValue / 100) * waveBar.Value);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

        private void changeTimeOut(object sender, EventArgs e)
        {
            FusionTrackBar ft1 = (FusionTrackBar)sender;
            app.Default.TimeOut = ft1.Value;
            app.Default.Save();
        }
    }
}
