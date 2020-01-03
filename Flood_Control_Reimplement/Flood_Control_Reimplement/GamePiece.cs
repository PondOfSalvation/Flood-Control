using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    class GamePiece
    {
        #region Tile Sheet Pixel Constants

        const int tileMargin = 1;
        const int tilePadding = 1;
        const int tileWidth = 40;
        const int tileHeight = 40;

        #endregion

        #region Piece States

        // PieceType must follow the sprite Y Index(0-based) on Tile_Sheet.png
        public enum PieceType { LR, UD, UL, UR, DR, DL, Empty }
        public const int EmptyIndex = (int)PieceType.Empty;

        public PieceType Type { get; private set; } = PieceType.Empty;
        public bool WaterFilled { get; set; } = false;

        #endregion


        #region Constructors

        public GamePiece(PieceType type)
        {
            Type = type;
        }

        #endregion


        public virtual Rectangle SpriteRectangle
        {
            get
            {
                int tileX = (WaterFilled) ? 1 : 0;
                int tileY = (int)Type;

                int pixelX = tileMargin + tileX * (tileWidth + tilePadding);
                int pixelY = tileMargin + tileY * (tileHeight + tilePadding);

                return new Rectangle(pixelX, pixelY, tileWidth, tileHeight);
            }
        }

        public void Rotate(bool clockwise)
        {
            switch(Type)
            {
                case PieceType.LR: Type = PieceType.UD; break;
                case PieceType.UD: Type = PieceType.LR; break;
                case PieceType.UL: Type = (clockwise) ? PieceType.UR : PieceType.DL; break;
                case PieceType.UR: Type = (clockwise) ? PieceType.DR : PieceType.UL; break;
                case PieceType.DR: Type = (clockwise) ? PieceType.DL : PieceType.UR; break;
                case PieceType.DL: Type = (clockwise) ? PieceType.UL : PieceType.DR; break;
            }
        }
    }
}
