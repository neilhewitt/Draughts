using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Player
    {
        private Game _game;

        public PieceColour Colour { get; }

        public Player(Game game, PieceColour colour)
        {
            _game = game;
            Colour = colour;
        }
    }
}
