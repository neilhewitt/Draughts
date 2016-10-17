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
                    if (tree.Edges.Count() > 0)
                    {
                        Dictionary<Move, int> movesByPiecesTaken = new Dictionary<Move, int>(); // moves are unique, count may not be, hence swapping key / value
                        foreach (MoveNode edge in tree.Edges)
                        {
                            if (edge.Square.IsEmpty)
                            {
                                Move move = new Move(edge.Root.Square, edge.Square);
                                moves.Add(move);
                            }
                        }
                    }
                }
                
                return moves;
            }
        }

        public Move BestMove
        {
            get
            {
                Dictionary<Move, int> movesByPiecesTaken = new Dictionary<Move, int>(); // moves are unique, count may not be, hence backwards

                foreach (Piece piece in _game.Board.Squares.Where(s => s.IsOccupied && s.Occupier.Owner == this).Select(s => s.Occupier))
                {
                    MoveTree tree = piece.GetMoveTree();
                    if (tree.Edges.Count() > 0)
                    {
                        foreach (MoveNode edge in tree.Edges)
                        {
                            if (edge.Square.IsEmpty)
                            {
                                Move move = new Move(edge.Root.Square, edge.Square);
                                int count = 0;
                                MoveNode node = edge.Parent;
                                MoveNode root = edge.Root;
                                while (node != root)
                                {
                                    count++;
                                    node = node.Parent;
                                }
                                movesByPiecesTaken.Add(move, count);
                            }
                        }
                    }
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
            if (move.From.Row < 0 || move.From.Column < 0 || move.From.Row > 7 || move.From.Column > 7) return false;
            if (move.To.Row < 0 || move.To.Row < 0 || move.To.Row > 7 || move.To.Row > 7) return false;

            IEnumerable<Move> moves = ValidMoves;
            
            if (moves.Any(m => m.From.Row == move.From.Row && m.From.Column == move.From.Column && m.To.Row == move.To.Row && m.To.Column == move.To.Column))
            {
                Square origin = _game.Board[move.From.Row, move.From.Column];
                Square destination = _game.Board[move.To.Row, move.To.Column];
                origin.Occupier.MoveTo(destination);
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
