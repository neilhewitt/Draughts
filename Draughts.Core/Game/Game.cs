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

        private int _blackTurns;
        private int _whiteTurns;

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

        public bool RegisterToPlay(string name, Func<IEnumerable<Move>, Move, Move> selectMoveCallback)
        {
            if (!BlackPlayer.IsComputerPlayer && !WhitePlayer.IsComputerPlayer) return false;

            if (BlackPlayer.IsComputerPlayer && WhitePlayer.IsComputerPlayer)
            {
                int coinToss = _random.Next(10);
                (coinToss % 2 == 0 ? BlackPlayer : WhitePlayer).BecomeHuman(name, selectMoveCallback);
            }
            else
            {
                (BlackPlayer.IsComputerPlayer ? BlackPlayer : WhitePlayer).BecomeHuman(name, selectMoveCallback);
            }

            return true;
        }

        private void GameLoop()
        {
            while (true)
            {
                // if in the first two turns for either player, we use special best move selection behaviour
                // to avoid always having the same opening AI moves
                if (!TakeTurn(BlackPlayer, _blackTurns++ < 2)) break;
                if (!TakeTurn(WhitePlayer, _whiteTurns++ < 2)) break;
            }
        }

        private bool TakeTurn(Player player, bool isStartOfGame)
        {
            Move selectedMove = null;
            Move bestMove = null;
            IEnumerable<Move> validMoves = player.GetValidMoves(out bestMove, isStartOfGame);

            selectedMove = player.IsComputerPlayer ? bestMove : player.SelectMoveCallback(validMoves, bestMove);
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
