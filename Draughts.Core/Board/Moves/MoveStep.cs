using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveStep
    {
        public int Row { get; }
        public int Column { get; }

        public MoveStep Previous { get; }
        public MoveStep Next { get; private set; }
        public bool IsEnd => Next == null;

        public void AddNext(MoveStep node)
        {
            Next = node;
        }

        public MoveStep(int row, int column, MoveStep previous = null)
        {
            Row = row;
            Column = column;
            Previous = previous;
        }
    }
}
