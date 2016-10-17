using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Move
    {
        public int FromRow { get; }
        public int FromColumn { get; }
        public int ToRow { get; }
        public int ToColumn { get; }

        internal Move(Square start, Square end)
        {
            FromRow = start.RowIndex;
            FromColumn = start.ColumnIndex;
            ToRow = end.RowIndex;
            ToColumn = end.ColumnIndex;
        }
    }
}
