using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Move
    {
        private IList<MoveStep> _steps;

        public MoveStep Start => _steps.FirstOrDefault();
        public MoveStep End => _steps.LastOrDefault();
        public int PiecesTaken => _steps.Count() - 2;
        public bool PieceIsCrowned { get; }

        public IEnumerable<MoveStep> Steps => _steps;

        public void AddToEnd(int row, int column)
        {
            MoveStep node = new MoveStep(row, column, _steps.LastOrDefault());
            _steps.Add(node);
        }

        public void AddToStart(int row, int column)
        {
            MoveStep node = new MoveStep(row, column, null);
            node.AddNext(_steps.FirstOrDefault());
            _steps.Insert(0, node);
        }

        internal Move(bool isCrowned)
        {
            _steps = new List<MoveStep>();
            PieceIsCrowned = isCrowned;
        }
    }
}
