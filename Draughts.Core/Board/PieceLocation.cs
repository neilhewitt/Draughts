using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class PieceLocation
    {
        public int Row { get; }
        public int Column { get; }
        public PieceColour Colour { get; }
        public bool IsCrowned { get; }

        public PieceLocation(int row, int column, PieceColour colour, bool isCrowned)
        {
            Row = row;
            Column = column;
            Colour = colour;
            IsCrowned = isCrowned;
        }
    }
}
