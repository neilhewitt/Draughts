using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Piece
    {
        private Board _board;
        private int _row;
        private int _column;

        public PieceColour Colour { get; }
        public bool IsCrowned { get; private set; }
        public Player Owner { get; }

        public Square Location => _board[_row, _column];

        public void Crown()
        {
            IsCrowned = true;
        }

        public IEnumerable<Square> GetValidMoves()
        {
            SequenceMap map = new SequenceMap(_board, Location);
            return map.Edges.Select(x => x.Square);
        }

        public bool MoveTo(Square square, out IEnumerable<Piece> piecesTaken)
        {
            SequenceMap map = new SequenceMap(_board, Location);

            if (map.Edges.Select(x => x.Square).Contains(square))
            {
                SequenceNode root = map.Root;
                SequenceNode edge = map.Edges.FirstOrDefault(x => x.Square == square);

                List<Piece> takenList = new List<Piece>();
                SequenceNode node = edge.Parent;
                while (node.Parent != null && node.Parent != root)
                {
                    takenList.Add(node.Square.Occupier);
                    node = node.Parent;
                }

                Location.Clear();
                square.Occupy(this);
                _row = square.RowIndex;
                _column = square.ColumnIndex;

                piecesTaken = takenList;
                return true;
            }

            piecesTaken = new Piece[0];
            return false;
        }



        private bool CanMoveTo(Square square)
        {
            return GetValidMoves().Contains(square);
        }

        public Piece(PieceColour colour, Board board, int row, int column, Player player)
        {
            _board = board;
            _row = row;
            _column = column;
            Owner = player;
            Colour = colour;
        }
    }

    public enum PieceColour
    {
        Black, White
    }
}
