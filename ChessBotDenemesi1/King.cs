﻿using System;
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
            possibleMoves = GetPossibleMoves();
        }

        public override List<string> GetPossibleMoves()
        {
            return null;
        }
    }
}