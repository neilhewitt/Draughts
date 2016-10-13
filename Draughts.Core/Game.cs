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

        public Player BlackPlayer { get; }
        public Player WhitePlayer { get; }
        public Board Board { get; }

        public Game(string player1Name, string player2Name)
        {
            int coinToss = _random.Next(10);
            BlackPlayer = new Player(coinToss % 2 == 0 ? player1Name : player2Name, this, PieceColour.Black);
            WhitePlayer = new Player(coinToss % 2 != 0 ? player1Name : player2Name, this, PieceColour.White);
            Board = new Board(this);
            Board.Initialise();
        }
    }
}
