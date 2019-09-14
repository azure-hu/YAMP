using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace Azure.MediaUtils
{
	public class EqualiserEngine
	{
		private float[,] eqMatrix;
		private int _fxEQ;
		private bool _attached;
		private BASS_BFX_PEAKEQ eq;

		public EqualiserEngine(int stream, params float[] frequencies)
		{
			eqMatrix = new float[frequencies.Length, 2];

			AttachEQ(stream);

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
			_fxEQ = Bass.BASS_ChannelSetFX(stream, BASSFXType.BASS_FX_BFX_PEAKEQ, 0);
			_attached = (_fxEQ != 0);
			return _attached;
		}

		public bool DetachEQ(int stream)
		{
			_attached = Bass.BASS_ChannelRemoveFX(stream, _fxEQ);
			return _attached;
		}

		public void Reset(int band, float value)
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
				Bass.BASS_FXSetParameters(_fxEQ, _eq);
			}
		}

	}
}
