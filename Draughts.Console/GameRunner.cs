using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draughts.Core;
using System.Threading;

namespace Draughts.ConsoleApp
{
    public class ConsoleGameAgent : IPlayDraughts
    {
        private Game _game;

        public void Initialise(Game game)
        {
            _game = game;
        }

        public Move PlayerTakesTurn(IEnumerable<Move> moves, Move bestMove)
        {
            Display(_game, bestMove, false);
            for (int i = 0; i < 3; i++)
            {
                Display(_game, null, false);
                //Thread.Sleep(150);
                Display(_game, bestMove, false);
                //Thread.Sleep(150);
            }

            //Thread.Sleep(1000);

            if (bestMove != null)
            {
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
            //Console.Read();
        }

        public void Display(Game game, Move bestMove = null, bool clearFirst = true, IEnumerable<Move> allMoves = null)
        {
            if (clearFirst)
            {
                Console.Clear();
            }
            else
            {
                Console.SetCursorPosition(0, 0);
            }

            Console.WriteLine("SuperDraughts (C)2016 Zero Point Systems Ltd");
            Console.WriteLine("--------------------------------------------");
            Console.Write("\n");
            Console.WriteLine(game.CurrentPlayer.Name + " (" + game.CurrentPlayer.Colour.ToString() + ") plays\n");

            PieceState state = game.GetState();
            SquareColour current = SquareColour.Yellow;
            Console.Write("\t 01234567");
            for (int i = 0; i < 8; i++)
            {
                Console.Write("\t\n\t" + i.ToString());
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

                    if (bestMove != null && bestMove.Start.Row == i && bestMove.Start.Column == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    if ((bestMove != null && (bestMove.Nodes.Any(x => x.Row == i && x.Column == j)) || (allMoves != null && allMoves.Any(m => m.Nodes.Any(x => x.Row == i && x.Column == j)))))
                    {
                        Console.BackgroundColor = game.CurrentPlayer.Colour == PieceColour.Black ? ConsoleColor.Red : ConsoleColor.Blue;
                    }

                    PieceInfo piece = state.For(i, j);
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
