﻿using Draughts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Draughts.Site
{
    public class DraughtsPlayer
    {
        public void ComputerPlayerTakesTurn(Move move)
        {
            throw new NotImplementedException();
        }

        public void Initialise(Game game)
        {
            throw new NotImplementedException();
        }

        public Move PlayerTakesTurn(IEnumerable<Move> moves, Move bestMove)
        {
            throw new NotImplementedException();
        }

        public void PlayerWins(Player player, Player opponent, ReasonsForWinning reason)
        {
            throw new NotImplementedException();
        }
    }
}