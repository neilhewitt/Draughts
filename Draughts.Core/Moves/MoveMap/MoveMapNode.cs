using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveMapNode
    {
        private List<MoveMapNode> _children;
        private MoveMapNode _parent;

        public Square Square { get; }
        public MoveMapNode Parent => _parent;
        public IEnumerable<MoveMapNode> Children => _children;

        public MoveMapNode Root
        {
            get
            {
                MoveMapNode node = this;
                while (node.Parent != null) node = node.Parent;
                return node;
            }
        }


        internal MoveMapNode AddChild(Square square)
        {
            MoveMapNode node = new MoveMapNode(this, square);
            _children.Add(node);
            return node;
        }

        public MoveMapNode(MoveMapNode parent, Square square)
        {
            _parent = parent;
            Square = square;
            _children = new List<MoveMapNode>();
        }
    }
}
