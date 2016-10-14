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
            MoveTree tree = GetMoveTree();

            if (tree.IsValidToMoveTo(square))
            {
                MoveNode root = tree.Root;
                MoveNode edge = tree.EdgeFor(square);

                MoveNode node = edge;
                while (node.Parent != null)
                {
                    TakePieceOn(node.Square);
                    node = node.Parent;
                }

                _board[_row, _column].Clear();
                square.Occupy(this);
                _row = square.RowIndex;
                _column = square.ColumnIndex;

                if ((_row == 0 && Colour == PieceColour.White) || (_row == 7 && Colour == PieceColour.Black))
                {
                    TakeCrown();
                }

                return true;
            }

            return false;
        }

        public void TakeCrown()
        {
            IsCrowned = true;
        }

        public MoveTree GetMoveTree()
        {
            return new MoveTree(_board, _row, _column);
        }

        private void TakePieceOn(Square square)
        {
            if (square.IsOccupied)
            {
                square.Clear();
                Owner.TakePiece();
            }
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
