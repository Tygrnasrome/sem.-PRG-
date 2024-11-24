using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships 
{
    internal class Program
    {
        static Battlefield PlayerBf = new Battlefield(12, [0,8]);
        static Battlefield AIBf = new Battlefield(12, [30, 8]);

        static int[] SelectedTile = {0, 0};
        static bool Valid_placement = true;

        static void Main(string[] args)
        {
            Init();
            while (true)
            {
                Valid_placement = true;
                Console.SetCursorPosition(0, 0); // Reset cursor for redraw, Console.Clear() causes too many rendering - slow

                Render();
                PlayerInput();
                
            }
            // game loop
            while (true)
            {
                Console.Clear();
                Render();
                PlayerInput();
            }
        }
        static void Init()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PlayerBf.Active = true;
        }
        static void Render()
        {
            Console.WriteLine("Battleships");

            PlayerBf.DrawBattlefield(SelectedTile);
            AIBf.DrawBattlefield(SelectedTile);
        }
        
        static void PlayerInput()
        {
            // Read key without displaying it

            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    //if (SelectedTile[1] != 0)
                    //    SelectedTile[1] -= 1;
                    PlayerBf.SelectedShip.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    //if (SelectedTile[1] != PlayerBf.Size-1)
                    //    SelectedTile[1] += 1;
                    PlayerBf.SelectedShip.MoveDown(PlayerBf.Size);
                    break;
                case ConsoleKey.LeftArrow:
                    //if (SelectedTile[0] != 0)
                    //    SelectedTile[0] -= 1;
                    PlayerBf.SelectedShip.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    //if (SelectedTile[0] != PlayerBf.Size - 1)
                    //    SelectedTile[0] += 1;
                    PlayerBf.SelectedShip.MoveRight(PlayerBf.Size);

                    break;
                case ConsoleKey.Enter:
                    //if (valid_placement)
                    //    playerBattlefield[SelectedTile[1], SelectedTile[0]] = 's';
                    break;
            }
        }
    }
}
