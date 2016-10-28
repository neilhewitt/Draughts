

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class BoardState
    {
        private IList<SquareInfo> _squares;

        public IEnumerable<SquareInfo> Squares => _squares;
        public int BlackPiecesRemaining => _squares.Where(s => s.PieceInfo != null && s.PieceInfo.Colour == PieceColour.Black).Count();
        public int WhitePiecesRemaining => _squares.Where(s => s.PieceInfo != null && s.PieceInfo.Colour == PieceColour.White).Count();

        public SquareInfo For(int row, int column)
        {
            return _squares.SingleOrDefault(p => p.Row == row && p.Column == column);
        }

        public BoardState(Board board)
        {
            _squares = new List<SquareInfo>(
                board.Squares.Select(s => new SquareInfo(s.Row, s.Column, s.Occupier != null ? new PieceInfo(s.Occupier.Colour, s.Occupier.IsCrowned) : null))
                );
        }
    }
}
