using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Square
    {
        public SquareColour Colour { get; }
        public Piece Occupier { get; private set; }
        public int Row { get; }
        public int Column { get; }
        public bool IsOccupied => Occupier != null;
        public bool IsEmpty => !IsOccupied;

        internal void Clear()
        {
            Occupier = null;
        }

        internal void OccupyWith(Piece occupier)
        {
            Occupier = occupier;
        }

        public Square(SquareColour colour, Piece occupier, int row, int column)
        {
            Colour = colour;
            Occupier = occupier;
            Row = row;
            Column = column;
        }
    }

    public enum SquareColour
    {
        Yellow, White
    }
}
