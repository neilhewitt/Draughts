using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
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
