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
        private IPlayDraughts _agent;
        private Player _whoseTurnIsItAnyway;

        public Player BlackPlayer { get; }
        public Player WhitePlayer { get; }
        public Player CurrentPlayer => _whoseTurnIsItAnyway;

        internal Board Board { get; }

        public PieceState GetState()
        {
            return new PieceState(Board);
        }

        public void StartPlay()
        {
            GameLoop();
        }

        private void GameLoop()
        {
            while (true)
            {
                if (!TakeTurn(BlackPlayer)) break;
                if (!TakeTurn(WhitePlayer)) break;
            }
        }

        private bool TakeTurn(Player player)
        {
            _whoseTurnIsItAnyway = player;
            Move selectedMove = null;
            Move bestMove = null;
            IEnumerable<Move> validMoves = player.GetValidMoves(out bestMove);
            if (!player.IsComputerPlayer)
            {
                selectedMove = _agent.PlayerTakesTurn(validMoves, bestMove);
            }
            else
            {
                selectedMove = bestMove;
            }
            
            if (bestMove == null || !player.Move(selectedMove))
            {
                // can't move, loses game
                _agent.PlayerWins(OpponentOf(player), player, ReasonsForWinning.CantMove);
                return false;
            }

            if (OpponentOf(player).PiecesRemaining == 0)
            {
                _agent.PlayerWins(OpponentOf(player), player, ReasonsForWinning.AllPiecesTaken);
                return false;
            }

            return true;
        }

        private Player OpponentOf(Player player)
        {
            return (player == BlackPlayer ? WhitePlayer : BlackPlayer);
        }

        public Game(string player1Name, string player2Name, IPlayDraughts client, bool computerPlays1 = false, bool computerPlays2 = false)
        {
            _agent = client;
            int coinToss = _random.Next(10);
            BlackPlayer = new Player(coinToss % 2 == 0 ? player1Name : player2Name, this, PieceColour.Black, coinToss % 2 == 0 ? computerPlays1 : computerPlays2);
            WhitePlayer = new Player(coinToss % 2 != 0 ? player1Name : player2Name, this, PieceColour.White, coinToss % 2 != 0 ? computerPlays1 : computerPlays2);

            Board = new Board(this);
            Board.Initialise();
            _whoseTurnIsItAnyway = BlackPlayer;
            _agent.Initialise(this);
        }
    }

    public enum ReasonsForWinning
    {
        AllPiecesTaken,
        CantMove
    }
}
