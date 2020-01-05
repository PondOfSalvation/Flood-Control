using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    interface IAnimatedPiece
    {
        void UpdateAnimation();
        bool IsAnimationOver { get; }
    }
}