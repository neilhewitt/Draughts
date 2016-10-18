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

        public string Name { get; }
        public PieceColour Colour { get; }
        public bool IsComputerPlayer { get; }

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.IsOccupied && s.Occupier.Owner == this);
        public int PiecesTaken { get; private set; }

        public IEnumerable<Move> ValidMoves
        {
            get
            {
                List<Move> moves = new List<Move>();

                foreach (Piece piece in _game.Board.Squares.Where(s => s.IsOccupied && s.Occupier.Owner == this).Select(s => s.Occupier))
                {
                    MoveTree tree = piece.GetMoveTree();
                    moves.AddRange(tree.Moves);
                }
                
                return moves;
            }
        }

        public Move BestMove
        {
            get
            {
                Dictionary<Move, int> movesByPiecesTaken = new Dictionary<Move, int>(); // moves are unique, count may not be, hence backwards
                foreach(Move move in ValidMoves)
                {
                    movesByPiecesTaken.Add(move, move.PiecesTaken);
                }

                if (movesByPiecesTaken.Count > 0)
                {
                    IEnumerable<Move> bestMoves = movesByPiecesTaken.Where(x => x.Value == movesByPiecesTaken.Values.Max()).Select(x => x.Key);
                    if (bestMoves.Count() == 1)
                    {
                        return bestMoves.First();
                    }
                    else
                    {
                        return bestMoves.Skip(_random.Next(bestMoves.Count() - 1)).First();
                    }
                }

                return null;
            }
        }

        internal bool Move(Move move)
        {
            if (move.Start.Row < 0 || move.Start.Column < 0 || move.Start.Row > 7 || move.Start.Column > 7) return false;
            if (move.End.Row < 0 || move.End.Row < 0 || move.End.Row > 7 || move.End.Row > 7) return false;
            
            if (ValidMoves.Any(m => m.Start.Row == move.Start.Row && m.Start.Column == move.Start.Column && m.End.Row == move.End.Row && m.End.Column == move.End.Column))
            {
                Square origin = _game.Board[move.Start.Row, move.Start.Column];
                origin.Occupier.Move(move);
                return true;
            }

            return false;
        }

        internal void TakePiece()
        {
            PiecesTaken++;
        }

        public Player(string name, Game game, PieceColour colour, bool isComputerPlayer)
        {
            Name = name;
            _game = game;
            Colour = colour;
            IsComputerPlayer = isComputerPlayer;
        }
    }
}
