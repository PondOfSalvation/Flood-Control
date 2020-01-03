using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flood_Control_Reimplement
{
    class GameBoard
    {
        const int gameBoardWidth = 8;
        const int gameBoardHeight = 10;
        
        readonly Random rng = new Random();
        
        Dictionary<Point, RotatingPiece> rotatingPieces = new Dictionary<Point, RotatingPiece>();
        Dictionary<Point, FadingPiece> fadingPieces = new Dictionary<Point, FadingPiece>();
        Dictionary<Point, FallingPiece> fallingPieces = new Dictionary<Point, FallingPiece>();
        
        bool IsAnimating
        {
            get { return (rotatingPieces.Count > 0) || (fadingPieces.Count > 0) || (fallingPieces.Count > 0);}
        }

        GamePiece[,] board = new GamePiece[gameBoardHeight, gameBoardWidth];

        public GameBoard()
        {
            ResetBoard();
        }

        public GamePiece this[int y, int x]
        {
            get { return board[y, x]; }
        }

        public void RotatePiece(int y, int x, bool clockwise)
        {
            board[y, x].Rotate(clockwise);
            ClearWater();
            for (int i = 0; i < gameBoardHeight; ++i)
                switch(board[i,0].Type)
                {
                    case GamePiece.PieceType.LR:
                    case GamePiece.PieceType.UL:
                    case GamePiece.PieceType.DL:
                        PropagateWater(i, 0);
                        break;
                }
        }

        public void ResetBoard()
        {
            for (int y = 0; y < gameBoardHeight; ++y)
                for (int x = 0; x < gameBoardWidth; ++x)
                {
                    GamePiece.PieceType randomPiece = (GamePiece.PieceType)rng.Next(GamePiece.EmptyIndex);
                    board[y, x] = new GamePiece(randomPiece);
                }
        }

        #region Private Helper Functions

        private void ClearWater()
        {
            for (int y=0;y<gameBoardHeight;++y)
                for (int x=0;x<gameBoardWidth;++x)
                {
                    board[y, x].WaterFilled = false;
                }
        }

        // Returns true if the score condition is met
        private bool PropagateWater(int y, int x)
        {
            bool hasConnected = false;
            
            if (y < 0 || x < 0 || y >= gameBoardHeight)
                return false;
            if (x >= gameBoardWidth)
            {
                AddFadingPiece(y, x);
                return (hasConnected = true);
            }
            if (board[y, x].WaterFilled)
                return false;

            board[y, x].WaterFilled = true;

       
            switch(board[y,x].Type)
            {
                case GamePiece.PieceType.LR:
                    hasConnected = hasConnected || PropagateWater(y, x - 1);
                    hasConnected = hasConnected || PropagateWater(y, x + 1);
                    break;
                case GamePiece.PieceType.UD:
                    hasConnected = hasConnected || PropagateWater(y - 1, x);
                    hasConnected = hasConnected || PropagateWater(y + 1, x);
                    break;
                case GamePiece.PieceType.UL:
                    hasConnected = hasConnected || PropagateWater(y - 1, x);
                    hasConnected = hasConnected || PropagateWater(y, x - 1);
                    break;
                case GamePiece.PieceType.UR:
                    hasConnected = hasConnected || PropagateWater(y - 1, x);
                    hasConnected = hasConnected || PropagateWater(y, x + 1);
                    break;
                case GamePiece.PieceType.DL:
                    hasConnected = hasConnected || PropagateWater(y + 1, x);
                    hasConnected = hasConnected || PropagateWater(y, x - 1);
                    break;
                case GamePiece.PieceType.DR:
                    hasConnected = hasConnected || PropagateWater(y + 1, x);
                    hasConnected = hasConnected || PropagateWater(y, x + 1);
                    break;
            }
            
            if (hasConnected)
            {
                AddFadingPiece(y, x);
            }
            
            return hasConnected;
        }
        
        private void AddFadingPiece(int y, int x)
        {
            fadingPieces[new Point(x, y)] = new FadingPiece(board[y,x].Type);
        }

        #endregion
    }
}
