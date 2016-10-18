using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Move
    {
        private IList<MoveNode> _nodes;

        public MoveNode Start => _nodes.FirstOrDefault();
        public MoveNode End => _nodes.LastOrDefault();
        public int PiecesTaken => _nodes.Count() - 2;
        public bool PieceIsCrowned { get; }

        public IEnumerable<MoveNode> Nodes => _nodes;

        public void AddToEnd(int row, int column)
        {
            MoveNode node = new MoveNode(row, column, _nodes.LastOrDefault());
            _nodes.Add(node);
        }

        public void AddToStart(int row, int column)
        {
            MoveNode node = new MoveNode(row, column, null);
            node.AddNext(_nodes.FirstOrDefault());
            _nodes.Insert(0, node);
        }

        public Move(bool isCrowned)
        {
            _nodes = new List<MoveNode>();
            PieceIsCrowned = isCrowned;
        }
    }

    public class MoveNode
    {
        public int Row { get; }
        public int Column { get; }

        public MoveNode Previous { get; }
        public MoveNode Next { get; private set; }
        public bool IsEnd => Next == null;

        public void AddNext(MoveNode node)
        {
            Next = node;
        }

        public MoveNode(int row, int column, MoveNode previous = null)
        {
            Row = row;
            Column = column;
            Previous = previous;
        }
    }
}
