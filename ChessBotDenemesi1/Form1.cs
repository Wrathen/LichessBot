using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace ChessBotDenemesi1
{
    public partial class Form1 : Form
    {
        /* To Do List
        ---> Açmaz ve Piecelerin Check halindeyken hareketleri
        */

        // Mouse Click Simulations
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        // Bitmap Assignments
        Bitmap Siyah_Piyon = new Bitmap("D:\\Images\\Siyah_Piyon.png"); // Siyah = Black, Beyaz = White
        Bitmap Siyah_Kale = new Bitmap("D:\\Images\\Siyah_Kale.png");
        Bitmap Siyah_At = new Bitmap("D:\\Images\\Siyah_At.png"); // Piyon = Pawn, Kale = Rook
        Bitmap Siyah_Fil = new Bitmap("D:\\Images\\Siyah_Fil.png"); // At = Knight, Fil = Bishop
        Bitmap Siyah_Vezir = new Bitmap("D:\\Images\\Siyah_Vezir.png"); // Vezir = Queen, Sah = King
        Bitmap Siyah_Sah = new Bitmap("D:\\Images\\Siyah_Sah.png");

        Bitmap Beyaz_Piyon = new Bitmap("D:\\Images\\Beyaz_Piyon.png");
        Bitmap Beyaz_Kale = new Bitmap("D:\\Images\\Beyaz_Kale.png");
        Bitmap Beyaz_At = new Bitmap("D:\\Images\\Beyaz_At.png");
        Bitmap Beyaz_Fil = new Bitmap("D:\\Images\\Beyaz_Fil.png");
        Bitmap Beyaz_Vezir = new Bitmap("D:\\Images\\Beyaz_Vezir.png");
        Bitmap Beyaz_Sah = new Bitmap("D:\\Images\\Beyaz_Sah.png");

        // Which Perspective will we use to determine squares' name. If white 0,0 is a1. If black 0,0 is h8.
        string Perspective = "White";

        // Who's Turn? 1=White, 0=Black
        bool Turn = true;

        // Board Positions
        int boardPositionX0 = 0;
        int boardPositionY0 = 0;
        int boardPositionX1 = 0;
        int boardPositionY1 = 0;

        // Board Terms
        string[] prefix_SquareString = new string[] { "a", "b", "c", "d", "e", "f", "g", "h" };
        string[] prefix_SquareNumber = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
        string[] prefix_PieceTypes = new string[] { "", "R", "N", "B", "K", "Q" };
        static string LastMove = "";

        // Special Coordinates To Optimize Speed of Getting Chess Board Details/Pieces...
        int[,] specialCoordinates = new int[,] { { 39, 27 }, { 16, 26 }, { 34, 27 }, { 18, 31 }, { 39, 31 }, { 40, 51 } }; // To Eliminate Some of the Possibilities. For Speed/Optimisation

        // List Of All Pieces on The Board
        public static List<Piece> AllPieces = new List<Piece>();
        public static List<Piece> WhitePieces = new List<Piece>();
        public static List<Piece> BlackPieces = new List<Piece>();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1;
            timer1.Enabled = true;
        }   
        void GetChessBoardDetails(Bitmap toBeSeeked)
        {
            Piece piece = null;
            string SquareOfPiece = "";
            string PieceColor = "";
            for(int katx = 0; katx < 8; katx++)
            {
                for(int katy = 0; katy < 8; katy++)
                {
                    int reverseKatY = Math.Abs(katy - 7); // This is for start checking from left-bottom instead of left-top

                    for (int tries = 0; tries < 6; tries++)
                    {
                        SquareOfPiece = KatToSquare(katx, katy, Perspective);
                        if (ColorsMatch(toBeSeeked.GetPixel(specialCoordinates[tries,0] + (katx*64), specialCoordinates[tries, 1] + (reverseKatY * 64)), Color.FromArgb(255,255,255,255)))
                        {
                            PieceColor = "White";
                            switch (tries)
                            {
                                case 0:
                                    if (PiecesMatch(toBeSeeked, Beyaz_Fil, katx, reverseKatY))
                                    {
                                        piece = new Bishop(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 1:
                                    if (PiecesMatch(toBeSeeked, Beyaz_At, katx, reverseKatY))
                                    {
                                        piece = new Knight(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 2:
                                    if (PiecesMatch(toBeSeeked, Beyaz_Kale, katx, reverseKatY))
                                    {
                                        piece = new Rook(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 3:
                                    if (PiecesMatch(toBeSeeked, Beyaz_Sah, katx, reverseKatY))
                                    {
                                        piece = new King(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 4:
                                    if (PiecesMatch(toBeSeeked, Beyaz_Vezir, katx, reverseKatY))
                                    {
                                        piece = new Queen(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 5:
                                    if (PiecesMatch(toBeSeeked, Beyaz_Piyon, katx, reverseKatY))
                                    {
                                        piece = new Pawn(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                            }
                            WhitePieces.Add(piece);
                            AllPieces.Add(piece);
                        }
                        else if (ColorsMatch(toBeSeeked.GetPixel(specialCoordinates[tries, 0] + (katx*64), specialCoordinates[tries, 1] + (reverseKatY * 64)), Color.FromArgb(255, 0, 0, 0)))
                        {
                            PieceColor = "Black";
                            switch (tries)
                            {
                                case 0:
                                    if (PiecesMatch(toBeSeeked, Siyah_Fil, katx, reverseKatY))
                                    {
                                        piece = new Bishop(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 1:
                                    if (PiecesMatch(toBeSeeked, Siyah_At, katx, reverseKatY))
                                    {
                                        piece = new Knight(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 2:
                                    if (PiecesMatch(toBeSeeked, Siyah_Kale, katx, reverseKatY))
                                    {
                                        piece = new Rook(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 3:
                                    if (PiecesMatch(toBeSeeked, Siyah_Sah, katx, reverseKatY))
                                    {
                                        piece = new King(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 4:
                                    if (PiecesMatch(toBeSeeked, Siyah_Vezir, katx, reverseKatY))
                                    {
                                        piece = new Queen(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                                case 5:
                                    if (PiecesMatch(toBeSeeked, Siyah_Piyon, katx, reverseKatY))
                                    {
                                        piece = new Pawn(SquareOfPiece, PieceColor);
                                        tries = 6; // To End the For Loop of Tries.
                                    }
                                    break;
                            }
                            BlackPieces.Add(piece);
                            AllPieces.Add(piece);
                        }
                    }
                }
            }
        } 
        string GetLastMove(Bitmap currentBoard)
        {
            /*
             if(!anyPieceFound) // If we failed to find any piece on that specific tile, we determine what color it is.
                    {
                        Color temp2 = toBeSeeked.GetPixel(7 + (katx * 64), 18 + (reverseKatY * 64));
                        if (ColorsMatch(temp2, Color.FromArgb(255, 181, 136, 99))) listBox2.Items.Add(katx + "," + katy + " katında " + " Siyah0 Kare Bulundu!");
                        else if (ColorsMatch(temp2, Color.FromArgb(255, 170, 162, 58))) listBox2.Items.Add(katx + "," + katy + " katında " + " Siyah1 Kare Bulundu!");
                        else if (ColorsMatch(temp2, Color.FromArgb(255, 240, 217, 181))) listBox2.Items.Add(katx + "," + katy + " katında " + " Beyaz0 Kare Bulundu!");
                        else if (ColorsMatch(temp2, Color.FromArgb(255, 205, 210, 106))) listBox2.Items.Add(katx + "," + katy + " katında " + " Beyaz1 Kare Bulundu!");
                    }
            */
            string lastMove1 = "-"; // We Have 2 of these because when a piece moves there are 2 squares getting colored on lichess
            string lastMove2 = "-"; // They get some greenish color and we will track them.
            Color temp;
            for (int x = 0; x < 8; x++)
            {
                for(int y = 7; y > 0; y--)
                {
                    temp = currentBoard.GetPixel(7 + (x * 64), 18 + (y * 64));
                    if(ColorsMatch(temp,Color.FromArgb(255,170,162,58)) || ColorsMatch(temp,Color.FromArgb(255,205,210,106)))
                    {
                        if (lastMove1 == "-") lastMove1 = KatToSquare(x, y, Perspective); // Translate Kats to Square
                        else if (lastMove2 == "-") lastMove2 = KatToSquare(x, y, Perspective); // We Fill Both Variables with Values
                    }
                }
            }
            // Now We are going to check which of these lastMoves was the mover instead of being the moved square
            foreach(Piece p in AllPieces)
            {
                if(lastMove1 == p.square)
                {
                    if ((Turn && p.color == "White") || (!Turn && p.color == "Black")) return lastMove1;
                }
                else if(lastMove2 == p.square)
                {
                    if ((Turn && p.color == "White") || (!Turn && p.color == "Black")) return lastMove2;
                }
            }
            return null;
        }
        public void MakeMove(string sourceSquare, string targetSquare, string move)
        {
            if (move != "")
            {
                string prefix = (move.Length > 2) ? move.Substring(move.Length - 3, 1) : ""; // It Either give us R or nothing if Pawn was played
                if (prefix_PieceTypes.Contains(prefix))
                {
                    List<Piece> possiblePieces = new List<Piece>();
                    foreach (Piece p in AllPieces)
                    {
                        if (p.type.Substring(0, 1) == prefix || (prefix == "" && p.type.Substring(0, 1) == "P")) possiblePieces.Add(p);
                    }
                    foreach (Piece p in possiblePieces)
                    {
                        Cursor.Position = SquareToPoint(p.square);
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                        Cursor.Position = SquareToPoint(move.Substring(move.Length - 2, 2));
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                    }
                }
            }
            else
            {
                Cursor.Position = SquareToPoint(sourceSquare);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Cursor.Position = SquareToPoint(targetSquare);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            LastMove = GetLastMove(GetChessBoard(boardPositionX0,boardPositionX1,boardPositionY0,boardPositionY1));
            timer_checkboard.Interval = Convert.ToInt32(textBox8.Text);
            timer_checkboard.Enabled = true;
        }
        // Graphics Functions
        Bitmap GetChessBoard(int x0, int x1, int y0, int y1)
        {
            Rectangle rect = new Rectangle(x0, y0, x1 - x0, y1 - y0);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            bmp.Save(@"D:\\Images\\printscreen.jpg", ImageFormat.Jpeg);
            return bmp;
        }
        public bool PiecesMatch(Bitmap toBeSeeked, Bitmap piece, int katx, int katy)
        {
            float tileColorCount = 0;
            float matchedPieceColorCount = 0;
            float requiredPercentage = 70f;
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    Color temp1 = toBeSeeked.GetPixel(x + (katx * 64), y + (katy * 64));
                    if (!ColorsMatch(temp1, Color.FromArgb(255, 181, 136, 99)) &&
                        !ColorsMatch(temp1, Color.FromArgb(255, 170, 162, 58)) &&
                        !ColorsMatch(temp1, Color.FromArgb(255, 240, 217, 181)) &&
                        !ColorsMatch(temp1, Color.FromArgb(255, 205, 210, 106)))
                    {
                        if (ColorsMatch(temp1, piece.GetPixel(x, y))) matchedPieceColorCount++;
                    }
                    else tileColorCount++;
                }
            }
            if (matchedPieceColorCount / (4096f - tileColorCount) > requiredPercentage / 100) return true;
            return false;
        }
        public bool BitmapsMatch(Bitmap b1, Bitmap b2)
        {
            int Width = b1.Width;
            int Height = b1.Height;
            if (b1.Width > b2.Width) Width = b2.Width;
            if (b1.Height > b2.Height) Height = b2.Height;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!ColorsMatch(b1.GetPixel(x, y), b2.GetPixel(x, y))) return false;
                }
            }
            return true;
        }
        public bool ColorsMatch(Color c1, Color c2)
        {
            if (Math.Abs(c1.A - c2.A) > 25) return false;
            if (Math.Abs(c1.R - c2.R) > 25) return false;
            if (Math.Abs(c1.G - c2.G) > 25) return false;
            if (Math.Abs(c1.B - c2.B) > 25) return false;
            return true;
        }
        // Convertion Functions
        public string KatToSquare(int katx, int katy, string color)
        {
            string square = "";

            if (color == "White")
            {
                square += prefix_SquareString[katx];
                square += prefix_SquareNumber[katy];
            }
            else
            {
                square += prefix_SquareString[Math.Abs(katx - 7)];
                square += prefix_SquareNumber[Math.Abs(katy - 7)];
            }
            return square;
        } // Converts Kat to Squares (example: [0,0,white] to a1 <---OR---> [0,0,black] to h8)
        public Point SquareToPoint(string square)
        {
            int x = 0;
            int y = 0;

            for (int a = 0; a < 8; a++)
            {
                if (square.Substring(0, 1) == prefix_SquareString[a]) x = boardPositionX0 + (a * 64) + 32;
                continue;
            }
            for (int b = 0; b < 8; b++)
            {
                if (square.Substring(1, 1) == prefix_SquareNumber[b]) y = boardPositionY1 - (b * 64) - 32;
                continue;
            }

            return new Point(x, y);
        } // Converts e4 to for example 604px,729px Coordinates.
        // Form Elements' Functions
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = MousePosition.X + "," + MousePosition.Y;
            /*Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(MousePosition.X, MousePosition.Y, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            if (Control.ModifierKeys == Keys.Shift) label8.Text = bmp.GetPixel(0, 0).ToString();*/
        } // Utility Stuff
        private void timer_checkboard_Tick(object sender, EventArgs e)
        {
            if(LastMove != GetLastMove(GetChessBoard(boardPositionX0,boardPositionX1,boardPositionY0,boardPositionY1)))
            {
                MessageBox.Show("Sıra Sende!");
            }
        } // Check If Move Was Made
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap chessBoard = GetChessBoard(boardPositionX0, boardPositionX1, boardPositionY0, boardPositionY1);
            Bitmap theImageWeJustGot = new Bitmap("D:\\Images\\printscreen.jpg");
            GetChessBoardDetails(theImageWeJustGot);
            theImageWeJustGot.Dispose();
        } // Scan The Area Button
        private void button2_Click(object sender, EventArgs e)
        {
            boardPositionX0 = Convert.ToInt32(textBox1.Text);
            boardPositionY0 = Convert.ToInt32(textBox2.Text);
            boardPositionX1 = Convert.ToInt32(textBox3.Text);
            boardPositionY1 = Convert.ToInt32(textBox4.Text);
            button1.Enabled = true;
            button3.Enabled = true;
        } // Set The Area Button
        private void button3_Click(object sender, EventArgs e)
        {
            MakeMove(textBox5.Text, textBox6.Text, textBox7.Text);
        } // Make Move Button   
    }
}
