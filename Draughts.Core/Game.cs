using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class Game
    {
        private static Random _random = new Random();

        private Board _board;

        public Player BlackPlayer { get; }
        public Player WhitePlayer { get; }

        public Board Board => _board;

        //public string[] GetState()
        //{
        //    List<string> output = new List<string>();
        //    foreach(GridRow<Piece> row in _board.Grid.GetRows())
        //    {
        //        string rowInfo = String.Empty;
        //        foreach(GridCell<Piece> cell in row)
        //        {
        //            rowInfo += cell.Contents != null ? (cell.Contents.Colour == PieceColour.Black ? "B" : "W") : " ";
        //        }
        //        output.Add(rowInfo);
        //    }
        //    return output.ToArray();
        //}

        public Game(string player1Name, string player2Name)
        {
            int coinToss = _random.Next(10);
            BlackPlayer = new Player(coinToss % 2 == 0 ? player1Name : player2Name, this, PieceColour.Black);
            WhitePlayer = new Player(coinToss % 2 != 0 ? player1Name : player2Name, this, PieceColour.White);
            _board = new Board(this);
            _board.Initialise();
        }
    }
}
