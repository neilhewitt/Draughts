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

        internal void Move(Move move)
        {
            foreach(MoveStep step in move.Steps)
            {
                Square square = _board[step.Row, step.Column];
                if (step == move.Steps.First())
                {
                    square.Clear();
                }
                if (step == move.Steps.Last())
                {
                    square.Occupy(this);
                    _row = square.RowIndex;
                    _column = square.ColumnIndex;
                }
                else
                {
                    if (square.IsOccupied)
                    {
                        square.Clear();
                        Owner.TakePiece();
                    }
                }
            }

            if ((_row == 0 && Colour == PieceColour.White) || (_row == 7 && Colour == PieceColour.Black))
            {
                IsCrowned = true;
            }
        }

        internal IEnumerable<Move> GetMoves()
        {
            return new MoveTree(_board, _row, _column).Moves;
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
