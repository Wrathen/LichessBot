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
                if (MyKingsAttackers[0].squareLetterIndex == myKing.squareLetterIndex) // Is the attacker attacking my queen vertical?
                {
                    // biri bizim şaha saldırıyorsa önüne geçmeye çalışıcaz. Ama önüne geçmeye çalışırken önce
                    // biz kale olduğumuz için anca sağ sol üst alt gidebiliyoruz. önce aynı letterdamıyız falan diye bakıcan
                    // sonra eğer aynı letterdaysak falan önüne tek tek bakıcan, geçmen gereken yere kadar boşsa geç oraya.
                    // veya son bakman gereken yerde asıl attackerın varsa, onu yiyebilirsin
                }
                else if (MyKingsAttackers[0].squareNumber == myKing.squareNumber) // Is the attacker attacking my queen horizontal?
                {

                }
                return _possibleMoves;
            } 
            if (ProtectsKingFrom != null) // If there is an enemy Piece making us not able to move.
            {
                if (ProtectsKingFrom.squareLetterIndex == squareLetterIndex) // Check Horizontal
                {
                    for(int i = 1; i < ProtectsKingFrom.squareLetterIndex + 1; i++)
                    {
                        _possibleMoves.Add(squarePrefix[squareLetterIndex + i] + squareNumber); // We can go horizontal as far as to the enemy piece we are protecting our king from.
                    }
                }
                else if (ProtectsKingFrom.squareNumber == squareNumber) // Check Vertical
                {
                    for (int i = 1; i < ProtectsKingFrom.squareNumber + 1; i++)
                    {
                        _possibleMoves.Add(squarePrefix[squareLetterIndex] + (squareNumber + i)); // We can go vertical as far as to the enemy piece we are protecting our king from.
                    }
                }
                return _possibleMoves;
            }
            for (int k = squareLetterIndex + 1; k < squarePrefix.Length; k++) // Gonna Check Right Squares
            {
                if (k == 8) break; // letterIndex 7 means h squares like h1-h2-...-h8. If we are at h square then dont look any more right. Because h is the rightest.
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[k] + squareNumber);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if(PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[k] + squareNumber);
                    break;
                }
                else _possibleMoves.Add(squarePrefix[k] + squareNumber); // If no piece on our way then its a possible move
            }
            for (int k = squareLetterIndex - 1; k < squarePrefix.Length; k--) // Gonna Check Left Squares
            {
                if (k == -1) break; // letterIndex 0 means a squares like a1-a2-...-a8. If we are at a square then dont look any more left. Because a is the leftest.
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[k] + squareNumber);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[k] + squareNumber);
                    break;
                }
                else _possibleMoves.Add(squarePrefix[k] + squareNumber); // If no piece on our way then its a possible move
            }
            for (int k = squareNumber + 1; k < squarePrefix.Length; k++) // Gonna Check Up Squares
            {
                if (k == 9) break; // If we are at 8th square then dont look any more upper. Because 8th is the uppest.
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squareLetter + k);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squareLetter + k);
                    break;
                }
                else _possibleMoves.Add(squareLetter + k); // If no piece on our way then its a possible move
            }
            for (int k = squareNumber - 1; k < squarePrefix.Length; k--) // Gonna Check Down Squares
            {
                if (k == 0) break; // If we are at 1st square then dont look any more down. Because 1st is the downest.
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squareLetter + k);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squareLetter + k);
                    break;
                }
                else _possibleMoves.Add(squareLetter + k); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
    }
}
