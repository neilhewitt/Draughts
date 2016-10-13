using Draughts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game game = new Game("John", "Jane");
                var piece = game.Board[2, 0].Occupier;
                piece.Crown();
                IEnumerable<Square> squares = piece.GetValidMoves();
                Display(game, squares);
                Console.ReadLine();

                while (true)
                {
                    if (squares.Count() == 0) break;
                    IEnumerable<Piece> piecesTaken = null;
                    piece.MoveTo(squares.First(), out piecesTaken);
                    squares = piece.GetValidMoves();
                    if (squares.Count() == 0) break;
                    Display(game, squares);
                    Console.ReadLine();
                }

                Display(game);
                Console.ReadLine();
            }
        }

        public static void Display(Game game, IEnumerable<Square> overlay = null)
        {
            Console.Clear();
            Console.WriteLine("SuperDraughts (C)2016 Zero Point Systems Ltd");
            Console.WriteLine("--------------------------------------------");
            Console.Write("\n");
            Console.WriteLine(game.BlackPlayer.Name + " plays Black\n");

            for (int i = 0; i < 8; i++)
            {
                Console.Write("\t\n\t");
                for (int j = 0; j < 8; j++)
                {
                    Square square = game.Board[i, j];
                    if (square.Colour == SquareColour.White)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    if (overlay != null && overlay.Contains(square))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }

                    Console.Write((square.Occupier?.Colour == PieceColour.Black ? "B" : square.Occupier == null ? " " : "W"));
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            Console.Write("\t\n\n");

            Console.WriteLine("\nSHALL WE PLAY A GAME? (Y/N)");
        }
    }
}
