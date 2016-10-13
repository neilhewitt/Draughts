using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class BoardState
    {
        private IList<PieceLocation> _pieces;

        public IEnumerable<PieceLocation> AllPieces => _pieces;
        public IEnumerable<PieceLocation> BlackPieces => _pieces.Where(p => p.Colour == PieceColour.Black);
        public IEnumerable<PieceLocation> WhitePieces => _pieces.Where(p => p.Colour == PieceColour.White);

        public PieceLocation For(int row, int column)
        {
            return _pieces.SingleOrDefault(p => p.Location.Row == row && p.Location.Column == column);
        }

        public BoardState(Board board)
        {
            _pieces = new List<PieceLocation>(
                board.Squares.Where(s => s.Occupier != null).Select(s => new PieceLocation(new Location(s.RowIndex, s.ColumnIndex), s.Occupier.Colour, s.Occupier.IsCrowned))
                );
        }
    }

    public class PieceLocation
    {
        public Location Location { get; }
        public PieceColour Colour { get; }
        public bool IsCrowned { get; }

        public PieceLocation(Location location, PieceColour colour, bool isCrowned)
        {
            Location = location;
            Colour = colour;
            IsCrowned = isCrowned;
        }
    }
}
