using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flood_Control_Reimplement
{
    class GameBoard
    {
        const int gameBoardWidth = 8;
        const int gameBoardHeight = 10;

        readonly Random rng = new Random();

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

        private void PropagateWater(int y, int x)
        {
            if (y < 0 || x < 0 || y >= gameBoardHeight)
                return;
            if (x >= gameBoardWidth)
            {
                RemoveConnected(y, x - 1);
                return;
            }
            if (board[y, x].WaterFilled)
                return;

            board[y, x].WaterFilled = true;

            switch(board[y,x].Type)
            {
                case GamePiece.PieceType.LR:
                    PropagateWater(y, x - 1);
                    PropagateWater(y, x + 1);
                    break;
                case GamePiece.PieceType.UD:
                    PropagateWater(y - 1, x);
                    PropagateWater(y + 1, x);
                    break;
                case GamePiece.PieceType.UL:
                    PropagateWater(y - 1, x);
                    PropagateWater(y, x - 1);
                    break;
                case GamePiece.PieceType.UR:
                    PropagateWater(y - 1, x);
                    PropagateWater(y, x + 1);
                    break;
                case GamePiece.PieceType.DL:
                    PropagateWater(y + 1, x);
                    PropagateWater(y, x - 1);
                    break;
                case GamePiece.PieceType.DR:
                    PropagateWater(y + 1, x);
                    PropagateWater(y, x + 1);
                    break;
            }
        }

        private void RemoveConnected(int y, int x)
        {
            
        }

        #endregion
    }
}
