using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class BeforeGameStartsEventArgs
    {
        public BoardState State { get; }
        public Player BlackPlayer { get; }
        public Player WhitePlayer { get; }

        public BeforeGameStartsEventArgs(BoardState state, Player blackPlayer, Player whitePlayer)
        {
            State = state;
            BlackPlayer = blackPlayer;
            WhitePlayer = whitePlayer;
        }
    }
}
