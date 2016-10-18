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
        private IPlayDraughts _client;
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
                TakeTurn(BlackPlayer);
                TakeTurn(WhitePlayer);
            }
        }

        private void TakeTurn(Player player)
        {
            _whoseTurnIsItAnyway = player;
            Move move = player.IsComputerPlayer ? player.BestMove : _client.PlayerTakesTurn(player.ValidMoves, player.BestMove);
            
            if (move == null || !player.Move(move))
            {
                // can't move, loses game
                _client.PlayerWins(OpponentOf(player), player, ReasonsForLosing.CantMove);
                return;
            }

            if (OpponentOf(player).PiecesRemaining == 0)
            {
                _client.PlayerWins(OpponentOf(player), player, ReasonsForLosing.AllPiecesTaken);
                return;
            }
        }

        private Player OpponentOf(Player player)
        {
            if (player == BlackPlayer) return WhitePlayer;
            return BlackPlayer;
        }

        public Game(string player1Name, string player2Name, IPlayDraughts client, bool computerPlays1 = false, bool computerPlays2 = false)
        {
            _client = client;
            int coinToss = _random.Next(10);

            BlackPlayer = new Player(coinToss % 2 == 0 ? player1Name : player2Name, this, PieceColour.Black, coinToss % 2 == 0 ? computerPlays1 : computerPlays2);
            WhitePlayer = new Player(coinToss % 2 != 0 ? player1Name : player2Name, this, PieceColour.White, coinToss % 2 != 0 ? computerPlays1 : computerPlays2);
            Board = new Board(this);
            Board.Initialise();
            _whoseTurnIsItAnyway = BlackPlayer;
            _client.Initialise(this);
        }
    }

    public enum ReasonsForLosing
    {
        AllPiecesTaken,
        CantMove
    }
}
