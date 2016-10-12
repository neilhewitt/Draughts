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

        public IEnumerable<Square> GetValidSquares()
        {
            List<Square> validSquares = new List<Square>();
            foreach(Square square in _board.Squares)
            {
                if (CanMoveTo(square))
                {
                    validSquares.Add(square);
                }
            }
            return validSquares;
        }

        public bool MoveTo(Square square, out IEnumerable<Piece> piecesTaken)
        {
            if (CanMoveTo(square))
            {
                List<Piece> taken = new List<Piece>();
                int row = Math.Min(square.RowIndex, Location.RowIndex) + 1;
                int column = Math.Min(square.ColumnIndex, Location.ColumnIndex) + 1;
                int endRow = Math.Max(square.RowIndex, Location.RowIndex) - 1;
                int endColumn = Math.Max(square.ColumnIndex, Location.ColumnIndex) - 1;
                while (row <= endRow && column <= endColumn)
                {
                    taken.Add(_board[row, column].Occupier);
                    _board[row, column].Clear();
                    row++;
                    column++;
                }

                Location.Clear();
                square.Occupy(this);
                _row = square.RowIndex;
                _column = square.ColumnIndex;

                piecesTaken = taken;
                return true;
            }

            piecesTaken = new Piece[0];
            return false;
        }

        private bool CanMoveTo(Square square)
        {
            // black - row must be greater (unless crowned), column must be -1 or +1 for each row
            // white - row must be less (unless crowned), column must be -1 or +1 for reach row
            // destination square must be unoccupied
            // for a multi-row/col jump, all intervening squares must be occupied by the opposite colour's pieces

            if (square == Location) return false;

            int row = Math.Min(square.RowIndex, Location.RowIndex);
            int column = Math.Min(square.ColumnIndex, Location.ColumnIndex);
            int endRow = Math.Max(square.RowIndex, Location.RowIndex);
            int endColumn = Math.Max(square.ColumnIndex, Location.ColumnIndex);

            if (row == endRow || column == endColumn) return false;

            bool canMove = false;

            if (Colour == PieceColour.Black ? square.RowIndex > Location.RowIndex : square.RowIndex < Location.RowIndex
                || (IsCrowned && square.RowIndex != Location.RowIndex))
            {
                int rowDifference = endRow - row;
                int columnDifference = endColumn - column;
                if (rowDifference == columnDifference && square.Occupier == null)
                {
                    canMove = true;
                    while (++row < endRow && ++column < endColumn)
                    {
                        Piece occupier = _board[row, column].Occupier;
                        if (occupier == null || (Colour == PieceColour.Black ? occupier.Colour == PieceColour.Black : occupier.Colour == PieceColour.White))
                        {
                            canMove = false;
                            break;
                        }
                        row++;
                        column++;
                    }
                }
            }

            return canMove;
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
