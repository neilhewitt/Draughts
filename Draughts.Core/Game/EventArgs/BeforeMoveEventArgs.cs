using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class BeforeMoveEventArgs
    {
        public Player Player { get; }
        public IEnumerable<Move> ValidMoves { get; }
        public Move BestMove { get; }
        public BoardState BoardState { get; }

        public BeforeMoveEventArgs(Player player, IEnumerable<Move> validMoves, Move bestMove, BoardState state)
        {
            Player = player;
            ValidMoves = validMoves;
            BestMove = bestMove;
            BoardState = state;
        }
    }
}
