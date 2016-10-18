﻿using Draughts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ConsoleGameAgent runner = new ConsoleGameAgent();
                Game game = new Game("John", "Jane", new ConsoleGameAgent());
                game.StartPlay();
            }
        }
    }
}
