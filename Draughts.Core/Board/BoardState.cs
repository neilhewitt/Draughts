

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class BoardState
    {
        private IList<PieceInfo> _pieces;

        public IEnumerable<PieceInfo> AllPieces => _pieces;
        public IEnumerable<PieceInfo> BlackPieces => _pieces.Where(p => p.Colour == PieceColour.Black);
        public IEnumerable<PieceInfo> WhitePieces => _pieces.Where(p => p.Colour == PieceColour.White);

        public PieceInfo For(int row, int column)
        {
            return _pieces.SingleOrDefault(p => p.Row == row && p.Column == column);
        }

        public BoardState(Board board)
        {
            _pieces = new List<PieceInfo>(
                board.Squares.Where(s => s.IsOccupied).Select(s => new PieceInfo(s.RowIndex, s.ColumnIndex, s.Occupier.Colour, s.Occupier.IsCrowned))
                );
        }
    }
}
