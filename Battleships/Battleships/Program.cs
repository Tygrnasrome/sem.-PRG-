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
		static int AIDifficulty = 0;
        static void Main(string[] args)
        {
			Console.CursorVisible = false;
			while (true)
			{
				Init();
				// placing ships
				AIBf.PlaceAllShips();
				while (!PlayerBf.AllShipsPlaced())
				{
					Valid_placement = true;
					Console.SetCursorPosition(0, 0); // Reset cursor for redraw, Console.Clear() causes too many rendering - slow
					PlaceRender();
					PlayerInput();
				}
				Console.Clear();
                PlayerBf.DrawBattlefield("Hrac", false);
                AIBf.DrawBattlefield("AI opponent", false, true);
                Console.SetCursorPosition(0, 0);

				while (AIDifficulty == 0)
				{
					Console.WriteLine("Battleships\n");
					Console.WriteLine("Zadej obtiznost AI protivnika (1-2)");
					Console.WriteLine("1 - very easy");
					Console.WriteLine("2 - normal");

                    switch (Console.ReadKey(intercept: true).Key)
					{
						case ConsoleKey.D1:
							AIDifficulty = 1;
							break;
						case ConsoleKey.D2:
							AIDifficulty = 2;
							break;
						default:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Nespravny vstup");
							Console.ResetColor();
							break;
					}
					Console.SetCursorPosition(0, 0);
				}
				Console.Clear();

				// game loop
				while (!PlayerBf.AllShipsDestroyed() && !AIBf.AllShipsDestroyed())
				{
					Console.SetCursorPosition(0, 0);
					GameRender();
					PlayerGameInput();
				}
				if (AIBf.AllShipsDestroyed())
				{
					// ChatGPT generated
                    Console.WriteLine("W       W  III  N   N");
                    Console.WriteLine("W       W   I   NN  N");
                    Console.WriteLine("W   W   W   I   N N N");
                    Console.WriteLine(" W W W W    I   N  NN");
                    Console.WriteLine("  W   W    III  N   N");
                }else
				{
                    // ChatGPT generated
                    Console.WriteLine("DDDD   EEEEE  FFFFF  EEEEE  AAAAA  TTTTT");
                    Console.WriteLine("D   D  E      F      E      A   A    T  ");
                    Console.WriteLine("D   D  EEEE   FFF    EEEE   AAAAA    T  ");
                    Console.WriteLine("D   D  E      F      E      A   A    T  ");
                    Console.WriteLine("DDDD   EEEEE  F      EEEEE  A   A    T  ");
                }
				Console.ReadKey(intercept: true);
			}
        }
        static void Init()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PlayerBf.Active = true;

            PlayerBf = new Battlefield(12, [0, 8]);
            AIBf = new Battlefield(12, [30, 8]);

            SelectedTile = [0, 0];
            Valid_placement = true;
            AIDifficulty = 0;
			Console.Clear();
        }
        static void PlaceRender()
        {
            Console.WriteLine("Battleships\n");
			Console.WriteLine("Pomoci sipek, WSAD, enter zvolte rozmisteni lodi");
			Console.WriteLine("Muzete pouzit autofill klavesou Tab");

            PlayerBf.DrawBattlefield("Hrac");
            AIBf.DrawBattlefield("AI opponent", false, true);
        }
		static int[] playerBfLoc = PlayerBf.Location;
		static void GameRender()
		{
			Console.WriteLine("Battleships\n");

			Console.WriteLine("Pomoci sipek, WSAD, enter namirte a strelte");
			PlayerBf.Location = playerBfLoc;
			PlayerBf.DrawBattlefield("Hrac", false);
			AIBf.DrawBattlefield("AI opponent", false, true);
			AIBf.DrawSelectedField(SelectedTile);

			PlayerBf.Location = [60, 8];
			PlayerBf.DrawBattlefield("Pohled AI", false, false);
		}
        
        static void PlayerInput()
        {
            // Read key without displaying it

            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
					if (SelectedTile[1] != 0)
						SelectedTile[1] -= 1;
                    PlayerBf.SelectedShip.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
					if (SelectedTile[1] != PlayerBf.Size-1)
					SelectedTile[1] += 1;
                    PlayerBf.SelectedShip.MoveDown(PlayerBf.Size);
                    break;
                case ConsoleKey.LeftArrow:
					if (SelectedTile[0] != 0)
						SelectedTile[0] -= 1;
					PlayerBf.SelectedShip.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
					if (SelectedTile[0] != PlayerBf.Size - 1)
						SelectedTile[0] += 1;
                    PlayerBf.SelectedShip.MoveRight(PlayerBf.Size);
                    break;
                case ConsoleKey.Enter:
					if (Valid_placement)
						PlayerBf.PlaceSelectedShip();
                    break;
				case ConsoleKey.A:
				case ConsoleKey.W:
					PlayerBf.SelectedShip.Rotate(PlayerBf.Size);
					break;
				case ConsoleKey.D:
				case ConsoleKey.S:
					PlayerBf.SelectedShip.Rotate(PlayerBf.Size);
					break;
				case ConsoleKey.Tab:
					PlayerBf.PlaceAllShips();
					break;
			}
        }
	
		static void PlayerGameInput()
		{
			switch (Console.ReadKey(intercept: true).Key)
			{
				case ConsoleKey.UpArrow:
					if (SelectedTile[1] != 0)
						SelectedTile[1] -= 1;
					break;
				case ConsoleKey.DownArrow:
					if (SelectedTile[1] != PlayerBf.Size - 1)
						SelectedTile[1] += 1;
					break;
				case ConsoleKey.LeftArrow:
					if (SelectedTile[0] != 0)
						SelectedTile[0] -= 1;
					break;
				case ConsoleKey.RightArrow:
					if (SelectedTile[0] != PlayerBf.Size - 1)
						SelectedTile[0] += 1;
					break;
				case ConsoleKey.Enter:
					AIBf.DestroyField(SelectedTile);
					AIGameInput();
					break;

			}
		}
		static Random rand = new Random();
		static void AIGameInput()
		{
			int x, y;
			if (AIDifficulty == 1)
			{
				x = rand.Next(PlayerBf.Size);
				y = rand.Next(PlayerBf.Size);
				while (PlayerBf.RevealedFields[x, y])
				{
					x = rand.Next(PlayerBf.Size);
					y = rand.Next(PlayerBf.Size);
				}

				PlayerBf.DestroyField([x, y]);
			}
			if (AIDifficulty == 2)
			{
				// TODO
			}

		}
	}
}
