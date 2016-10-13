using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveMap
    {
        private SequenceNode _root;
        private Board _board;
        private PieceColour _colour;

        public SequenceNode Root => _root;
        
        public IEnumerable<SequenceNode> Sequences
        {
            get
            {
                return _root.Children;
            }
        }

        public IEnumerable<SequenceNode> Edges
        {
            get
            {
                List<SequenceNode> nodes = new List<SequenceNode>();
                FindEdges(_root, nodes);
                return nodes.Where(node => node != _root);
            }
        }

        private void FindEdges(SequenceNode node, IList<SequenceNode> output)
        {
            if (node.Children.Count() == 0)
            {
                output.Add(node);
            }
            else
            {
                foreach(SequenceNode childNode in node.Children)
                {
                    FindEdges(childNode, output);
                }
            }
        }

        private void TestMove(int row, int column, int rowStep, SequenceNode node)
        {
            if (row < 0 || column < 0 || row > 7 || column > 7) return;

            Square square = _board[row, column];
            if (square.Colour == SquareColour.White) return;
            if (square.Occupier == null)
            {
                node.AddChild(square);
                return;
            }
            if (square.Occupier.Colour != _colour)
            {
                SequenceNode childNode = node.AddChild(square);
                TestMove(row + rowStep, column + 1, rowStep, childNode);
                TestMove(row + rowStep, column - 1, rowStep, childNode);
            }
        }

        private void Calculate()
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

            List<SequenceNode> edges = new List<SequenceNode>();
            List<SequenceNode> invalidRoots = new List<SequenceNode>();
            FindEdges(_root, edges);
            foreach(SequenceNode edge in edges)
            {
                if (edge.Square.Occupier != null)
                {
                    SequenceNode node = edge;
                    while (node.Parent != _root) node = node.Parent;
                    _root.RemoveChild(node);
                }
            }
        }

        public MoveMap(Board board, Square start)
        {
            if (start.Occupier == null)
            {
                throw new ArgumentException("Starting square must be occupied.");
            }

            _root = new SequenceNode(null, start);
            _board = board;
            _colour = start.Occupier.Colour;
            Calculate();
        }
    }
}
