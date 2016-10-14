using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveNode
    {
        private List<MoveNode> _children;
        private MoveNode _parent;

        public Square Square { get; }
        public MoveNode Parent => _parent;
        public IEnumerable<MoveNode> Children => _children;

        public MoveNode Root
        {
            get
            {
                MoveNode node = this;
                while (node.Parent != null) node = node.Parent;
                return node;
            }
        }


        internal MoveNode AddChild(Square square)
        {
            MoveNode node = new MoveNode(this, square);
            _children.Add(node);
            return node;
        }

        internal void ClearChildren()
        {
            _children.Clear();
        }

        internal void RemoveChild(MoveNode childNode)
        {
            if (_children.Contains(childNode))
            {
                _children.Remove(childNode);
            }
        }

        public MoveNode(MoveNode parent, Square square)
        {
            _parent = parent;
            Square = square;
            _children = new List<MoveNode>();
        }
    }
}
