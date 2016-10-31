using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draughts.Core;
using System.Threading;
using System.IO;

namespace Draughts.ConsoleApp
{
    public class TestUI
    {
        private string _logPath;
        private List<string> _logEntries;
        int _movesSinceLastPieceTaken = 0;
        bool _flaggedStall = false;

        public void PreviewTurn(object sender, BeforeMoveEventArgs args)
        {
            Display(args.BoardState, args.Player, args.BestMove, false);
            Console.SetCursorPosition(0, 17);
            Move move = args.BestMove;
            ConsoleWriteLineAndLog(args.Player.Colour.ToString() + " says: my move will be (" + move.Start.Row + ", " + move.Start.Column + ") to (" + move.End.Row + ", " + move.End.Column + ") taking " 
                + move.PiecesTaken + " pieces           ");
            //if (move.PiecesTaken > 0)
            //Thread.Sleep(300);
            //else 
            //Console.ReadKey();
        }

        public void PlayerTakesTurn(object sender, MoveEventArgs args)
        {
            Display(args.BoardState, args.Player, args.Move, false);
            string state = "";
            int row = 0;
            foreach(SquareInfo info in args.BoardState.Squares)
            {
                if (info.Row > row)
                {
                    row = info.Row;
                    state += "|";
                }

                state += info.PieceInfo != null ? info.PieceInfo.Colour == PieceColour.Black ? (info.PieceInfo.IsCrowned ? "b" : "B") : (info.PieceInfo.IsCrowned ? "w" : "W") : "0";
            }
            Log("State: " + state);

            if (args.Move.PiecesTaken > 0)
            {
                Log("Black has " + args.BoardState.BlackPiecesRemaining + " pieces remaining, White has " + args.BoardState.WhitePiecesRemaining + " pieces remaining.");
                _movesSinceLastPieceTaken = 0;
                _flaggedStall = false;
            }
            else
            {
                _movesSinceLastPieceTaken++;
            }

            if (_movesSinceLastPieceTaken > 10 && !_flaggedStall)
            {
                Log("- STALLED");
                _flaggedStall = true;
            }
        }

        public void GameEnds(object sender, GameEndsEventArgs args)
        {
            Display(args.BoardState, args.Winner, null);
            if (args.ReasonPlayerWon == ReasonsForWinning.CantMove) Console.WriteLine(args.Winner.Opponent.Name + " (" + args.Winner.Opponent.Colour + ") cannot play, and loses the game.\n");
            //Console.WriteLine("\n" + args.Winner.Name + " (" + args.Winner.Colour + ") WINS!!!\n\nPress any key to play again.");
            //Console.Read();
            Log("---- GAME ENDS " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " ----");
            FlushLog();
        }

        public void Display(BoardState state, Player player, Move bestMove = null, bool clearFirst = true, IEnumerable<Move> allMoves = null)
        {
            if (allMoves != null)
            {
                string log = "Valid moves are: ";
                foreach (Move move in allMoves)
                {
                    log += "(" + move.Start.Row + ", " + move.Start.Column + ") : (" + move.End.Row + ", " + move.End.Column + "); ";
                }
                Log(log);

                if (bestMove != null) Log("Best move is: (" + bestMove.Start.Row + ", " + bestMove.Start.Column + ") : (" + bestMove.End.Row + ", " + bestMove.End.Column + ")");
            }

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
            Console.Write(player.Name + " (" + player.Colour.ToString() + ") plays                     \n\n");

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

                    PieceInfo piece = state.For(i, j).PieceInfo;
                    if (piece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        if (piece.IsCrowned)
                        {
                            Console.ForegroundColor = piece.Colour == PieceColour.Black ? ConsoleColor.Black : ConsoleColor.White;
                            Console.Write("X");
                        }
                        else
                        {
                            Console.ForegroundColor = piece.Colour == PieceColour.Black ? ConsoleColor.Black : ConsoleColor.White;
                            Console.Write("O");
                        }
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    current = current == SquareColour.Yellow ? SquareColour.White : SquareColour.Yellow;
                }
                current = current == SquareColour.Yellow ? SquareColour.White : SquareColour.Yellow;
            }
            Console.Write("\t\n\n");
            Console.Write("Black has " + state.BlackPiecesRemaining + " pieces remaining, White has " + state.WhitePiecesRemaining + " pieces remaining.    ");
        }

        private void Log(string message)
        {
            _logEntries.Add(message);
            if (_logEntries.Count() > 20) FlushLog();
        }

        private void ConsoleWriteLineAndLog(string message)
        {
            Console.WriteLine(message);
            Log(message.Replace("\t", "").Replace("\n", ""));
        }

        private void FlushLog()
        {
            //File.AppendAllLines(_logPath, _logEntries);
            _logEntries.Clear();
        }

        public TestUI(Game game, string logPath)
        {
            _logPath = logPath;
            _logEntries = new List<string>();
            game.BeforePlayerMoves += PreviewTurn;
            game.PlayerMoves += PlayerTakesTurn;
            game.GameEnds += GameEnds;

            Log("---- GAME BEGINS " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " ----");
            Log(game.BlackPlayer.Name + " plays Black");
            Log(game.WhitePlayer.Name + " plays White");
        }
    }
}
