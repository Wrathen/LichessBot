﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    public abstract class Piece
    {
        public string square = ""; // Like e4
        public string squareLetter = ""; // Like e4's e
        public int squareNumber = 0; // Like e4's 4
        public int squareLetterIndex = 0; // Index of squarePrefix
        public static string[] squarePrefix = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" }; // All Possible Square Letters
        public string type = ""; // Like Rook
        public string typePrefix = ""; // Like R,K,N etc..
        public string color = ""; // Like White
        public bool moved = false; // For Castling and En Passant
        public List<string> possibleMoves = new List<string>();
        // Enemy Pieces
        public static bool whiteIsUnderAttack = false;
        public static bool blackIsUnderAttack = false;
        public Piece ProtectsKingFrom = null;
        public Piece MyKingsAttacker = null;
        // King Special Variable
        public Piece Attacker = null;
        // Some Variables Written Here for Maybe more optimisation? They are used in AmIProtectingMyKing Function.
        // So we dont constantly create more variables instead we create them once and store on every piece.
        private int diagonalDistance = 0;
        private int diagonalLetterDistance = 0;
        private int diagonalNumberDistance = 0;

        public Piece() { }
        public Piece(string square, string type, string color)
        {
            this.square = square;
            this.type = type;
            this.color = color;
            this.squareLetter = square.Substring(0, 1);
            this.squareNumber = Convert.ToInt32(square.Substring(1, 1));
            if (type.Substring(0, 1) == "P") typePrefix = ""; // Pawns arent written to moves. ie: move e4 doesnt have Pawn prefix.
            else if (type.Substring(0, 1) == "K")
            {
                if (type.Substring(1, 1) == "n") typePrefix = "N"; // We are looking for Kn in the string. So We can say Its Knight and we name it as N.
                else typePrefix = "K"; // If we failed to see Kn in the string Then its King for sure...
            }
            else typePrefix = type.Substring(0, 1); // If our piece isnt Pawn or Knight or King. B for Bishop, R for Rook etc..
            for (int i = 0; i < squarePrefix.Length; i++) { if (squarePrefix[i] == squareLetter) squareLetterIndex = i; }
        }

        public abstract List<string> GetPossibleMoves();
        public static Piece GetWhiteKing()
        {
            foreach (Piece p in Form1.WhitePieces) { if (p.type == "King") return p; }
            return null;
        }
        public static Piece GetBlackKing()
        {
            foreach (Piece p in Form1.BlackPieces) { if (p.type == "King") return p; }
            return null;
        }

        public List<string> GetMovesWhenUnderAttack(List<string> PossibleMoves, Piece myKing)
        {
            List<string> _possibleMoves = new List<string>();

            if (PossibleMoves.Contains(MyKingsAttacker.square)) // Can We Capture The Attacker?
            {
                _possibleMoves.Add(MyKingsAttacker.square);
            }

            if ((MyKingsAttacker.type == "Bishop" || MyKingsAttacker.type == "Rook" || MyKingsAttacker.type == "Queen")) // Can We Go Between Them?
            {
                int xDistance = MyKingsAttacker.squareLetterIndex - myKing.squareLetterIndex;
                int yDistance = MyKingsAttacker.squareNumber - myKing.squareNumber;
                if (Math.Abs(xDistance) == Math.Abs(yDistance)) // Diagonal
                {
                    int diagonalDistance = Math.Abs(xDistance) > Math.Abs(yDistance) ? xDistance : yDistance;
                    for (int x = diagonalDistance / Math.Abs(diagonalDistance); Math.Abs(x) < 9 - Math.Abs(diagonalDistance); x += x / Math.Abs(x))
                    {
                        if (PossibleMoves.Contains(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + x))
                        {
                            _possibleMoves.Add(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + x);
                            break;
                        }
                    }
                }
                else // Orthogonal
                {
                    for (int x = xDistance / Math.Abs(xDistance); Math.Abs(x) < Math.Abs(xDistance); x += x / Math.Abs(x))
                    {
                        for (int y = yDistance / Math.Abs(yDistance); Math.Abs(y) < Math.Abs(yDistance); y += y / Math.Abs(y))
                        {
                            if (PossibleMoves.Contains(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + y))
                            {
                                _possibleMoves.Add(squarePrefix[x + myKing.squareLetterIndex] + myKing.squareNumber + y);
                                break;
                            }
                        }
                    }
                }
            }
            return _possibleMoves;
        }
        public static void SetKingsAttackers(string color)
        {
            Piece King = color == "White" ? GetWhiteKing() : GetBlackKing();
            Piece Attacker = null;
            if (color == "White") // Looking If White King is Under Attack
            {
                foreach (Piece p in Form1.BlackPieces)
                {
                    if (p.possibleMoves.Contains(King.square))
                    {
                        Attacker = p;
                        blackIsUnderAttack = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (Piece p in Form1.WhitePieces)
                {
                    if (p.possibleMoves.Contains(King.square))
                    {
                        Attacker = p;
                        whiteIsUnderAttack = true;
                        break;
                    }
                }
            }
            if (Attacker == null)
            {
                whiteIsUnderAttack = false;
                blackIsUnderAttack = false;
            }
            else
            {
                if (Attacker.color == "White") whiteIsUnderAttack = false;
                else blackIsUnderAttack = false;
            }
            King.Attacker = Attacker;
        }
        public Piece AmIProtectingMyKing()
        {
            int iX = 0;
            int iY = 0;
            Piece pieceImChecking = null;
            Piece myKing = color == "White" ? GetWhiteKing() : GetBlackKing();
            diagonalDistance = 0;
            diagonalLetterDistance = 0;
            diagonalNumberDistance = 0;
            if (myKing.squareLetter == squareLetter) // Checking Horizontal
            {
                for (int i = 1; i < 8; i++)
                {
                    iY = squareNumber > myKing.squareNumber ? i : -i;
                    if (squareNumber + iY > 8 || squareNumber + iY < 1) break;
                    pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex] + (squareNumber + iY));
                    if (pieceImChecking != null)
                    {
                        if (pieceImChecking.color != color && (pieceImChecking.type == "Rook" || pieceImChecking.type == "Queen")) return pieceImChecking;
                        else return null;
                    }
                }
            }
            else if (myKing.squareNumber == squareNumber) // Checking Vertical
            {
                for (int i = 1; i < 8; i++)
                {
                    iX = squareLetterIndex > myKing.squareLetterIndex ? i : -i;
                    if (squareLetterIndex + iX > 7 || squareLetterIndex + iX < 0) break;
                    pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + iX] + squareNumber);
                    if (pieceImChecking != null)
                    {
                        if (pieceImChecking.color != color && (pieceImChecking.type == "Rook" || pieceImChecking.type == "Queen")) return pieceImChecking;
                        else return null;
                    }
                }
            }
            else if (Math.Abs(squareLetterIndex - myKing.squareLetterIndex) == Math.Abs(squareNumber - myKing.squareNumber)) // Checking Diagonals
            {
                diagonalDistance = Math.Abs(squareLetterIndex - myKing.squareLetterIndex);
                diagonalLetterDistance = squareLetterIndex - myKing.squareLetterIndex; // if this is negative then the king is on our left. positive = right
                diagonalNumberDistance = squareNumber - myKing.squareNumber; // if this is negative then the king is on our down. positive = up
                // If diagonalDistance > 1 then I gotta check my behind.
                // If diagonalDistance == 1 then I gotta check only further away. And look for an enemy or an ally.
                if (diagonalDistance == 1) // I gotta check only further away.
                {
                    for (int i = 1; i < 8; i++)
                    {
                        iX = diagonalLetterDistance > 0 ? i : -i; // This will be used for Square Letters.
                        iY = diagonalNumberDistance > 0 ? i : -i; // This will be used for Square Numbers.
                        if (squareLetterIndex + iX > 7 || squareLetterIndex + iX < 0) break; // Ever seen i4 before? No? Then we break this for loop.
                        if (squareNumber + iY > 8 || squareNumber + iY < 1) break; // Ever seen a0 before? No? Then we break this for loop.
                        pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + iX] + (squareNumber + iY));
                        if (pieceImChecking != null) // If we find a piece on the way
                        {
                            if (pieceImChecking.color != color && (pieceImChecking.type == "Bishop" || pieceImChecking.type == "Queen")) return pieceImChecking; // If its an enemy and it can move diagonal then we are protecting our king.
                            else return null;
                        }
                    }
                }
                else if (squareLetterIndex == 0 || squareLetterIndex == 7 || squareNumber == 1 || squareNumber == 8) // I gotta check only behind.
                {
                    for (int i = 1; i < 8; i++)
                    {
                        iX = diagonalLetterDistance > 0 ? i : -i; // This will be used for Square Letters.
                        iY = diagonalNumberDistance > 0 ? i : -i; // This will be used for Square Numbers.
                        if (myKing.squareLetterIndex + iX > 7 || myKing.squareLetterIndex + iX < 0) break; // Ever seen i4 before? No? Then we break this for loop.
                        if (myKing.squareNumber + iY > 8 || myKing.squareNumber + iY < 1) break; // Ever seen a0 before? No? Then we break this for loop.
                        pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[myKing.squareLetterIndex + iX] + (myKing.squareNumber + iY));
                        if (pieceImChecking != null) // If we find a piece on the way
                        {
                            if (pieceImChecking.color != color && (pieceImChecking.type == "Bishop" || pieceImChecking.type == "Queen")) return pieceImChecking; // If its an enemy and it can move diagonal then we are protecting our king.
                            else return null;
                        }
                    }
                }
                else // I gotta check behind first. Then Further Away
                {
                    for (int i = 1; i < 8; i++) // I check my behind squares first.
                    {
                        iX = diagonalLetterDistance > 0 ? i : -i; // This will be used for Square Letters.
                        iY = diagonalNumberDistance > 0 ? i : -i; // This will be used for Square Numbers.
                        if (myKing.squareLetterIndex + iX > 7 || myKing.squareLetterIndex + iX < 0) break; // Ever seen i4 before? No? Then we break this for loop.
                        if (myKing.squareNumber + iY > 8 || myKing.squareNumber + iY < 1) break; // Ever seen a0 before? No? Then we break this for loop.
                        pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[myKing.squareLetterIndex + iX] + (myKing.squareNumber + iY));
                        if (pieceImChecking != null) // If we find a piece on the way
                        {
                            if (pieceImChecking.color != color && (pieceImChecking.type == "Bishop" || pieceImChecking.type == "Queen")) return pieceImChecking; // If its an enemy and it can move diagonal then we are protecting our king.
                            else if (pieceImChecking.square == square) break; // If its me now we gotta check further away so we get out of this for loop and go to another.
                            else return null; // If its not me, then probably its an ally of ours. Or maybe not enemy bishop or queen. Then Im not protecting our king. Get out of this function.
                        }
                    }
                    for (int i = 1; i < 8; i++) // Nothing was found on the way back to our king. Now I gotta check further away.
                    {
                        iX = diagonalLetterDistance > 0 ? i : -i; // This will be used for Square Letters.
                        iY = diagonalNumberDistance > 0 ? i : -i; // This will be used for Square Numbers.
                        if (squareLetterIndex + iX > 7 || squareLetterIndex + iX < 0) break; // Ever seen i4 before? No? Then we break this for loop.
                        if (squareNumber + iY > 8 || squareNumber + iY < 1) break; // Ever seen a0 before? No? Then we break this for loop.
                        pieceImChecking = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + iX] + (squareNumber + iY));
                        if (pieceImChecking != null) // If we find a piece on the way
                        {
                            if (pieceImChecking.color != color && (pieceImChecking.type == "Bishop" || pieceImChecking.type == "Queen")) return pieceImChecking; // If its an enemy and it can move diagonal then we are protecting our king.
                            else return null;
                        }
                    }
                }
            }
            return null;
        }
        public static Piece IsAnyPieceOnThisSquare(string square) // Returns The Piece On The Square
        {
            foreach (Piece p in Form1.AllPieces)
            {
                if (p.square == square)
                {
                    return p;
                }
            }
            return null;
        }
        public List<string> GetMovesFromGoingRight()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = squareLetterIndex + 1; k < squarePrefix.Length; k++)
            {
                if (k == 8) break; // letterIndex 7 means h squares like h1-h2-...-h8. If we are at h square then dont look any more right. Because h is the rightest.
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[k] + squareNumber);
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[k] + squareNumber);
                    break;
                }
                else _possibleMoves.Add(squarePrefix[k] + squareNumber); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingLeft()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = squareLetterIndex - 1; k > -1; k--)
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
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingUp()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = squareNumber + 1; k < 9; k++)
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
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingDown()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = squareNumber - 1; k > 0; k--)
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
        public List<string> GetMovesFromGoingUpRight()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = 1; k < 9; k++)
            {
                if (squareNumber + k > 8 || squareLetterIndex + k > 7) break;
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + k] + (squareNumber + k));
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[squareLetterIndex + k] + (squareNumber + k));
                    break;
                }
                else _possibleMoves.Add(squarePrefix[squareLetterIndex + k] + (squareNumber + k)); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingUpLeft()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = 1; k < 9; k++)
            {
                if (squareNumber + k > 8 || squareLetterIndex - k < 0) break;
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex - k] + (squareNumber + k));
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[squareLetterIndex - k] + (squareNumber + k));
                    break;
                }
                else _possibleMoves.Add(squarePrefix[squareLetterIndex - k] + (squareNumber + k)); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingDownRight()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = 1; k < 9; k++)
            {
                if (squareNumber - k < 1 || squareLetterIndex + k > 7) break;
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex + k] + (squareNumber - k));
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[squareLetterIndex + k] + (squareNumber - k));
                    break;
                }
                else _possibleMoves.Add(squarePrefix[squareLetterIndex + k] + (squareNumber - k)); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
        public List<string> GetMovesFromGoingDownLeft()
        {
            List<string> _possibleMoves = new List<string>();
            for (int k = 1; k < 9; k++)
            {
                if (squareNumber - k < 1 || squareLetterIndex - k < 0) break;
                Piece PieceOnTheSquare = IsAnyPieceOnThisSquare(squarePrefix[squareLetterIndex - k] + (squareNumber - k));
                if (PieceOnTheSquare != null) // If any Piece is on our way
                {
                    if (PieceOnTheSquare.color != color) _possibleMoves.Add(squarePrefix[squareLetterIndex - k] + (squareNumber - k));
                    break;
                }
                else _possibleMoves.Add(squarePrefix[squareLetterIndex - k] + (squareNumber - k)); // If no piece on our way then its a possible move
            }
            return _possibleMoves;
        }
    }
}