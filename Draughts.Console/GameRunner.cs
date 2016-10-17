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

        public Move PlayerTakesTurn(Player player)
        {
            Move bestMove = player.BestMove;
            Display(_game, bestMove);
            if (bestMove != null)
            {
                Thread.Sleep(500);
                return bestMove;
            }
            else
            {
                return null;
            }
        }

        public void PlayerWins(Player player, Player opponent, ReasonsForLosing reason)
        {
            Display(_game, null);
            if (reason == ReasonsForLosing.CantMove) Console.WriteLine(opponent.Name + " (" + opponent.Colour + ") cannot play, and loses the game.\n");
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
            SquareColour current = SquareColour.Yellow;
            for (int i = 0; i < 8; i++)
            {
                Console.Write("\t\n\t");
                for (int j = 0; j < 8; j++)
                {
                    if (current == SquareColour.White)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }

                    if (bestMove != null && bestMove.FromRow == i && bestMove.FromColumn == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    if (bestMove != null && bestMove.FromRow == i && bestMove.ToColumn == j)
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
                            Console.ForegroundColor = piece.Colour == PieceColour.Black ? ConsoleColor.Black : ConsoleColor.White;
                            Console.Write((piece.Colour == PieceColour.Black ? "b" : "w"));
                        }
                        else
                        {
                            Console.ForegroundColor = piece.Colour == PieceColour.Black ? ConsoleColor.Black : ConsoleColor.White;
                            Console.Write((piece.Colour == PieceColour.Black ? "B" : "W"));
                        }
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    current = current == SquareColour.Yellow ? SquareColour.White : SquareColour.Yellow;
                }
                current = current == SquareColour.Yellow ? SquareColour.White : SquareColour.Yellow;
            }
            Console.Write("\t\n\n");
            Console.WriteLine("Black has " + game.BlackPlayer.PiecesRemaining + " pieces remaining, White has " + game.WhitePlayer.PiecesRemaining + " pieces remaining.");
        }

        public void ComputerPlayerTakesTurn(Move move)
        {
            throw new NotImplementedException();
        }
    }
}
