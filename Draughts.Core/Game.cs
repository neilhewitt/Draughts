using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Game
    {
        private Board _board;

        public Player Black { get; }
        public Player White { get; }

        public string[] GetState()
        {
            List<string> output = new List<string>();
            foreach(GridRow<Piece> row in _board.Grid.GetRows())
            {
                string rowInfo = String.Empty;
                foreach(GridCell<Piece> cell in row)
                {
                    rowInfo += cell.Contents != null ? (cell.Contents.Colour == PieceColour.Black ? "B" : "W") : " ";
                }
                output.Add(rowInfo);
            }
            return output.ToArray();
        }

        public Game()
        {
            Black = new Player(this, PieceColour.Black);
            White = new Player(this, PieceColour.White);
            _board = new Board(this);
            _board.Initialise();
        }
    }
}
