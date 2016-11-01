using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Player
    {
        private static Random _random = new Random(DateTime.Now.Millisecond);

        private Game _game;
        private Func<IEnumerable<Move>, Move, Move> _selectMoveCallback;

        public string Name { get; private set; }
        public PieceColour Colour { get; }
        public bool IsComputerPlayer { get; private set; }
        public Player Opponent => Colour == PieceColour.Black ? _game.WhitePlayer : _game.BlackPlayer;

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == this);
        public int PiecesCaptured { get; private set; }

        internal Func<IEnumerable<Move>, Move, Move> SelectMoveCallback => _selectMoveCallback;

        internal IEnumerable<Move> GetAvailableMoves(out Move bestMove, bool useRandomBestMove = false)
        {
            IEnumerable<Move> validMoves = _game.Board.ValidMovesFor(this);
            // we can use either random best move (pick any from valid moves) or MiniMax (AI play-ahead up to n generations)
            bestMove = useRandomBestMove ? RandomBestMove(validMoves) : MiniMaxBestMove(validMoves, 6);
            {
                if (bestMove != null && bestMove.PiecesTaken > 0)
                {
                    // we *have* to take this move
                    return new List<Move>() { bestMove };
                }
                else
                {
                    return validMoves;
                }
            }
        }

        internal void Move(Move move)
        {
            Square origin = _game.Board[move.Start.Row, move.Start.Column];
            origin.Occupier.Move(move);
        }

        internal void CapturedAPiece()
        {
            PiecesCaptured++;
        }

        internal void BecomeHuman(string name, Func<IEnumerable<Move>, Move, Move> selectMoveCallback)
        {
            Name = name;
            IsComputerPlayer = false;
            _selectMoveCallback = selectMoveCallback;
        }

        private Move MiniMaxBestMove(IEnumerable<Move> validMoves, int maxMovesAhead)
        {
            return new MiniMax(_game, this).FindBestMove(validMoves, maxMovesAhead);
        }

        private Move RandomBestMove(IEnumerable<Move> validMoves)
        {
            Move bestMove = validMoves.RandomFirstOrDefault();
            return bestMove;
        }

        public Player(Game game, PieceColour colour)
        {
            Name = "Computer";
            _game = game;
            Colour = colour;
            IsComputerPlayer = true;
            _selectMoveCallback = (moves, move) => move;
        }
    }
}
