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
        public Board Board => _board;
        public Square Square => _board[_row, _column];
        public IEnumerable<Move> ValidMoves => new MoveMap(this).Moves;
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
                    square.OccupyWith(this);
                    _row = square.Row;
                    _column = square.Column;
                }
                else
                {
                    if (square.IsOccupied)
                    {
                        square.Clear();
                        Owner.CapturedAPiece();
                    }
                }
            }

            if ((_row == 0 && Colour == PieceColour.White) || (_row == 7 && Colour == PieceColour.Black))
            {
                IsCrowned = true;
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
