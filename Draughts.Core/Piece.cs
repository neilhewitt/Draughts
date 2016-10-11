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
        private Player _player;

        public PieceColour Colour { get; }
        public bool IsCrowned { get; private set; }

        public void Crown()
        {
            IsCrowned = true;
        }

        public Piece(PieceColour colour, Board board, Player player)
        {
            _board = board;
            Colour = colour;
        }
    }

    public enum PieceColour
    {
        Black, White
    }
}
