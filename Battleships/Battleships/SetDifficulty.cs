using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class SetDifficulty : Phase
    {
        public int Difficulty {  get; set; }
        public SetDifficulty(ref Battlefield player, ref Battlefield ai) : base(ref player, ref ai)
        {
            Player = player;
            Ai = ai;
            Difficulty = 0;
        }
        public override bool isCompleted { get; set; }
        public void Start(out int difficulty)
        {
            Init();
            while (!isCompleted)
            {
                Render();
                UserInput();
                Update();
            }
            difficulty = Difficulty;
        }
        public override void Init()
        {
            isCompleted = false;
            Console.SetCursorPosition(0, 0);
            WriteBattleships();
            Console.WriteLine("Zadej obtiznost AI protivnika (1-2)");
            Console.WriteLine("1 - very easy");
            Console.WriteLine("2 - easy");
            Console.WriteLine("3 - normal");
            Console.WriteLine("4 - hard");
            Console.WriteLine("5 - elite");

        }
        public override void UserInput()
        {
            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.D1:
                    Difficulty = 1;
                    break;
                case ConsoleKey.D2:
                    Difficulty = 2;
                    break;
                case ConsoleKey.D3:
                    Difficulty = 3;
                    break;
                case ConsoleKey.D4:
                    Difficulty = 4;
                    break;
                case ConsoleKey.D5:
                    Difficulty = 5;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, 12);
                    Console.WriteLine("Nespravny vstup");
                    Console.ResetColor();
                    break;
            }
        }
        public override void Render() { }
        public override void Update() 
        {
            if (Difficulty != 0)
            {
                isCompleted = true;
                Console.Clear();
            }
        }
    }
}
