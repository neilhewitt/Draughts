using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draughts.Core;
using System.Threading;

namespace Draughts.ConsoleApp
{
    public class GameRunner : IPlayDraughts
    {
        private Game _game;

        public void Initialise(Game game)
        {
            _game = game;
            Display(_game, _game.CurrentPlayer.BestMove);
        }

        public bool PlayerTakesTurn(Player player)
        {
            Move bestMove = player.BestMove;
            Display(_game, bestMove);
            if (bestMove != null) player.Move(bestMove);
            Thread.Sleep(500);
            return (bestMove != null);
        }

        public void PlayerWins(Player player)
        {
            Display(_game, null);
            Console.WriteLine("\n" + player.Name + " (" + player.Colour + ") WINS!!!\n\nPress any key to play again.");
            Console.Read();
        }

        public void Display(Game game, Move bestMove)
        {
            Console.Clear();
            Console.WriteLine("SuperDraughts (C)2016 Zero Point Systems Ltd");
            Console.WriteLine("--------------------------------------------");
            Console.Write("\n");
            Console.WriteLine(game.CurrentPlayer.Name + " (" + game.CurrentPlayer.Colour.ToString() + ") plays\n");

            BoardState state = game.GetState();
            SquareColour current = SquareColour.Black;
            for (int i = 0; i < 8; i++)
            {
                Console.Write("\t\n\t");
                for (int j = 0; j < 8; j++)
                {
                    if (current == SquareColour.White)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    if (bestMove != null && bestMove.From.Row == i && bestMove.From.Column == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    if (bestMove != null && bestMove.To.Row == i && bestMove.To.Column == j)
                    {
                        Console.BackgroundColor = game.CurrentPlayer.Colour == PieceColour.Black ? ConsoleColor.Red : ConsoleColor.Blue;
                    }

                    PieceLocation piece = state.For(i, j);
                    if (piece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        if (piece.IsCrowned)
                        {
                            Console.Write((piece.Colour == PieceColour.Black ? "b" : "w"));
                        }
                        else
                        {
                            Console.Write((piece.Colour == PieceColour.Black ? "B" : "W"));
                        }
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    current = current == SquareColour.Black ? SquareColour.White : SquareColour.Black;
                }
                current = current == SquareColour.Black ? SquareColour.White : SquareColour.Black;
            }
            Console.Write("\t\n\n");

            Console.WriteLine("\nSHALL WE PLAY A GAME? (Y/N)");
        }
    }
}
