using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace libZoi
{
    public static class FireWorks
    {
        public enum VisualType
        {
            SpectrumLineN, SpectrumLineP, SpectrumFull, SpectrumBean,
            SpectrumDot, SpectrumEllipse, SpectrumWave, WaveForm
        }

        static Visuals vs;
        static Color _base = Color.GreenYellow, _peak = Color.IndianRed, _hold = Color.Gray, _back = Color.Transparent;
        static int _width = 2, _dist = 1, _pdelay = 5, _channel;
        static bool _isLinear = true, _drawFull = true, _antiAlias = true, _isReady = false;
        static BASSTimer _timer;
        static Graphics _graphicsG;
        static System.Windows.Forms.PictureBox _graphicsB;
        static Rectangle _rect;
        static VisualType _vstype = VisualType.SpectrumLineP;

        private static void BaseInit()
        {
            vs = new Visuals();
            _timer = new BASSTimer();
            _timer.Enabled = false;
            _isReady = true;
        }

        #region Control Methods

        public static void Init(Graphics g, Rectangle rectangle)
        {
            //_graphics = g.GetHdc();
            _graphicsG = g;
            _rect = rectangle;
            BaseInit();
            _timer.Tick += new EventHandler(_timer_Graphics_Tick);
        }

        public static void Init(Graphics g, Rectangle rectangle, int timerInterval)
        {
            Init(g, rectangle);
            _timer.Interval = timerInterval;
        }

        public static void Init(ref System.Windows.Forms.PictureBox g)
        {
            _graphicsB = g;
            _rect = g.DisplayRectangle;
            BaseInit();
            _timer.Tick += new EventHandler(_timer_Bmp_Tick);
        }

        public static void Init(ref System.Windows.Forms.PictureBox g, int timerInterval)
        {
            Init(ref g);
            _timer.Interval = timerInterval;
        }

        public static void Set(string param, object value)
        {
            switch (param)
            {
                case "isLinear": _isLinear = (bool)value; break;
                case "drawFull": _drawFull = (bool)value; break;
                case "antiAlias": _antiAlias = (bool)value; break;
                case "pixelWidth": _width = (int)value; break;
                case "pixelDistance": _dist = (int)value; break;
                case "peakDelay": _pdelay = (int)value; break;
                case "timerInterval": _timer.Interval = (int)value; break;
                case "baseColor": _base = (Color)value; break;
                case "peakColor": _peak = (Color)value; break;
                case "holdColor": _hold = (Color)value; break;
                case "backColor": _back = (Color)value; break;
                default: throw new Exception("Undefined Parameter!");
            }
        }


        public static Color GetColor(string param)
        {
            switch (param)
            {
                case "base": return _base;
                case "peak": return _peak;
                case "hold": return _hold;
                case "back": return _back;
                default: throw new Exception("Undefined Parameter!");
            }
        }

        public static void SetVisual(VisualType value)
        {
            _vstype = (value > VisualType.WaveForm ? VisualType.SpectrumLineN : value);
        }

        public static VisualType GetVSType { get { return _vstype; } }

        public static bool IsReady { get { return _isReady; } }

        public static void StartDraw(int channel)
        {
            _channel = channel;
            _timer.Enabled = true;
        }

        public static void StopDraw()
        {
            _timer.Enabled = false;
        }

        #endregion Control Methods

        #region Draw With Graphics

        static void _timer_Graphics_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (_vstype)
                {
                    case VisualType.SpectrumLineP:
                        DrawSpectrumLine(ref _channel, _graphicsG, ref _rect, true);
                        break;
                    case VisualType.SpectrumLineN:
                        break;
                    case VisualType.SpectrumFull:
                        break;
                    case VisualType.SpectrumBean:
                        break;
                    case VisualType.SpectrumDot:
                        break;
                    case VisualType.SpectrumEllipse:
                        break;
                    case VisualType.SpectrumWave:
                        break;
                    case VisualType.WaveForm:
                        break;
                }
            }

            catch (Exception x)
            {
                MBoxHelper.ShowInfoMsg(x.Message, "");
            }
        }

        public static bool DrawSpectrumLine(ref int channel, Graphics g, ref Rectangle r, bool drawPeak)
        {
            if (drawPeak)
            {
                return vs.CreateSpectrumLinePeak(channel, g, r, _base, _peak, _hold, _back, _width, _dist,
                    _pdelay, _isLinear, _drawFull, _antiAlias);
            }
            else
            {
                return vs.CreateSpectrumLine(channel, g, r, _base, _peak, _back, _width, _dist, _isLinear,
                    _drawFull, _antiAlias);
            }

        }

        public static bool DrawSpectrumFull(ref int channel, Graphics g, ref Rectangle r)
        {
            return vs.CreateSpectrum(channel, g, r, _base, _peak, _back, _isLinear, _drawFull, _antiAlias);
        }

        public static bool DrawSpectrumBean(ref int channel, Graphics g, ref Rectangle r)
        {
            return vs.CreateSpectrumBean(channel, g, r, _base, _peak, _back, _width,
                _isLinear, _drawFull, _antiAlias);
        }

        public static bool DrawSpectrumDot(ref int channel, Graphics g, ref Rectangle r)
        {
            return vs.CreateSpectrumDot(channel, g, r, _base, _peak, _back, _width, _dist,
                _isLinear, _drawFull, _antiAlias);
        }

        public static bool DrawSpectrumEllipse(ref int channel, Graphics g, ref Rectangle r)
        {
            return vs.CreateSpectrumEllipse(channel, g, r, _base, _peak, _back, _width, _dist,
                _isLinear, _drawFull, _antiAlias);
        }

        public static bool DrawSpectrumWave(ref int channel, Graphics g, ref Rectangle r)
        {
            return vs.CreateSpectrumWave(channel, g, r, _base, _peak, _back, _width,
                _isLinear, _drawFull, _antiAlias);
        }

        public static bool DrawWaveForm(ref int channel, Graphics g, ref Rectangle r, bool mono)
        {
            return vs.CreateWaveForm(channel, g, r, _base, _peak, _hold, _back, _width,
                _drawFull, mono, _antiAlias);
        }

        #endregion Draw With Graphics

        #region Draw Bitmap



        static void _timer_Bmp_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (_vstype)
                {
                    case VisualType.SpectrumLineP:
                        _graphicsB.Image = DrawSpectrumLine(ref _channel, ref _rect, true);
                        break;
                    case VisualType.SpectrumLineN:
                        _graphicsB.Image = DrawSpectrumLine(ref _channel, ref _rect, false);
                        break;
                    case VisualType.SpectrumFull:
                        _graphicsB.Image = DrawSpectrumFull(ref _channel, ref _rect);
                        break;
                    case VisualType.SpectrumBean:
                        _graphicsB.Image = DrawSpectrumBean(ref _channel, ref _rect);
                        break;
                    case VisualType.SpectrumDot:
                        _graphicsB.Image = DrawSpectrumDot(ref _channel, ref _rect);
                        break;
                    case VisualType.SpectrumEllipse:
                        _graphicsB.Image = DrawSpectrumEllipse(ref _channel, ref _rect);
                        break;
                    case VisualType.SpectrumWave:
                        _graphicsB.Image = DrawSpectrumWave(ref _channel, ref _rect);
                        break;
                    case VisualType.WaveForm:
                        _graphicsB.Image = DrawWaveForm(ref _channel, ref _rect, false);
                        break;
                }
            }
            catch (Exception x)
            {
                MBoxHelper.ShowInfoMsg(x.Message, "");
            }
        }

        public static Bitmap DrawSpectrumLine(ref int channel, ref Rectangle r, bool drawPeak)
        {
            if (drawPeak)
            {
                return vs.CreateSpectrumLinePeak(channel, r.Width, r.Height, _base, _peak, _hold, _back,
                    _width, _width, _dist, _pdelay, _isLinear, _drawFull, _antiAlias);
            }
            else
            {
                return vs.CreateSpectrumLine(channel, r.Width, r.Height, _base, _peak, _back, _width,
                    _dist, _isLinear, _drawFull, _antiAlias);
            }

        }

        public static Bitmap DrawSpectrumFull(ref int channel, ref Rectangle r)
        {
            return vs.CreateSpectrum(channel, r.Width, r.Height, _base, _peak, _back, _isLinear, _drawFull, _antiAlias);
        }

        public static Bitmap DrawSpectrumBean(ref int channel, ref Rectangle r)
        {
            return vs.CreateSpectrumBean(channel, r.Width, r.Height, _base, _peak, _back, _width,
                _isLinear, _drawFull, _antiAlias);
        }

        public static Bitmap DrawSpectrumDot(ref int channel, ref Rectangle r)
        {
            return vs.CreateSpectrumDot(channel, r.Width, r.Height, _base, _peak, _back, _width, _dist,
                _isLinear, _drawFull, _antiAlias);
        }

        public static Bitmap DrawSpectrumEllipse(ref int channel, ref Rectangle r)
        {
            return vs.CreateSpectrumEllipse(channel, r.Width, r.Height, _base, _peak, _back, _width, _dist,
                _isLinear, _drawFull, _antiAlias);
        }

        public static Bitmap DrawSpectrumWave(ref int channel, ref Rectangle r)
        {
            return vs.CreateSpectrumWave(channel, r.Width, r.Height, _base, _peak, _back, _width,
                _isLinear, _drawFull, _antiAlias);
        }

        public static Bitmap DrawWaveForm(ref int channel, ref Rectangle r, bool mono)
        {
            return vs.CreateWaveForm(channel, r.Width, r.Height, _base, _peak, _hold, _back, _width,
                _drawFull, mono, _antiAlias);
        }

        #endregion Draw Bitmap


        public static void SetVisual(byte p)
        {
            SetVisual((VisualType)p);
        }

        public static int GetTimerInterval()
        {
            return _timer.Interval;
        }
    }
}
