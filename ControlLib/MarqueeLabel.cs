using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace libIsh
{
    public class MarqueeLabel : Label
    {
        private int CurrentPosition { get; set; }
        private Timer Timer { get; set; }

        public MarqueeLabel()
        {
            GenerateControl();
            Timer.Interval = 100;
        }

        public MarqueeLabel(int interval)
        {
            GenerateControl();
            Timer.Interval = interval;
        }

        private void GenerateControl()
        {

            UseCompatibleTextRendering = true;
            Timer = new Timer();
            Timer.Tick += new EventHandler(Timer_Tick);
        }

        public bool Switch(bool _switch)
        {
            return (Timer.Enabled = _switch);
        }

        public void Duplicate()
        {
            this.Text += this.Text;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            /*if (CurrentPosition < -Width /2)
                CurrentPosition = Width;
            else
                CurrentPosition -= 2;*/
            this.Text = Text.Substring(1, this.Text.Length - 1) + this.Text.Substring(0, 1);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TranslateTransform((float)CurrentPosition, 0);
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Timer != null)
                    Timer.Dispose();
            }
            Timer = null;
        }

        public void DuplicateIf()
        {
            while (this.Text.Length < this.Size.Width)
                this.Duplicate();
        }
    }
}
