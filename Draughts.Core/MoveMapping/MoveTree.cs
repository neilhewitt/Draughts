using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveTree
    {
        private MoveNode _root;
        private Board _board;
        private PieceColour _colour;
        private IEnumerable<MoveNode> _edges;

        public MoveNode Root => _root;

        public IEnumerable<MoveNode> Nodes => _root.Children;

        public IEnumerable<MoveNode> Edges
        {
            get
            {
                List<MoveNode> nodes = new List<MoveNode>();
                FindEdges(_root, nodes);
                return nodes.Where(node => node != _root);
            }
        }

        public bool IsValidToMoveTo(Square square)
        {
            return Edges.Any(x => x.Square == square);
        }

        public MoveNode EdgeFor(Square square)
        {
            return Edges.FirstOrDefault(x => x.Square == square);
        }

        private void FindEdges(MoveNode node, IList<MoveNode> output)
        {
            if (node.Children.Count() == 0)
            {
                output.Add(node);
            }
            else
            {
                foreach(MoveNode childNode in node.Children)
                {
                    FindEdges(childNode, output);
                }
            }
        }

        private void TestMove(int row, int column, int rowStep, MoveNode node)
        {
            if (row < 0 || column < 0 || row > 7 || column > 7) return; // stepped out of bounds, sequence ends

            Square square = _board[row, column];
            if (square.IsEmpty) // sequence ends here
            {
                node.AddChild(square);
                return;
            }
            else if (square.Occupier.Colour == _colour) // blocked by piece of own colour, not a valid sequence
            {
                return;
            }
            else // sequence *may* continue, so recurse down one level in each diagonal direction
            {
                MoveNode childNode = node.AddChild(square);
                TestMove(row + rowStep, column + 1, rowStep, childNode);
                TestMove(row + rowStep, column - 1, rowStep, childNode);
            }
        }

        private void Evaluate()
        {
            int rowStep = _root.Square.Occupier.Colour == PieceColour.Black ? 1 : -1;

            int row = _root.Square.RowIndex;
            int column = _root.Square.ColumnIndex;
            TestMove(row + rowStep, column + 1, rowStep, _root);
            TestMove(row + rowStep, column - 1, rowStep, _root);
            if (_root.Square.Occupier.IsCrowned)
            {
                TestMove(row - rowStep, column + 1, -rowStep, _root);
                TestMove(row - rowStep, column - 1, -rowStep, _root);
            }

            List<MoveNode> edges = new List<MoveNode>();
            FindEdges(_root, edges);
            foreach(MoveNode edge in edges)
            {
                if (edge.Square.IsOccupied)
                {
                    MoveNode node = edge;
                    while (node.Parent != null && node.Parent != _root) node = node.Parent;
                    _root.RemoveChild(node);
                }
            }
        }

        public MoveTree(Board board, int row, int column)
        {
            Square start = board[row, column];
            if (start.IsEmpty)
            {
                throw new ArgumentException("Starting square must be occupied.");
            }

            _root = new MoveNode(null, start);
            _board = board;
            _colour = start.Occupier.Colour;
            Evaluate();
        }
    }
}
