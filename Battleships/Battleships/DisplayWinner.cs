using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class DisplayWinner : Phase
    {
        public DisplayWinner(ref Battlefield player, ref Battlefield ai) : base(ref player, ref ai)
        {
            Player = player;
            Ai = ai;
        }
        public override bool isCompleted { get; set; }
        public override void Init()
        {
            isCompleted = false;
            Console.SetCursorPosition(0, 0);
            WriteBattleships();
            Console.SetCursorPosition(0, Player.Location[1] +Player.Size+2);
            if (Ai.AllShipsDestroyed())
            {
                // ChatGPT generated
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\n░       ░  ░░░  ░   ░");
                Console.WriteLine("░       ░   ░   ░░  ░");
                Console.WriteLine("░   ░   ░   ░   ░ ░ ░");
                Console.WriteLine(" ░ ░ ░ ░    ░   ░  ░░");
                Console.WriteLine("  ░   ░    ░░░  ░   ░\n");
                Console.ResetColor();
            }
            else
            {
                // ChatGPT generated
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\n░░░░   ░░░░░  ░░░░░  ░░░░░  ░░░░░  ░░░░░");
                Console.WriteLine("░   ░  ░      ░      ░      ░   ░    ░  ");
                Console.WriteLine("░   ░  ░░░░   ░░░░   ░░░░   ░░░░░    ░  ");
                Console.WriteLine("░   ░  ░      ░      ░      ░   ░    ░  ");
                Console.WriteLine("░░░░   ░░░░░  ░      ░░░░░  ░   ░    ░  \n");
                Console.ResetColor();
            }
            Console.WriteLine("Pro pokracovani stisknete libovolne tlacitko");
            Console.ReadKey(intercept: true);
        }
        public override void UserInput() {}
        public override void Render() {}
        public override void Update()
        {
            isCompleted = true;
        }
    }
}
