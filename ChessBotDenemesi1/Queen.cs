using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotDenemesi1
{
    class Queen: Piece
    {
        public Queen() { }
        public Queen(string square, string color) : base(square, "Queen", color)
        {
            possibleMoves = GetPossibleMoves();
        }

        public override List<string> GetPossibleMoves()
        {
            List<string> a = new List<string>();
            a.Add("i9");
            return a;
            return null;
        }
    }
}
