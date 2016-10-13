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
        private Move _bestMove;

        public string Name { get; }
        public PieceColour Colour { get; }

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.Occupier != null && s.Occupier.Owner == this);
        public int PiecesTaken { get; private set; }

        public IEnumerable<Move> ValidMoves
        {
            get
            {
                List<Move> moves = new List<Move>();

                foreach (Piece piece in _game.Board.Squares.Where(s => s.Occupier != null && s.Occupier.Owner == this).Select(s => s.Occupier))
                {
                    MoveMap map = piece.GetMoveMap();
                    if (map.Edges.Count() > 0)
                    {
                        Dictionary<Move, int> movesByPiecesTaken = new Dictionary<Move, int>(); // moves are unique, count may not be, hence backwards
                        foreach (SequenceNode edge in map.Edges)
                        {
                            if (edge.Square.Occupier == null)
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

                foreach (Piece piece in _game.Board.Squares.Where(s => s.Occupier != null && s.Occupier.Owner == this).Select(s => s.Occupier))
                {
                    MoveMap map = piece.GetMoveMap();
                    if (map.Edges.Count() > 0)
                    {
                        foreach (SequenceNode edge in map.Edges)
                        {
                            if (edge.Square.Occupier == null)
                            {
                                Move move = new Move(edge.Root.Square, edge.Square);
                                int count = 0;
                                SequenceNode node = edge.Parent;
                                SequenceNode root = edge.Root;
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

        public void Move(Move move)
        {
            Move(move.From.Row, move.From.Column, move.To.Row, move.To.Column);
        }

        public void Move(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            if (fromRow < 0 || fromColumn < 0 || fromRow > 7 || fromColumn > 7) throw new ArgumentOutOfRangeException("Row or column was outside board extent.");
            if (toRow < 0 || toRow < 0 || toRow > 7 || toRow > 7) throw new ArgumentOutOfRangeException("Row or column was outside board extent.");

            IEnumerable<Move> moves = ValidMoves;
            
            if (moves.Any(m => m.From.Row == fromRow && m.From.Column == fromColumn && m.To.Row == toRow && m.To.Column == toColumn))
            {
                Square origin = _game.Board[fromRow, fromColumn];
                Square destination = _game.Board[toRow, toColumn];
                origin.Occupier.MoveTo(destination);
            }
        }

        internal void TakePiece()
        {
            PiecesTaken++;
        }

        public Player(string name, Game game, PieceColour colour)
        {
            Name = name;
            _game = game;
            Colour = colour;
        }
    }
}
