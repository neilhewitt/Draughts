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
        Move PlayerTakesTurn(Player player);
        void PlayerWins(Player player, Player opponent, ReasonsForLosing reason);
    }
}
