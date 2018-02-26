using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class Knight: Piece
    {
        public Knight() { }
        public Knight(string square, string color) : base(square, "Knight", color)
        {
        }

        public override List<string> GetPossibleMoves()
        {
            List<string> _possibleMoves = new List<string>();
            ProtectsKingFrom = AmIProtectingMyKing();
            Piece myKing = color == "White" ? GetWhiteKing() : GetBlackKing();
            MyKingsAttacker = myKing.Attacker;
            if (MyKingsAttacker != null) // If our King is under attack
            {
                List<string> PossibleMoves = new List<string>();
                PossibleMoves.AddRange(GetMoves());
                return GetMovesWhenUnderAttack(PossibleMoves, myKing);
            }
            else if (ProtectsKingFrom != null) // If there is an enemy Piece making us not able to move.
            {
                return _possibleMoves;
            }
            else _possibleMoves.AddRange(GetMoves());
            return _possibleMoves;
        }
        public List<string> GetMoves()
        {
            int[,] Offsets = new int[,] { { 1, 2 }, { 2, 1 }, { 2, -1 }, { 1, -2 }, { -1, -2 }, { -2, -1 }, { -2, 1 }, { -1, 2 } };
            List<string> _possibleMoves = new List<string>();
            for (int k = 0; k < Offsets.Length / 2; k++)
            {
                if (squareLetterIndex + Offsets[k, 0] > 7 || squareLetterIndex + Offsets[k, 0] < 0 || squareNumber + Offsets[k, 1] > 8 || squareNumber + Offsets[k, 1] < 1) continue;
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + Offsets[k,0]] + (squareNumber + Offsets[k,1]));
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[squareLetterIndex + Offsets[k, 0]] + (squareNumber + Offsets[k, 1]));
                    continue;
                }
                else _possibleMoves.Add(squarePrefix[squareLetterIndex + Offsets[k, 0]] + (squareNumber + Offsets[k, 1])); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
    }
}
