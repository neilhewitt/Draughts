using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveMap
    {
        private MoveMapNode _root;
        private Board _board;
        private PieceColour _colour;

        public IEnumerable<Move> Moves { get; private set; }

        private void Generate()
        {
            // depending on which player we are evaluating moves for, we need to either move up or down the board
            int rowStep = _root.Square.Occupier.Colour == PieceColour.Black ? 1 : -1;

            int row = _root.Square.Row;
            int column = _root.Square.Column;
            TestMove(row + rowStep, column + 1, rowStep, _root);
            TestMove(row + rowStep, column - 1, rowStep, _root);

            // a crowned piece ('king') may move in the reverse direction as well
            if (_root.Square.Occupier.IsCrowned)
            {
                TestMove(row - rowStep, column + 1, -rowStep, _root);
                TestMove(row - rowStep, column - 1, -rowStep, _root);
            }

            // extract the edges (end squares) of each sequence, and remove those that end in an occupied square on the board's edge,
            // as those are not valid move targets
            // create a path from each edge which enumerates the squares covered by the move
            List<MoveMapNode> edges = new List<MoveMapNode>();
            FindEdges(_root, edges);

            List<Move> moves = new List<Move>();
            foreach (MoveMapNode edge in edges.Where(e => e.Square.IsEmpty))
            {
                Move move = new Move(edge.Root.Square.Occupier);
                MoveMapNode node = edge;
                while(true)
                {
                    if (node == null) break;

                    move.Add(node.Square.Row, node.Square.Column);
                    node = node.Parent;
                }
                moves.Add(move);
            }

            // exclude any moves that don't take pieces, if at least one does - we *must* take a piece if it's possible
            Moves = moves.Any(m => m.PiecesTaken > 0) ? moves.Where(m => m.PiecesTaken > 0) : moves; 
        }

        private void FindEdges(MoveMapNode node, IList<MoveMapNode> output)
        {
            if (node.Children.Count() == 0)
            {
                output.Add(node);
            }
            else
            {
                foreach (MoveMapNode childNode in node.Children)
                {
                    FindEdges(childNode, output);
                }
            }
        }

        private void TestMove(int row, int column, int rowStep, MoveMapNode node)
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
                MoveMapNode childNode = node.AddChild(square);
                TestMove(row + rowStep, column + 1, rowStep, childNode);
                TestMove(row + rowStep, column - 1, rowStep, childNode);
            }
        }

        public MoveMap(Piece piece)
        {
            Square start = piece.Square;
            _board = piece.Board;
            _colour = start.Occupier.Colour;
            _root = new MoveMapNode(null, start);
            Generate();
        }
    }
}
