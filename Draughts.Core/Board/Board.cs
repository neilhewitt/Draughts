using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexGrid;

namespace Draughts.Core
{
    public class Board
    {
        private Game _game;
        private Grid<Square> _grid;

        public BoardState State => new BoardState(this);

        public Square this[int row, int column]
        {
            get
            {
                if (row < 0 || row > 7 || column < 0 || column > 7) return null;

                return _grid[row, column];
            }
        }

        public IEnumerable<Square> Squares => _grid.Select(s => s.Contents);

        public void Initialise()
        {
            _grid.Clear();
            for(int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    SquareColour colour = (row % 2 == 0 ? (column % 2 == 0 ? SquareColour.Yellow : SquareColour.White) : (column % 2 == 0 ? SquareColour.White : SquareColour.Yellow));
                    Piece piece = ((row < 3 || row > 4) && colour == SquareColour.Yellow) ?
                        (row < 3 ? new Piece(PieceColour.Black, this, row, column, _game.BlackPlayer) : new Piece(PieceColour.White, this, row, column, _game.WhitePlayer))
                        : null;
                    _grid[row, column] = new Square(colour, piece, row, column);
                }
            }
        }

        public Board(Game game)
        {
            _game = game;
            _grid = new Grid<Square>(8, 8);
        }
    }
}
