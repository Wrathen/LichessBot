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
            MyKingsAttackers = IsKingUnderAttack(color);
            Piece myKing = color == "White" ? GetWhiteKing() : GetBlackKing();
            if (MyKingsAttackers != null) // If our King is under attack
            {
                List<string> PossibleMoves = new List<string>();
                if (MyKingsAttackers[0].squareLetterIndex < squareLetterIndex) PossibleMoves.AddRange(GetMovesFromGoingLeft());
                else if(MyKingsAttackers[0].squareLetterIndex > squareLetterIndex) PossibleMoves.AddRange(GetMovesFromGoingRight());
                if(MyKingsAttackers[0].squareNumber < squareNumber) PossibleMoves.AddRange(GetMovesFromGoingDown());
                else if(MyKingsAttackers[0].squareNumber > squareNumber) PossibleMoves.AddRange(GetMovesFromGoingUp());

                if (PossibleMoves.Contains(MyKingsAttackers[0].square)) // Can We Capture The Attacker?
                {
                    _possibleMoves.Add(MyKingsAttackers[0].square);
                }

                if ((MyKingsAttackers[0].type == "Bishop" || MyKingsAttackers[0].type == "Rook" || MyKingsAttackers[0].type == "Queen")) // Can We Go Between Them?
                {
                    int xDistance = MyKingsAttackers[0].squareLetterIndex - myKing.squareLetterIndex;
                    int yDistance = MyKingsAttackers[0].squareNumber - myKing.squareNumber;
                    for (int x = xDistance / Math.Abs(xDistance); Math.Abs(x) < Math.Abs(xDistance); x += x / Math.Abs(x))
                    {
                        for(int y = yDistance / Math.Abs(yDistance); Math.Abs(y) < Math.Abs(yDistance); y += y / Math.Abs(y))
                        {
                            if(PossibleMoves.Contains(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + y))
                            {
                                _possibleMoves.Add(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + y);
                            }
                        }
                    }
                }
                return _possibleMoves;
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
