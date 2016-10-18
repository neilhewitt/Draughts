using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Piece
    {
        private Board _board;
        private int _row;
        private int _column;

        public PieceColour Colour { get; }
        public bool IsCrowned { get; private set; }

        public Player Owner { get; }

        public void Move(Move move)
        {
            foreach(MoveNode node in move.Nodes)
            {
                Square square = _board[node.Row, node.Column];
                if (node == move.Nodes.First())
                {
                    square.Clear();
                }
                if (node == move.Nodes.Last())
                {
                    square.Occupy(this);
                }
                else
                {
                    TakePieceOn(square);
                }
            }

            if ((_row == 0 && Colour == PieceColour.White) || (_row == 7 && Colour == PieceColour.Black))
            {
                TakeCrown();
            }
        }

        public void TakeCrown()
        {
            IsCrowned = true;
        }

        public MoveTree GetMoveTree()
        {
            return new MoveTree(_board, _row, _column);
        }

        private void TakePieceOn(Square square)
        {
            if (square.IsOccupied)
            {
                square.Clear();
                Owner.TakePiece();
            }
        }

        public Piece(PieceColour colour, Board board, int row, int column, Player player)
        {
            _board = board;
            _row = row;
            _column = column;
            Owner = player;
            Colour = colour;
        }
    }

    public enum PieceColour
    {
        Black, White
    }
}
