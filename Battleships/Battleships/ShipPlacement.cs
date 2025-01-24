using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class ShipPlacement : Phase
    {
        public ShipPlacement(ref Battlefield player, ref Battlefield ai) : base(ref player, ref ai)
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
            Console.WriteLine("Pomoci sipek/WSAD, enter zvolte rozmisteni lodi");
            Console.WriteLine("Muzete pouzit autofill klavesou Tab\n");
            Console.WriteLine("Lode se nesmi navzajem prekryvat ani dotykat stranama");
        }
        public override void UserInput()
        {
            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    Player.SelectedShip.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    Player.SelectedShip.MoveDown(Player.Size);
                    break;
                case ConsoleKey.LeftArrow:
                    Player.SelectedShip.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    Player.SelectedShip.MoveRight(Player.Size);
                    break;
                case ConsoleKey.Enter:
                    Player.PlaceSelectedShip();
                    break;
                case ConsoleKey.A:
                case ConsoleKey.W:
                    Player.SelectedShip.Rotate(Player.Size);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.S:
                    Player.SelectedShip.Rotate(Player.Size);
                    break;
                case ConsoleKey.Tab:
                    Player.PlaceAllShips();
                    break;
            }
        }
        public override void Render()
        {
            Player.DrawBattlefield("Hrac");
        }
        public override void Update()
        {
            if (Player.AllShipsPlaced())
            {
                isCompleted = true;
                Console.Clear();
                Player.DrawBattlefield("Hrac", false);
                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
