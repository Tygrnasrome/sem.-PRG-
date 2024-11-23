using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships 
{
    internal class Program
    {
        static char[,] playerBattlefield = new char[8,8];
        static int[] selectedTile = {0, 0};
        static bool valid_placement = true;

        static void Main(string[] args)
        {
            Init();
            while (true)
            {
                valid_placement = true;
                Console.Clear();
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
            // nastavim bitevni pole na prazdna
            for (int i = 0; i < playerBattlefield.GetLength(0); i++)
            {
                for (int j = 0; j < playerBattlefield.GetLength(1); j++)
                {
                    playerBattlefield[i, j] = ' '; //██████████████████
                }
            }
            playerBattlefield[0,0] = 's';


        }
        static void Render()
        {
            DrawBattlefield(playerBattlefield);
        }
        
        static void PlayerInput()
        {
            // Read key without displaying it

            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedTile[1] != 0)
                        selectedTile[1] -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedTile[1] != playerBattlefield.GetLength(1)-1)
                        selectedTile[1] += 1;
                    break;
                case ConsoleKey.LeftArrow:
                    if (selectedTile[0] != 0)
                        selectedTile[0] -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    if (selectedTile[0] != playerBattlefield.GetLength(0) - 1)
                        selectedTile[0] += 1;
                    break;
                case ConsoleKey.Enter:
                    if (valid_placement)
                        playerBattlefield[selectedTile[1], selectedTile[0]] = 's';
                    break;
            }
        }

        static void DrawBattlefield(char[,] battlefield)
        {
            int widht = battlefield.GetLength(0);
            int height = battlefield.GetLength(1);
            Console.Write("┌");
            for (int i = 0; i < widht*2; i++)
                Console.Write("─");
            Console.Write("┐");
            Console.WriteLine();
            for (var col = 0; col < widht; col++)
            {
                Console.Write("|");
                for (var row = 0; row < height; row++)
                {
                    DrawField(battlefield[col, row], selectedTile[0] == row && selectedTile[1] == col);
                }
                Console.Write("|\n");
            }
            Console.Write("└");
            for (int i = 0; i < widht * 2; i++)
                Console.Write("─");
            Console.Write("┘");
            Console.WriteLine();
        }
        static void DrawField(char type, bool is_tmp)
        {
            if (is_tmp)
                switch (type)
                {
                    default: // tmp tiles
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("██");
                        Console.ResetColor();
                        break;

                    case 's': // error tmp tiles - cant be placed
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("██");
                        Console.ResetColor();
                        valid_placement = false;
                        break;
                }
            else
                switch (type)
                {
                    case ' ': // empty tiles
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("██");
                        Console.ResetColor();
                        break;
                    case 's': // ship tiles
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("██");
                        Console.ResetColor();
                        break;
                    case 'd': // destroyed ship tiles
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("██");
                        Console.ResetColor();
                        break;
                }
            }
    }
}
