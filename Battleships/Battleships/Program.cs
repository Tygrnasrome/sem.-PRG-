using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships 
{
    internal class Program
    {
		static int BattlefieldSize;
        static Battlefield PlayerBf;
        static Battlefield AIBf;

		static List<Weapon> Weapons;

		static AI Svatka;
		static Player Human;

		static int SonarUses;
		static int BombardmentUses;

		static int[] SelectedTile;
        static bool Valid_placement;
        static int[] playerBfLoc;
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
                PlayerBf.DrawBattlefield("Hrac",false);
                Console.SetCursorPosition(0, 0);

				while (Svatka.Difficulty == 0)
				{
					WriteBattleships();
					Console.WriteLine("Zadej obtiznost AI protivnika (1-2)");
					Console.WriteLine("1 - very easy");
					Console.WriteLine("2 - easy");
					Console.WriteLine("3 - normal");
                    Console.WriteLine("4 - hard");
					Console.WriteLine("5 - elite");

					switch (Console.ReadKey(intercept: true).Key)
					{
						case ConsoleKey.D1:
							Svatka.Difficulty = 1;
							break;
						case ConsoleKey.D2:
							Svatka.Difficulty = 2;
							break;
                        case ConsoleKey.D3:
                            Svatka.Difficulty = 3;
                            break;
						case ConsoleKey.D4:
							Svatka.Difficulty = 4;
							break;
						case ConsoleKey.D5:
							Svatka.Difficulty = 5;
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
                Console.SetCursorPosition(0, 0);
                GameRender();
                if (AIBf.AllShipsDestroyed())
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
					Console.ForegroundColor= ConsoleColor.Red;
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
        }
		static void WriteBattleships()
		{
            // ChatGTP generated
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("░░░░░   ░░░░░  ░░░░░  ░░░░░  ░      ░░░░░   ░░░░   ░   ░  ░░░   ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░      ░       ░   ░   ░    ░   ░  ░      ");
            Console.WriteLine("░░░░░   ░░░░░    ░      ░    ░      ░░░░    ░░░░   ░░░░░   ░    ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░           ░  ░   ░   ░    ░           ░ ");
            Console.WriteLine("░░░░░   ░   ░    ░      ░    ░░░░░  ░░░░░   ░░░░   ░   ░  ░░░   ░       ░░░░  \n");
            Console.ResetColor();
        }
        static void Init()
        {
			BattlefieldSize = 12;
			SonarUses = 4;
			BombardmentUses = 4;
			Console.OutputEncoding = System.Text.Encoding.UTF8;
            PlayerBf = new Battlefield(12, [0, 12]);
			playerBfLoc = PlayerBf.Location;
			PlayerBf.Active = true;
			AIBf = new Battlefield(12, [30, 12]);
			

			SelectedTile = [0, 0];
            Valid_placement = true;
            Weapons = new List<Weapon>() { new Cannon(), new Bomb(SonarUses), new Bombardment(BombardmentUses), new Sonar(SonarUses) };
            Svatka = new AI(BattlefieldSize, Weapons);
			Weapons = new List<Weapon>() { new Cannon(), new Bomb(SonarUses), new Bombardment(BombardmentUses), new Sonar(SonarUses)};
			Human = new Player(Weapons);
			Console.Clear();
        }
        static void PlaceRender()
        {
            WriteBattleships();
            Console.WriteLine("Pomoci sipek/WSAD, enter zvolte rozmisteni lodi");
			Console.WriteLine("Muzete pouzit autofill klavesou Tab\n");
			Console.WriteLine("Lode se nesmi navzajem prekryvat ani dotykat stranama");
            
            PlayerBf.DrawBattlefield("Hrac");
        }
		static void GameRender()
		{
            WriteBattleships();

            Console.WriteLine("Pomoci sipek, WSAD, enter namirte");
			Console.WriteLine("Pomoci klaves cisel vyberte zbran, Enter pro vystrel");
			for (int i = 0; i < Human.Weapons.Count; i++)
			{
				DrawAmmo(Human.Weapons[i], i+1);
			}

            PlayerBf.Location = playerBfLoc;
			PlayerBf.DrawBattlefield("Hrac", false);
			AIBf.DrawBattlefield("AI opponent", false, false);
			Human.SelectedWeapon.DrawWeapon(AIBf.Location);

			PlayerBf.Location = [60, 12];
			PlayerBf.DrawBattlefield("Pohled AI", false, false);

		}
        
        static void PlayerInput()
        {
            // Read key without displaying it

            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    PlayerBf.SelectedShip.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    PlayerBf.SelectedShip.MoveDown(PlayerBf.Size);
                    break;
                case ConsoleKey.LeftArrow:
					PlayerBf.SelectedShip.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
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
				case ConsoleKey.W:
				case ConsoleKey.S:
				case ConsoleKey.A:
				case ConsoleKey.D:
					Human.SelectedWeapon.Rotate(PlayerBf.Size, PlayerBf.Size);
					break;
				case ConsoleKey.UpArrow:
					if (SelectedTile[1] != 0)
						SelectedTile[1] -= 1;
					Human.SelectedWeapon.MoveUp();
					break;
				case ConsoleKey.DownArrow:
					if (SelectedTile[1] != PlayerBf.Size - 1)
						SelectedTile[1] += 1;
					Human.SelectedWeapon.MoveDown(PlayerBf.Size);
					break;
				case ConsoleKey.LeftArrow:
					if (SelectedTile[0] != 0)
						SelectedTile[0] -= 1;
					Human.SelectedWeapon.MoveLeft();
					break;
				case ConsoleKey.RightArrow:
					if (SelectedTile[0] != PlayerBf.Size - 1)
						SelectedTile[0] += 1;
					Human.SelectedWeapon.MoveRight(PlayerBf.Size);
					break;
				case ConsoleKey.Enter:
					if(Human.Shoot(SelectedTile, ref AIBf))
						AIGameInput();
					break;
				case ConsoleKey.D1:
					Human.ChangeWeapon(0);
					break;
				case ConsoleKey.D2:
					Human.ChangeWeapon(1);
					break;
				case ConsoleKey.D3:
					Human.ChangeWeapon(2);
					break;
                case ConsoleKey.D4:
                    Human.ChangeWeapon(3);
                    break;
            }
		}

		static void AIGameInput()
		{
			Svatka.Shoot(ref PlayerBf);
		}
		static void DrawAmmo(Weapon weapon, int num)
		{
            if (weapon.Uses != 0)
                Console.ForegroundColor = ConsoleColor.White;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(num.ToString()+ " - ");
			Console.Write(weapon.Name+": ");
            if (weapon.Uses < 0)
                Console.Write("∞");
            else
                Console.Write(weapon.Uses.ToString());
			Console.WriteLine();

        }
    }
}
