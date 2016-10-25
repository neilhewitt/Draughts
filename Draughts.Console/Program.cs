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
                ConsoleGameAgent agent = new ConsoleGameAgent();
                Game game = new Game("John", "Jane", agent);
                game.StartPlay();
            }
        }
    }
}
