using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    class RotatingPiece : GamePiece, IAnimatedPiece
    {
        bool clockwise;
    
        public RotatingPiece(PieceType type, bool clockwise) : base(type)
        {
            this.clockwise = clockwise;
        }
        
        public override void UpdateAnimation()
        {
            
        }
        
        public override bool IsAnimationOver
        {
            get { return ; }
        }
    }
}