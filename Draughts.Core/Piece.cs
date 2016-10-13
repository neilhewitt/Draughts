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

        public bool MoveTo(Square square)
        {
            MoveMap map = GetMoveMap();

            if (map.Edges.Select(x => x.Square).Contains(square))
            {
                SequenceNode root = map.Root;
                SequenceNode edge = map.Edges.FirstOrDefault(x => x.Square == square);

                List<Piece> takenList = new List<Piece>();
                SequenceNode node = edge.Parent;
                while (node.Parent != null && node.Parent != root)
                {
                    takenList.Add(node.Square.Occupier);
                    node.Square.Clear();
                    node = node.Parent;
                }

                _board[_row, _column].Clear();
                square.Occupy(this);
                _row = square.RowIndex;
                _column = square.ColumnIndex;

                if ((_row == 0 && Colour == PieceColour.Black) || (_row == 7 && Colour == PieceColour.White))
                {
                    Crown();
                }

                return true;
            }

            return false;
        }

        public void Crown()
        {
            IsCrowned = true;
        }

        public MoveMap GetMoveMap()
        {
            return new MoveMap(_board, _row, _column);
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
