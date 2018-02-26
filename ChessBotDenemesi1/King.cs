using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class King: Piece
    {
        public King() { }
        public King(string square, string color) : base(square, "King", color)
        {
        }

        public override List<string> GetPossibleMoves()
        {
            List<string> _possibleMoves = new List<string>();
            int[,] Offsets = new int[,] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
            string squareWeAreChecking = "";
            Piece PieceOnTheSquare = null;
            for(int k = 0; k < Offsets.Length / 2; k++)
            {
                if (squareLetterIndex + Offsets[k, 0] > 7 || squareLetterIndex + Offsets[k, 0] < 0 || squareNumber + Offsets[k, 1] > 8 || squareNumber + Offsets[k, 1] < 1) continue;
                squareWeAreChecking = squarePrefix[squareLetterIndex + Offsets[k, 0]] + (squareNumber + Offsets[k, 1]);
                if (IsSquareDangerous(squareWeAreChecking)) continue;
                PieceOnTheSquare = IsAnyPieceOnThisSquare(squareWeAreChecking);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color)
                    {
                        _possibleMoves.Add(squareWeAreChecking);
                    }
                    continue;
                }
                else _possibleMoves.Add(squareWeAreChecking); // If no piece on our way and the square is safe.
            }
            return _possibleMoves;
        }
        
        private bool IsSquareDangerous(string square)
        {
            List<Piece> EnemyPieces = color == "White" ? Form1.BlackPieces : Form1.WhitePieces;
            foreach (Piece p in EnemyPieces)
            {
                if (p.possibleMoves.Count > 0 && p.possibleMoves.Contains(square))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
