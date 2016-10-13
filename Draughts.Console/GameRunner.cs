using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draughts.Core;

namespace Draughts.ConsoleApp
{
    public class GameRunner : IPlayDraughts
    {
        private Game _game;
        private Move _lastMove;

        public void Initialise(Game game)
        {
            _game = game;
            Display(_game, _game.CurrentPlayer.ValidMoves, null);
        }

        public void PlayerTakesTurn(Player player)
        {
            IEnumerable<Move> moves = player.ValidMoves;
            Display(_game, moves);
            Console.WriteLine("Press ENTER to move.");
            Console.ReadLine();
            _lastMove = player.ValidMoves.First();
            player.MovesPiece(_lastMove);
        }

        public void PlayerWins(Player player)
        {
            throw new NotImplementedException();
        }

        public void Display(Game game, IEnumerable<Move> validMoves, Move lastMove = null)
        {
            Console.Clear();
            Console.WriteLine("SuperDraughts (C)2016 Zero Point Systems Ltd");
            Console.WriteLine("--------------------------------------------");
            Console.Write("\n");
            Console.WriteLine(game.BlackPlayer.Name + " plays Black\n");

            BoardState state = game.GetState();
            IEnumerable<PieceLocation> playerState = game.CurrentPlayer == game.BlackPlayer ? state.BlackPieces : state.WhitePieces;
            PieceLocation firstValidPiece = playerState.FirstOrDefault(x => validMoves.Any(m => m.From.Row == x.Location.Row && m.From.Column == x.Location.Column));
            if (firstValidPiece != null)
            {
                validMoves = validMoves.Where(x => x.From.Row == firstValidPiece.Location.Row && x.From.Column == firstValidPiece.Location.Column);
            }
            else
            {
                validMoves = null;
            }

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

                    if (firstValidPiece.Location.Row == i && firstValidPiece.Location.Column == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    if (lastMove != null && lastMove.To.Row == i && lastMove.To.Column == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    if (lastMove != null && lastMove.From.Row == i && lastMove.From.Column == j)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }

                    if (validMoves != null && validMoves.Any(x => x.To.Row == i && x.To.Column == j))
                    {
                        Console.BackgroundColor = game.CurrentPlayer.Colour == PieceColour.Black ? ConsoleColor.Gray : ConsoleColor.Blue;
                    }

                    PieceLocation piece = state.For(i, j);
                    if (piece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write((piece.Colour == PieceColour.Black ? "B" : "W"));
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
