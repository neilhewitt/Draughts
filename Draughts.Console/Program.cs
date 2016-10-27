using Draughts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draughts.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Game game = new Game();
                TestUI ui = new TestUI(game);
                game.RegisterToPlay("John", (moves, move) => move);
                game.Play();
            }
        }
    }
}
