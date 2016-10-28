using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class SquareInfo
    {
        public int Row { get; }
        public int Column { get; }

        public PieceInfo PieceInfo { get; }

        public SquareInfo(int row, int column, PieceInfo pieceInfo)
        {
            Row = row;
            Column = column;
            PieceInfo = pieceInfo;
        }
    }
}
