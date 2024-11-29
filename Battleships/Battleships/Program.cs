using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships 
{
    internal class Program
    {
#pragma warning disable CS8618 
        static int BattlefieldSize;
        static Battlefield PlayerBf;
        static Battlefield AIBf;

		static List<Weapon> Weapons;

		static AI Svatka;
		static Player Human;

		static int SonarUses;
		static int BombardmentUses;

        static int difficulty = 0;
        static Phase ShipPlace;
		static Phase GamePhase;
		static SetDifficulty DifficultySet;
		static Phase EndPhase;
#pragma warning restore CS8618 

        static void Main(string[] args)
        {
			Console.CursorVisible = false;
			while (true)
			{
				Init();
				// placing ships
				AIBf.PlaceAllShips();
				ShipPlace.Start();

				//set difficulty
				DifficultySet.Start(out difficulty);
                Svatka.Difficulty = difficulty;

				// game loop
				GamePhase.Start();

				//Display winner
                EndPhase.Start();
            }
        }
       static void Init()
        {
			BattlefieldSize = 12;
			SonarUses = 4;
			BombardmentUses = 4;
            PlayerBf = new Battlefield(12, [0, 12]);

			AIBf = new Battlefield(12, [30, 12]);
			

            Weapons = new List<Weapon>() { new Cannon(), new Bomb(SonarUses), new Bombardment(BombardmentUses), new Sonar(SonarUses) };
            Svatka = new AI(BattlefieldSize, Weapons);
			Weapons = new List<Weapon>() { new Cannon(), new Bomb(SonarUses), new Bombardment(BombardmentUses), new Sonar(SonarUses)};
			Human = new Player(Weapons);
			ShipPlace = new ShipPlacement(ref PlayerBf, ref AIBf);
            GamePhase = new Game(ref PlayerBf, ref AIBf, ref Human, ref Svatka);
			EndPhase = new DisplayWinner(ref PlayerBf, ref AIBf);
            DifficultySet = new SetDifficulty(ref PlayerBf, ref AIBf);
            Console.Clear();
        }
    }
}
