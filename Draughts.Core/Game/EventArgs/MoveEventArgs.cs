using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MoveEventArgs
    {
        public Player Player { get; }
        public Move Move { get; }
        public BoardState BoardState { get; }

        public MoveEventArgs(Player player, Move move, BoardState state)
        {
            Player = player;
            Move = move;
            BoardState = state;
        }
    }
}
