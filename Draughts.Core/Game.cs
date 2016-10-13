using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public interface IPlayDraughts
    {
        void Initialise(Game game);
        bool PlayerTakesTurn(Player player);
        void PlayerWins(Player player);
    }

    public class Game
    {
        private static Random _random = new Random();
        private IPlayDraughts _gameRunner;
        private Player _whoseTurnIsItAnyway;

        public Player BlackPlayer { get; }
        public Player WhitePlayer { get; }
        public Player CurrentPlayer => _whoseTurnIsItAnyway;

        internal Board Board { get; }

        public BoardState GetState()
        {
            return new BoardState(Board);
        }

        public void StartPlay()
        {
            GameLoop();
        }

        private void GameLoop()
        {
            while (true)
            {
                _whoseTurnIsItAnyway = BlackPlayer;

                if (!_gameRunner.PlayerTakesTurn(BlackPlayer))
                {
                    // black can't move, loses game
                    _gameRunner.PlayerWins(WhitePlayer);
                    return;
                }

                if (WhitePlayer.PiecesRemaining == 0)
                {
                    _gameRunner.PlayerWins(BlackPlayer);
                    return;
                }

                _whoseTurnIsItAnyway = WhitePlayer;

                if (!_gameRunner.PlayerTakesTurn(WhitePlayer))
                {
                    // black can't move, loses game
                    _gameRunner.PlayerWins(BlackPlayer);
                    return;
                }

                if (BlackPlayer.PiecesRemaining == 0)
                {
                    _gameRunner.PlayerWins(WhitePlayer);
                    return;
                }
            }
        }

        public Game(string player1Name, string player2Name, IPlayDraughts gameRunner)
        {
            _gameRunner = gameRunner;
            int coinToss = _random.Next(10);
            BlackPlayer = new Player(coinToss % 2 == 0 ? player1Name : player2Name, this, PieceColour.Black);
            WhitePlayer = new Player(coinToss % 2 != 0 ? player1Name : player2Name, this, PieceColour.White);
            Board = new Board(this);
            Board.Initialise();
            _whoseTurnIsItAnyway = BlackPlayer;
            _gameRunner.Initialise(this);
        }
    }
}
