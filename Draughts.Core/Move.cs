using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Move
    {
        public Location From { get; }
        public Location To { get; }

        internal Move(Square start, Square end)
        {
            From = new Location(start.RowIndex, start.ColumnIndex);
            To = new Location(end.RowIndex, end.ColumnIndex);
        }
    }
}
