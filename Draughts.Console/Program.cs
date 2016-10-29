using Draughts.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
            string path = "c:\\temp\\draughts.log";
            if (File.Exists(path)) File.Delete(path);
            int iterations = 1000000000;
            while (iterations-- > 0)
            {
                Game game = new Game();
                TestUI ui = new TestUI(game, path);
                game.RegisterToPlay("John", (moves, move) => move);
                game.RegisterToPlay("Slartibartfast", (moves, move) => move);
                game.Play();
            }
        }
    }
}
