using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Game : Phase
    {
        private Player Human { get; set; }
        private AI AIplayer { get; set; }
        private int[] PlayerLoc;
        private Battlefield AiCopy;
        private Battlefield PlayerCopy;
        public Game(ref Battlefield player, ref Battlefield ai, ref Player human, ref AI aiplayer) : base(ref player, ref ai)
        {
            Player = player;
            Ai = ai;
            Human = human;
            AIplayer = aiplayer;
            PlayerLoc = player.Location;
            AiCopy = Ai;
            PlayerCopy = Player;
        }
        public override bool isCompleted { get; set; }
        public override void Init()
        {
            AiCopy = Ai;
            PlayerCopy = Player;
            isCompleted = false;
            Console.SetCursorPosition(0, 0);
            WriteBattleships();
            Console.WriteLine("Pomoci sipek, WSAD, enter namirte");
            Console.WriteLine("Pomoci klaves cisel vyberte zbran, Enter pro vystrel");
        }
        public override void UserInput()
        {
            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.S:
                case ConsoleKey.A:
                case ConsoleKey.D:
                    Human.SelectedWeapon.Rotate(Player.Size, Player.Size);
                    break;
                case ConsoleKey.UpArrow:
                    Human.SelectedWeapon.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    Human.SelectedWeapon.MoveDown(Player.Size);
                    break;
                case ConsoleKey.LeftArrow:
                    Human.SelectedWeapon.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    Human.SelectedWeapon.MoveRight(Player.Size);
                    break;
                case ConsoleKey.Enter:
                    if (Human.Shoot(Human.SelectedWeapon.Location, ref AiCopy))
                    {
                        AIplayer.Shoot(ref PlayerCopy);
                        Ai = AiCopy;
                        Player = PlayerCopy;
                    }
                        
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
        public override void Render()
        {
            Console.SetCursorPosition(0, 7);
            for (int i = 0; i < Human.Weapons.Count; i++)
            {
                DrawAmmo(Human.Weapons[i], i + 1);
            }

            Player.Location = PlayerLoc;
            Player.DrawBattlefield("Hrac", false);
            Ai.DrawBattlefield("AI opponent", false, false);
            Human.SelectedWeapon.DrawWeapon(Ai.Location);

            Player.Location = [60, 12];
            Player.DrawBattlefield("Pohled AI", false, false);

        }
        public override void Update()
        {
            if (Player.AllShipsDestroyed() || Ai.AllShipsDestroyed())
            {
                Render();
                isCompleted = true;
            }
        }
        static void DrawAmmo(Weapon weapon, int num)
        {
            if (weapon.Uses != 0)
                Console.ForegroundColor = ConsoleColor.White;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(num.ToString() + " - ");
            Console.Write(weapon.Name + ": ");
            if (weapon.Uses < 0)
                Console.Write("∞");
            else
                Console.Write(weapon.Uses.ToString());
            Console.WriteLine();

        }
    }
}
