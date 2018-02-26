using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class Rook: Piece
    {
        public Rook() { }
        public Rook(string square, string color) : base(square,"Rook",color)
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
                if (MyKingsAttacker.squareLetterIndex < squareLetterIndex) PossibleMoves.AddRange(GetMovesFromGoingLeft());
                else if(MyKingsAttacker.squareLetterIndex > squareLetterIndex) PossibleMoves.AddRange(GetMovesFromGoingRight());
                if(MyKingsAttacker.squareNumber < squareNumber) PossibleMoves.AddRange(GetMovesFromGoingDown());
                else if(MyKingsAttacker.squareNumber > squareNumber) PossibleMoves.AddRange(GetMovesFromGoingUp());
                return GetMovesWhenUnderAttack(PossibleMoves, myKing);
            }
            else if (ProtectsKingFrom != null) // If there is an enemy Piece making us not able to move.
            {
                if (ProtectsKingFrom.squareLetterIndex == squareLetterIndex) // Check Vertical
                {
                    for (int i = 1; i < Math.Abs(squareNumber - ProtectsKingFrom.squareNumber + 1); i++)
                    {
                        if(ProtectsKingFrom.squareNumber > squareNumber) // Attacker is above us.
                        {
                            _possibleMoves.Add(squarePrefix[squareLetterIndex] + (squareNumber + i));
                        }
                        else // Attacker is below us.
                        {
                            _possibleMoves.Add(squarePrefix[squareLetterIndex] + (squareNumber - i));
                        }
                    }
                }
                else if (ProtectsKingFrom.squareNumber == squareNumber) // Check Horizontal
                {
                    for (int i = 1; i < Math.Abs(squareLetterIndex - ProtectsKingFrom.squareLetterIndex + 1); i++)
                    {
                        if(ProtectsKingFrom.squareLetterIndex > squareNumber) // Attacker is Righter than us.
                        {
                            _possibleMoves.Add(squarePrefix[squareLetterIndex + i] + squareNumber);
                        }
                        else // Attacker is Lefter than us.
                        {
                            _possibleMoves.Add(squarePrefix[squareLetterIndex - i] + squareNumber);
                        }
                    }
                }
                return _possibleMoves;
            }
            else
            {
                _possibleMoves.AddRange(GetMovesFromGoingUp());
                _possibleMoves.AddRange(GetMovesFromGoingRight());
                _possibleMoves.AddRange(GetMovesFromGoingDown());
                _possibleMoves.AddRange(GetMovesFromGoingLeft());
            }
            return _possibleMoves;
        }
    }
}
