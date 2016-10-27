﻿using System;
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

        private void Evaluate()
        {
            // depending on which player we are evaluating moves for, we need to either move up or down the board
            int rowStep = _root.Square.Occupier.Colour == PieceColour.Black ? 1 : -1;

            int row = _root.Square.RowIndex;
            int column = _root.Square.ColumnIndex;
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

                    move.Add(node.Square.RowIndex, node.Square.ColumnIndex);
                    node = node.Parent;
                }
                moves.Add(move);
            }

            Moves = moves;
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

        public MoveMap(Board board, int row, int column)
        {
            Square start = board[row, column];
            if (start.IsEmpty)
            {
                throw new ArgumentException("Starting square must be occupied.");
            }

            _root = new MoveMapNode(null, start);
            _board = board;
            _colour = start.Occupier.Colour;
            Evaluate();
        }
    }
}
