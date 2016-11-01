using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public class MiniMaxResult
    {
        public Move BestMove => BestMovePerGeneration[1];
        public int PlayerPiecesRemaining { get; set; }
        public int OpponentPiecesRemaining { get; set; }

        internal Dictionary<int, Move> BestMovePerGeneration { get; }

        public MiniMaxResult()
        {
            PlayerPiecesRemaining = 0;
            OpponentPiecesRemaining = 12;
            BestMovePerGeneration = new Dictionary<int, Core.Move>();
        }
    }
}
