using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Location
    {
        public int Row { get; }
        public int Column { get; }

        public Location(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
