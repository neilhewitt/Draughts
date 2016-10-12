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

        public string Name { get; }
        public PieceColour Colour { get; }

        public Player(string name, Game game, PieceColour colour)
        {
            Name = name;
            _game = game;
            Colour = colour;
        }
    }
}
