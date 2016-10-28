using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class GameEndsEventArgs
    {
        public Player Winner { get; }
        public ReasonsForWinning ReasonPlayerWon { get; }
        public BoardState BoardState { get; }

        public GameEndsEventArgs(Player winningPlayer, ReasonsForWinning reasonWon, BoardState state)
        {
            Winner = winningPlayer;
            ReasonPlayerWon = reasonWon;
            BoardState = state;
        }
    }
}
