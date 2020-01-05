using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    class FadingPiece : GamePiece, IAnimatedPiece
    {
        public const float alphaChangeRate = 0.02f;
        public float Alpha { get; set; } = 1.0f;
        
        public FadingPiece(PieceType type) : base(type) { }
        
        public override void UpdateAnimation()
        {
            Alpha = MathHelper.Max(0f, Alpha - alphaChangeRate);
        }
        
        public override bool IsAnimationOver
        {
            get { return (Alpha <= 0.0f); }
        }
    }
}