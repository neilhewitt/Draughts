using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.Core
{
    public interface IPlayDraughts
    {
        void Initialise(Game game);
        Move PlayerTakesTurn(IEnumerable<Move> possibleMoves, Move recommendedMove);
        void ComputerPlayerTakesTurn(Move move);
        void PlayerWins(Player player, Player opponent, ReasonsForLosing reason);
    }
}
