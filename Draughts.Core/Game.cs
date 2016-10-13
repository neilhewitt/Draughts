﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public interface IPlayDraughts
    {
        void Initialise(Game game);
        void PlayerTakesTurn(Player player);
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
                _gameRunner.PlayerTakesTurn(BlackPlayer);
                _whoseTurnIsItAnyway = WhitePlayer;
                _gameRunner.PlayerTakesTurn(WhitePlayer);
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
