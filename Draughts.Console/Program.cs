using Draughts.Core;
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
            Game game = new Game();
            Display(game);
            Console.ReadLine();
        }

        public static void Display(Game game)
        {
            Console.Clear();
            Console.WriteLine("SuperDraughts (C)2016 Zero Point Systems Ltd");
            Console.WriteLine("--------------------------------------------");
            Console.Write("\n");

            string[] state = game.GetState();

            for(int i = 0; i < 8; i++)
            {
                Console.Write("\t ─ ─ ─ ─ ─ ─ ─ ─\n\t");
                string row = state[i];
                for (int j = 0; j < 8; j++)
                {
                    Console.Write("|" + row[j]);
                }
                Console.WriteLine("|");
            }
            Console.Write("\t ─ ─ ─ ─ ─ ─ ─ ─\n\n");

            Console.WriteLine("\nSHALL WE PLAY A GAME? (Y/N)");
        }
    }
}
