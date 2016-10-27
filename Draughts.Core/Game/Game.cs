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

        public Player BlackPlayer { get; private set; }
        public Player WhitePlayer { get; private set; }

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
            Move selectedMove = null;
            Move bestMove = null;
            IEnumerable<Move> validMoves = player.GetValidMoves(out bestMove);

            selectedMove = player.IsComputerPlayer ? bestMove : player.MoveSelector(validMoves, bestMove);
            if (selectedMove == null)
            {
                GameEnds(this, new GameEndsEventArgs(player.Opponent, ReasonsForWinning.CantMove, Board.State));
                return false;
            }

            BeforePlayerMoves(this, new BeforeMoveEventArgs(player, validMoves, bestMove, Board.State));
            player.Move(selectedMove);
            PlayerMoves(this, new MoveEventArgs(player, selectedMove, Board.State));

            if (player.Opponent.PiecesRemaining == 0)
            {
                GameEnds(this, new GameEndsEventArgs(player.Opponent, ReasonsForWinning.AllPiecesTaken, Board.State));
                return false;
            }

            return true;
        }

        public Game()
        {
            BlackPlayer = new Player(this, PieceColour.Black);
            WhitePlayer = new Player(this, PieceColour.White);

            Board = new Board(this);
            Board.Initialise();

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
