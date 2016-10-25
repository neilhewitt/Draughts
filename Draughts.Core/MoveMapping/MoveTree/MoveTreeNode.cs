using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveTreeNode
    {
        private List<MoveTreeNode> _children;
        private MoveTreeNode _parent;

        public Square Square { get; }
        public MoveTreeNode Parent => _parent;
        public IEnumerable<MoveTreeNode> Children => _children;

        public MoveTreeNode Root
        {
            get
            {
                MoveTreeNode node = this;
                while (node.Parent != null) node = node.Parent;
                return node;
            }
        }


        internal MoveTreeNode AddChild(Square square)
        {
            MoveTreeNode node = new MoveTreeNode(this, square);
            _children.Add(node);
            return node;
        }

        public MoveTreeNode(MoveTreeNode parent, Square square)
        {
            _parent = parent;
            Square = square;
            _children = new List<MoveTreeNode>();
        }
    }
}
