using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
    public class TrackBarDrawItemEventArgs : EventArgs
    {
        #region Fields

        private Rectangle _bounds;
        private Graphics _graphics;
        private TrackBarItemState _state;

        #endregion

        #region Methods

        public TrackBarDrawItemEventArgs(Graphics graphics, Rectangle bounds, TrackBarItemState state)
        {
            this._graphics = graphics;
            this._bounds = bounds;
            this._state = state;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounds
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return this._bounds;
            }
        }

        /// <summary>
        /// Gets the graphics context
        /// </summary>
        public Graphics Graphics
        {
            get
            {
                return this._graphics;
            }
        }

        /// <summary>
        /// Gets the state of the item
        /// </summary>
        public TrackBarItemState State
        {
            get
            {
                return this._state;
            }
        }

        #endregion
    }

}
