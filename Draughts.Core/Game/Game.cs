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
        private Player _whoseTurnIsItAnyway;

        public Player BlackPlayer { get; private set; }
        public Player WhitePlayer { get; private set; }
        public Player CurrentPlayer => _whoseTurnIsItAnyway;

        public event EventHandler<BeforeMoveEventArgs> BeforePlayerMoves;
        public event EventHandler<MoveEventArgs> PlayerMoves;
        public event EventHandler<GameEndsEventArgs> GameEnds;

        internal Board Board { get; }

        public void Play()
        {
            GameLoop();
        }

        public bool RegisterToPlay(string name, Func<IEnumerable<Move>, Move, Move> moveSelector)
        {
            if (!BlackPlayer.IsComputerPlayer && !WhitePlayer.IsComputerPlayer)
            {
                return false;
            }

            if (BlackPlayer.IsComputerPlayer && WhitePlayer.IsComputerPlayer)
            {
                int coinToss = _random.Next(10);
                (coinToss % 2 == 0 ? BlackPlayer : WhitePlayer).BecomeHuman(name, moveSelector);
            }
            else
            {
                (BlackPlayer.IsComputerPlayer ? BlackPlayer : WhitePlayer).BecomeHuman(name, moveSelector);
            }

            return true;
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

            selectedMove = player.IsComputerPlayer ? bestMove : player.MoveSelector(validMoves, bestMove);
            if (selectedMove == null)
            {
                GameEnds(this, new GameEndsEventArgs(player.Opponent, ReasonsForWinning.CantMove, new BoardState(Board)));
                return false;
            }

            BeforePlayerMoves(this, new BeforeMoveEventArgs(player, validMoves, bestMove, new BoardState(Board)));
            player.Move(selectedMove);
            PlayerMoves(this, new MoveEventArgs(player, selectedMove, new BoardState(Board)));

            if (player.Opponent.PiecesRemaining == 0)
            {
                GameEnds(this, new GameEndsEventArgs(player.Opponent, ReasonsForWinning.AllPiecesTaken, new BoardState(Board)));
                return false;
            }

            return true;
        }

        public Game()
        {
            BlackPlayer = new Player("Computer", this, PieceColour.Black, true, (moves, move) => move);
            WhitePlayer = new Player("Computer", this, PieceColour.White, true, (moves, move) => move);

            Board = new Board(this);
            Board.Initialise();
            _whoseTurnIsItAnyway = BlackPlayer;

            this.BeforePlayerMoves += (sender, e) => { };
            this.PlayerMoves += (sender, e) => { };
            this.GameEnds += (sender, e) => { };
        }
    }

    public enum ReasonsForWinning
    {
        AllPiecesTaken,
        CantMove
    }
}
