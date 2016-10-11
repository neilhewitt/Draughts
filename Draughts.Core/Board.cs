using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Board
    {
        private Game _game;
        private Grid<Piece> _grid;

        internal Grid<Piece> Grid => _grid;

        public void Initialise()
        {
            _grid.Clear();
            for(int row = 0; row < 8; row++)
            {
                if (row > 1 && row < 6) row = 6; // skip middle rows
                for (int col = 0; col < 8; col++)
                {
                    _grid[row, col] = (row < 2 ? new Piece(PieceColour.Black, this, _game.Black) 
                        : new Piece(PieceColour.White, this, _game.White));
                }
            }
        }

        public Board(Game game)
        {
            _game = game;
            _grid = new Grid<Piece>(8, 8);
        }
    }
}
