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
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        public bool IsOccupied => Occupier != null;
        public bool IsEmpty => !IsOccupied;

        internal void Clear()
        {
            Occupier = null;
        }

        internal void Occupy(Piece occupier)
        {
            Occupier = occupier;
        }

        public Square(SquareColour colour, Piece occupier, int rowIndex, int columnIndex)
        {
            Colour = colour;
            Occupier = occupier;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }

    public enum SquareColour
    {
        Yellow, White
    }
}
