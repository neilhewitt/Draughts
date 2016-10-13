using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Player
    {
        private Game _game;
        private List<Move> _currentValidMoves;

        public string Name { get; }
        public PieceColour Colour { get; }

        public int PiecesRemaining => _game.Board.Squares.Count(s => s.Occupier.Owner == this);

        public IEnumerable<Move> ValidMoves
        {
            get
            {
                lock (this)
                {
                    if (_currentValidMoves == null)
                    {
                        _currentValidMoves = new List<Move>();
                        foreach (Piece piece in _game.Board.Squares.Where(s => s.Occupier != null && s.Occupier.Owner == this).Select(s => s.Occupier))
                        {
                            MoveMap map = piece.GetMoveMap();
                            if (map.Edges.Count() > 0)
                            {
                                foreach (SequenceNode node in map.Edges)
                                {
                                    if (node.Square.Occupier == null)
                                    {
                                        _currentValidMoves.Add(new Move(node.Root.Square, node.Square));
                                    }
                                }
                            }
                        }
                    }
                }
                return _currentValidMoves;
            }
        }

        public void MovesPiece(Move move)
        {
            MovesPiece(move.From.Row, move.From.Column, move.To.Row, move.To.Column);
        }

        public void MovesPiece(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            if (fromRow < 0 || fromColumn < 0 || fromRow > 7 || fromColumn > 7) throw new ArgumentOutOfRangeException("Row or column was outside board extent.");
            if (toRow < 0 || toRow < 0 || toRow > 7 || toRow > 7) throw new ArgumentOutOfRangeException("Row or column was outside board extent.");

            IEnumerable<Move> moves = _currentValidMoves;
            
            if (moves.Any(m => m.From.Row == fromRow && m.From.Column == fromColumn && m.To.Row == toRow && m.To.Column == toColumn))
            {
                Square origin = _game.Board[fromRow, fromColumn];
                Square destination = _game.Board[toRow, toColumn];
                origin.Occupier.MoveTo(destination);

                lock (this)
                {
                    _currentValidMoves = null; // state has changed
                }
            }
        }

        public Player(string name, Game game, PieceColour colour)
        {
            Name = name;
            _game = game;
            Colour = colour;
        }
    }
}
