using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class SequenceNode
    {
        private List<SequenceNode> _children;
        private SequenceNode _parent;

        public Square Square { get; }
        public SequenceNode Parent => _parent;
        public IEnumerable<SequenceNode> Children => _children;

        public SequenceNode Root
        {
            get
            {
                SequenceNode node = this;
                while (node.Parent != null) node = node.Parent;
                return node;
            }
        }


        internal SequenceNode AddChild(Square square)
        {
            SequenceNode node = new SequenceNode(this, square);
            _children.Add(node);
            return node;
        }

        internal void ClearChildren()
        {
            _children.Clear();
        }

        internal void RemoveChild(SequenceNode childNode)
        {
            if (_children.Contains(childNode))
            {
                _children.Remove(childNode);
            }
        }

        public SequenceNode(SequenceNode parent, Square square)
        {
            _parent = parent;
            Square = square;
            _children = new List<SequenceNode>();
        }
    }
}
