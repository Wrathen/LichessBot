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
            possibleMoves = GetPossibleMoves();
        }

        public override List<string> GetPossibleMoves()
        {
            return null;
        }
    }
}
