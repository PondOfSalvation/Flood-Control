using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    class FallingPiece : GamePiece, IAnimatedPiece
    {
        public FallingPiece(PieceType type) : base(type) { }
        
        public override void UpdateAnimation()
        {
            
        }
        
        public override bool IsAnimationOver
        {
            get { return ; }
        }
    }
}