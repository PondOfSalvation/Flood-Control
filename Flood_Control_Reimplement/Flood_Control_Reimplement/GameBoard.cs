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
        
        public enum AnimationType { Rotate, Fade, Fall }
        
        int rotatingPiecesCount = 0;
        int fadingPiecesCount = 0;
        int fallingPiecesCount = 0;
        
        Dictionary<Point, IAnimatedPiece> animatingPieces = new Dictionary<Point, IAnimatedPiece>();

        bool IsAnimating
        {
            get { return (rotatingPiecesCount > 0) || (fadingPiecesCount > 0) || (fallingPiecesCount > 0);}
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
        
        public void AddAnimatingPiece(int y, int x, AnimationType animationType)
        {
            switch(animationType)
            {
                case AnimationType.Rotate:
                    ++rotatingPiecesCount;
                    // TODO : Fix actual argument
                    animatingPieces[new Point(x, y)] = new RotatingPiece();
                    break;
                case AnimationType.Fade:
                    ++fadingPiecesCount;
                    animatingPieces[new Point(x, y)] = new FadingPiece(board[y,x].Type);
                    break;
                case AnimationType.Fall:
                    ++fallingPiecesCount;
                    // TODO : Fix actual argument
                    animatingPieces[new Point(x, y)] = new FallingPiece();
                    break;
                    
            }
        }
        
        public void UpdateAnimatingPieces()
        {
            var keysToRemove = new Queue<Point>;
            bool isFading = (fadingPiecesCount <= 0);
            
            foreach(var item in animatingPieces)
            {
                // if some pieces are fading, skip updating other animations.
                if (isFading && !(item.Value is FadingPiece))
                    continue;
                
                item.Value.UpdateAnimation();
                
                if (item.Value.IsAnimationOver)
                {
                    keysToRemove.Enqueue(item.Key);
                    
                    if (item.Value is RotatingPiece)
                        --rotatingPiecesCount;
                    else if (item.Value is FadingPiece)
                        --fadingPiecesCount;
                    else // item.Value is FallingPiece
                        --fallingPiecesCount;
                }
            }
            
            while (keysToRemove.Count > 0)
                animatingPieces.Remove(keysToRemove.Dequeue());
        }

        #endregion
    }
}
