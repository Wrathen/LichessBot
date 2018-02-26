using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class Pawn: Piece
    {
        public Pawn() { }
        public Pawn(string square, string color) : base(square, "Pawn", color)
        {
        }

        public override List<string> GetPossibleMoves()
        {
            List<string> _possibleMoves = new List<string>();
            Piece myKing = color == "White" ? GetWhiteKing() : GetBlackKing();
            ProtectsKingFrom = AmIProtectingMyKing();
            MyKingsAttacker = myKing.Attacker;
            if (MyKingsAttacker != null) // If our King is under attack
            {
                return GetMovesWhenUnderAttack(GetPossibleMoves(), myKing);
            }
            else if (ProtectsKingFrom != null) // If there is an enemy Piece making us not able to move.
            {
                if (ProtectsKingFrom.squareLetterIndex == squareLetterIndex && ProtectsKingFrom.squareNumber > squareNumber) // If the attacker is above us
                {
                    if (squareNumber == 2 && !moved && color == "White") _possibleMoves.Add(squareLetter + (squareNumber + 2));
                    if (squareNumber == 7 && !moved && color == "Black") _possibleMoves.Add(squareLetter + (squareNumber - 2));
                    if (color == "White") _possibleMoves.Add(squareLetter + (squareNumber + 1));
                    else _possibleMoves.Add(squareLetter + (squareNumber - 1));
                }
                else if (ProtectsKingFrom.squareNumber == squareNumber + 1 && ProtectsKingFrom.squareLetterIndex == squareLetterIndex + 1) // If the attacker is 1 Square Above and 1 Square Righter than Us
                {
                    if (color == "White") _possibleMoves.Add(squarePrefix[squareLetterIndex + 1] + (squareNumber + 1));
                    else _possibleMoves.Add(squarePrefix[squareLetterIndex + 1] + (squareNumber - 1));
                }
                else if (ProtectsKingFrom.squareNumber == squareNumber + 1 && ProtectsKingFrom.squareLetterIndex == squareLetterIndex - 1) // If the attacker is 1 Square Above and 1 Square Lefter than Us
                {
                    if (color == "White") _possibleMoves.Add(squarePrefix[squareLetterIndex - 1] + (squareNumber + 1));
                    else _possibleMoves.Add(squarePrefix[squareLetterIndex - 1] + (squareNumber - 1));
                }
                return _possibleMoves;
            }
            else _possibleMoves.AddRange(GetMoves());
            return _possibleMoves;
        }
        public List<string> GetMoves()
        {
            List<string> _possibleMoves = new List<string>();
            if (squareNumber == 2 && !moved && color == "White") _possibleMoves.Add(squareLetter + (squareNumber + 2));
            if (squareNumber == 7 && !moved && color == "Black") _possibleMoves.Add(squareLetter + (squareNumber - 2));
            if (color == "White") _possibleMoves.Add(squareLetter + (squareNumber + 1));
            else _possibleMoves.Add(squareLetter + (squareNumber - 1));
            return _possibleMoves;
        }
    }
}