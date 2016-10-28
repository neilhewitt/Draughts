using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class PieceInfo
    {
        public PieceColour Colour { get; }
        public bool IsCrowned { get; }

        public PieceInfo(PieceColour colour, bool isCrowned)
        {
            Colour = colour;
            IsCrowned = isCrowned;
        }
    }
}
