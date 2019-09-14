using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using Azure.LibCollection.CS;

namespace Azure.MediaUtils
{
	public class Visualizer
	{
		public enum VisualType
		{
			SpectrumLineN, SpectrumLineP, SpectrumFull, SpectrumBean,
			SpectrumDot, SpectrumEllipse, SpectrumWave, WaveForm
		}

		private Visuals vs;
		private Color _base = Color.GreenYellow, _peak = Color.IndianRed, _hold = Color.Gray, _back = Color.Transparent;
		private int _width = 2, _dist = 1, _pdelay = 5, _channel;
		private bool _isLinear = true, _drawFull = true, _antiAlias = true, _isReady = false;
		private BASSTimer _timer;
		private Graphics _graphicsG;
		private System.Windows.Forms.PictureBox _graphicsB;
		private Rectangle _rect;
		private VisualType _vstype = VisualType.SpectrumLineP;

		private void BaseInit()
		{
			vs = new Visuals();
			_timer = new BASSTimer();
			_timer.Enabled = false;
			_isReady = true;
		}

		#region Control Methods

		private void Init(Graphics g, Rectangle rectangle)
		{
			_graphicsG = g;
			_rect = rectangle;
			BaseInit();
			_timer.Tick += new EventHandler(_timer_Graphics_Tick);
		}

		public void Init(ref System.Windows.Forms.PictureBox g, Rectangle rectangle)
		{
			_graphicsB = g;
			_rect = rectangle;
			BaseInit();
			_timer.Tick += new EventHandler(_timer_Bmp_Tick);
		}

		public Visualizer(Graphics g, Rectangle rectangle, int timerInterval = 1000)
		{
			this.Init(g, rectangle);
			_timer.Interval = timerInterval;
		}

		public Visualizer(ref System.Windows.Forms.PictureBox g)
		{
			Init(ref g, g.ClientRectangle);
		}


		public Visualizer(ref System.Windows.Forms.PictureBox g, int timerInterval)
		{
			Init(ref g, g.ClientRectangle);
			_timer.Interval = timerInterval;
		}

		public Visualizer(ref System.Windows.Forms.PictureBox g, Rectangle rectangle, int timerInterval)
		{
			Init(ref g, rectangle);
			_timer.Interval = timerInterval;
		}

		public void Set(string param, object value)
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


		public Color GetColor(string param)
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

		public void SetVisual(VisualType value)
		{
			_vstype = (value > VisualType.WaveForm ? VisualType.SpectrumLineN : value);
		}

		public VisualType GetVSType { get { return _vstype; } }

		public bool IsReady { get { return _isReady; } }

		public void StartDraw(int channel)
		{
			_channel = channel;
			_timer.Enabled = true;
		}

		public void StopDraw()
		{
			_timer.Enabled = false;
		}

		#endregion Control Methods

		#region Draw With Graphics

		private void _timer_Graphics_Tick(object sender, EventArgs e)
		{
			try
			{
				switch (_vstype)
				{
					case VisualType.SpectrumLineP:
						DrawSpectrumLine(ref _channel, _graphicsG, ref _rect, true);
						break;
					case VisualType.SpectrumLineN:
						DrawSpectrumLine(ref _channel, _graphicsG, ref _rect, false);
						break;
					case VisualType.SpectrumFull:
						DrawSpectrumFull(ref _channel, _graphicsG, ref _rect);
						break;
					case VisualType.SpectrumBean:
						DrawSpectrumBean(ref _channel, _graphicsG, ref _rect);
						break;
					case VisualType.SpectrumDot:
						DrawSpectrumDot(ref _channel, _graphicsG, ref _rect);
						break;
					case VisualType.SpectrumEllipse:
						DrawSpectrumEllipse(ref _channel, _graphicsG, ref _rect);
						break;
					case VisualType.SpectrumWave:
						DrawSpectrumWave(ref _channel, _graphicsG, ref _rect);
						break;
					case VisualType.WaveForm:
						DrawWaveForm(ref _channel, _graphicsG, ref _rect, false);
						break;
				}
			}

			catch (Exception x)
			{
				MBoxHelper.ShowInfoMsg(x.Message, "");
			}
		}

		public bool DrawSpectrumLine(ref int channel, Graphics g, ref Rectangle r, bool drawPeak)
		{
			if (drawPeak)
			{
				return vs.CreateSpectrumLinePeak(channel, g, r, _base, _peak, _hold, _back, _width, _width, _dist,
					_pdelay, _isLinear, _drawFull, _antiAlias);
			}
			else
			{
				return vs.CreateSpectrumLine(channel, g, r, _base, _peak, _back, _width, _dist, _isLinear,
					_drawFull, _antiAlias);
			}

		}

		public bool DrawSpectrumFull(ref int channel, Graphics g, ref Rectangle r)
		{
			return vs.CreateSpectrum(channel, g, r, _base, _peak, _back, _isLinear, _drawFull, _antiAlias);
		}

		public bool DrawSpectrumBean(ref int channel, Graphics g, ref Rectangle r)
		{
			return vs.CreateSpectrumBean(channel, g, r, _base, _peak, _back, _width,
				_isLinear, _drawFull, _antiAlias);
		}

		public bool DrawSpectrumDot(ref int channel, Graphics g, ref Rectangle r)
		{
			return vs.CreateSpectrumDot(channel, g, r, _base, _peak, _back, _width, _dist,
				_isLinear, _drawFull, _antiAlias);
		}

		public bool DrawSpectrumEllipse(ref int channel, Graphics g, ref Rectangle r)
		{
			return vs.CreateSpectrumEllipse(channel, g, r, _base, _peak, _back, _width, _dist,
				_isLinear, _drawFull, _antiAlias);
		}

		public bool DrawSpectrumWave(ref int channel, Graphics g, ref Rectangle r)
		{
			return vs.CreateSpectrumWave(channel, g, r, _base, _peak, _back, _width,
				_isLinear, _drawFull, _antiAlias);
		}

		public bool DrawWaveForm(ref int channel, Graphics g, ref Rectangle r, bool mono)
		{
			return vs.CreateWaveForm(channel, g, r, _base, _peak, _hold, _back, _width,
				_drawFull, mono, _antiAlias);
		}

		#endregion Draw With Graphics

		#region Draw Bitmap



		private void _timer_Bmp_Tick(object sender, EventArgs e)
		{
			try
			{
				_graphicsB.Image = GetImage(0);
			}
			catch (Exception)
			{
				_graphicsB.Image = null;
				//MBoxHelper.ShowInfoMsg(x.Message, "");
			}
		}

		public Image GetImage(int channel /*= 0*/)
		{
			if (_channel <= 0 && channel != 0)
				_channel = channel;

			Bitmap _img = new Bitmap(_rect.Width, _rect.Height);
			try
			{
				switch (_vstype)
				{
					case VisualType.SpectrumLineP:
						_img = DrawSpectrumLine(ref _channel, ref _rect, true);
						break;
					case VisualType.SpectrumLineN:
						_img = DrawSpectrumLine(ref _channel, ref _rect, false);
						break;
					case VisualType.SpectrumFull:
						_img = DrawSpectrumFull(ref _channel, ref _rect);
						break;
					case VisualType.SpectrumBean:
						_img = DrawSpectrumBean(ref _channel, ref _rect);
						break;
					case VisualType.SpectrumDot:
						_img = DrawSpectrumDot(ref _channel, ref _rect);
						break;
					case VisualType.SpectrumEllipse:
						_img = DrawSpectrumEllipse(ref _channel, ref _rect);
						break;
					case VisualType.SpectrumWave:
						_img = DrawSpectrumWave(ref _channel, ref _rect);
						break;
					case VisualType.WaveForm:
						_img = DrawWaveForm(ref _channel, ref _rect, false);
						break;
				}
			}
			catch (Exception)
			{
			}

			_img.MakeTransparent();

			return _img;
		}

		public Bitmap DrawSpectrumLine(ref int channel, ref Rectangle r, bool drawPeak)
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

		public Bitmap DrawSpectrumFull(ref int channel, ref Rectangle r)
		{
			return vs.CreateSpectrum(channel, r.Width, r.Height, _base, _peak, _back, _isLinear, _drawFull, _antiAlias);
		}

		public Bitmap DrawSpectrumBean(ref int channel, ref Rectangle r)
		{
			return vs.CreateSpectrumBean(channel, r.Width, r.Height, _base, _peak, _back, _width,
				_isLinear, _drawFull, _antiAlias);
		}

		public Bitmap DrawSpectrumDot(ref int channel, ref Rectangle r)
		{
			return vs.CreateSpectrumDot(channel, r.Width, r.Height, _base, _peak, _back, _width, _dist,
				_isLinear, _drawFull, _antiAlias);
		}

		public Bitmap DrawSpectrumEllipse(ref int channel, ref Rectangle r)
		{
			return vs.CreateSpectrumEllipse(channel, r.Width, r.Height, _base, _peak, _back, _width, _dist,
				_isLinear, _drawFull, _antiAlias);
		}

		public Bitmap DrawSpectrumWave(ref int channel, ref Rectangle r)
		{
			return vs.CreateSpectrumWave(channel, r.Width, r.Height, _base, _peak, _back, _width,
				_isLinear, _drawFull, _antiAlias);
		}

		public Bitmap DrawWaveForm(ref int channel, ref Rectangle r, bool mono)
		{
			return vs.CreateWaveForm(channel, r.Width, r.Height, _base, _peak, _hold, _back, _width,
				_drawFull, mono, _antiAlias);
		}

		#endregion Draw Bitmap


		public void SetVisual(byte p)
		{
			SetVisual((VisualType)p);
		}

		public int GetTimerInterval()
		{
			return _timer.Interval;
		}
	}
}
