using System;
using System.Collections.Generic;

using System.Text;

namespace Fusionbird.FusionToolkit.FusionTrackBar
{
    [Flags]
    public enum TrackBarOwnerDrawParts
    {
        Channel = 4,
        None = 0,
        Thumb = 2,
        Ticks = 1
    }
}
