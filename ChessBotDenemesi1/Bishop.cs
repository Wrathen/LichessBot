using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class Bishop: Piece
    {
        public Bishop() { }
        public Bishop(string square, string color) : base(square, "Bishop", color)
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
                if (MyKingsAttacker.squareLetterIndex < squareLetterIndex && MyKingsAttacker.squareNumber < squareNumber) PossibleMoves.AddRange(GetMovesFromGoingDownLeft());
                else if (MyKingsAttacker.squareLetterIndex > squareLetterIndex && MyKingsAttacker.squareNumber > squareNumber) PossibleMoves.AddRange(GetMovesFromGoingUpRight());
                else if (MyKingsAttacker.squareLetterIndex < squareLetterIndex && MyKingsAttacker.squareNumber > squareNumber) PossibleMoves.AddRange(GetMovesFromGoingUpLeft());
                else if (MyKingsAttacker.squareLetterIndex > squareLetterIndex && MyKingsAttacker.squareNumber < squareNumber) PossibleMoves.AddRange(GetMovesFromGoingDownRight());
                return GetMovesWhenUnderAttack(PossibleMoves, myKing);
            }
            else if (ProtectsKingFrom != null) // If there is an enemy Piece making us not able to move.
            {
                if (Math.Abs(ProtectsKingFrom.squareLetterIndex - squareLetterIndex) == Math.Abs(ProtectsKingFrom.squareNumber - squareNumber)) // Check Diagonal
                {
                    int xDistance = ProtectsKingFrom.squareLetterIndex - squareLetterIndex;
                    int yDistance = ProtectsKingFrom.squareNumber - squareNumber;
                    int diagonalDistance = Math.Abs(xDistance) > Math.Abs(yDistance) ? xDistance : yDistance;
                    for (int x = 1; x < 9 - Math.Abs(diagonalDistance); x++)
                    {
                        if (xDistance > 0 && yDistance > 0) _possibleMoves.Add(squarePrefix[squareLetterIndex + x] + (squareNumber + x));
                        else if (xDistance > 0 && yDistance < 0) if (xDistance > 0 && yDistance > 0) _possibleMoves.Add(squarePrefix[squareLetterIndex + x] + (squareNumber - x));
                        else if (xDistance < 0 && yDistance > 0) _possibleMoves.Add(squarePrefix[squareLetterIndex - x] + (squareNumber + x));
                        else _possibleMoves.Add(squarePrefix[squareLetterIndex - x] + (squareNumber - x));
                    }
                }
                return _possibleMoves;
            }
            else
            {
                _possibleMoves.AddRange(GetMovesFromGoingUpRight());
                _possibleMoves.AddRange(GetMovesFromGoingUpLeft());
                _possibleMoves.AddRange(GetMovesFromGoingDownRight());
                _possibleMoves.AddRange(GetMovesFromGoingDownLeft());
            }
            return _possibleMoves;
        }
    }
}
