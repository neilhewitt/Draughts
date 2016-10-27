using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draughts.Core;
using System.Threading;

namespace Draughts.ConsoleApp
{
    public class TestUI
    {
        public void PreviewTurn(object sender, BeforeMoveEventArgs args)
        {
            Display(args.BoardState, args.Player, args.BestMove, false);
            Console.SetCursorPosition(0, 17);
            Move move = args.BestMove;
            Console.WriteLine(args.Player.Name + " says: my move will be (" + move.Start.Row + ", " + move.Start.Column + ") to (" + move.End.Row + ", " + move.End.Column + ") taking " 
                + move.PiecesTaken + " pieces           ");
            Thread.Sleep(300);
        }

        public void PlayerTakesTurn(object sender, MoveEventArgs args)
        {
            Display(args.BoardState, args.Player, args.Move, false);
        }

        public void GameEnds(object sender, GameEndsEventArgs args)
        {
            Display(args.BoardState, args.Winner, null);
            if (args.ReasonPlayerWon == ReasonsForWinning.CantMove) Console.WriteLine(args.Winner.Opponent.Name + " (" + args.Winner.Opponent.Colour + ") cannot play, and loses the game.\n");
            //Console.WriteLine("\n" + args.Winner.Name + " (" + args.Winner.Colour + ") WINS!!!\n\nPress any key to play again.");
            //Console.Read();
        }

        public void Display(BoardState state, Player player, Move bestMove = null, bool clearFirst = true, IEnumerable<Move> allMoves = null)
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
            Console.WriteLine(player.Name + " (" + player.Colour.ToString() + ") plays                     \n");

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

                    if ((bestMove != null && (bestMove.Steps.Any(x => x.Row == i && x.Column == j)) || (allMoves != null && allMoves.Any(m => m.Steps.Any(x => x.Row == i && x.Column == j)))))
                    {
                        Console.BackgroundColor = player.Colour == PieceColour.Black ? ConsoleColor.Red : ConsoleColor.Blue;
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
            Console.WriteLine("Black has " + state.BlackPieces.Count() + " pieces remaining, White has " + state.WhitePieces.Count() + " pieces remaining.");
        }

        public TestUI(Game game)
        {
            game.BeforePlayerMoves += PreviewTurn;
            game.PlayerMoves += PlayerTakesTurn;
            game.GameEnds += GameEnds;
        }
    }
}
