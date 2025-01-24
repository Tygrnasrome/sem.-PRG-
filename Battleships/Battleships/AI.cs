using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class AI
    {
        public int Difficulty { get; set; }
        public List<int[]> ShipFields { get; set; }
        public int[,] ShipRotations { get; set; }
        public List<int[]> Checkboard { get; set; }

		public List<Weapon> Weapons { get; set; }
		public AI(int size, List<Weapon> weapons) 
        {
            ShipFields = new List<int[]>();
            Checkboard = new List<int[]>();
            ShipRotations = new int[size, size];
			Weapons = weapons;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size/2; x++)
                {
                    Checkboard.Add([x*2+y%2,y]);
                }
            }
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size / 2; x++)
                {
                    if (y != size-1)
                        Checkboard.Add([x * 2 - y%2 + 1, y]);
                }
            }

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    ShipRotations[x, y] = -1;
                }
            }
            Difficulty = 0;   
        }

        public void Shoot(ref Battlefield PlayerBf)
        {
            switch (Difficulty)
            {
                case 1:
                    VeryEasyAI(ref PlayerBf);
                    break;
				case 2:
					EasyAI(ref PlayerBf);
					break;
				case 3:
                    NormalAI(ref PlayerBf);
                    break;
                case 4:
                    HardAI(ref PlayerBf);
                    break;
				case 5:
					EliteAI(ref PlayerBf);
					break;
			}
        }
        public void VeryEasyAI(ref Battlefield PlayerBf)
        {
			// strili nahodne
            int x, y;
            Random rand = new();
            x = rand.Next(PlayerBf.Size);
            y = rand.Next(PlayerBf.Size);
            while (PlayerBf.RevealedFields[x, y])
            {
                x = rand.Next(PlayerBf.Size);
                y = rand.Next(PlayerBf.Size);
            }

            PlayerBf.DestroyField([x, y]);
        }
		public void EasyAI(ref Battlefield PlayerBf)
		{
			//ma strategii, ale nezjistuje rotace lodi
			if (NearShipShoot(ref PlayerBf)) return;
			CheckboardShoot(ref PlayerBf);
		}
		public void NormalAI(ref Battlefield PlayerBf)
        {
			//ma strategii a zjistuje rotace lodi
			findShipRotations(ref  PlayerBf);
			if (NearShipShoot(ref PlayerBf))return;
            CheckboardShoot(ref PlayerBf);
        }
        public void HardAI(ref Battlefield PlayerBf)
        {
			//navic od predchozich AI vyuziva zbrane sonar a bomb
            findShipRotations(ref PlayerBf);
			if (NearShipShoot(ref PlayerBf)) return;
			if (RandomSpecialWeaponShoot(ref PlayerBf, Weapons[1])) return;
            if (RandomSpecialWeaponShoot(ref PlayerBf, Weapons[3])) return;
            CheckboardShoot(ref PlayerBf);
        }
		public void EliteAI(ref Battlefield PlayerBf)
		{
			//navic od predchozich AI vyuziva strategicky zbrane sonar a bomb
			findShipRotations(ref PlayerBf);
			if (NearShipShoot(ref PlayerBf)) return;
			if (SpecialWeaponShoot(ref PlayerBf, Weapons[1])) return;
			if (SpecialWeaponShoot(ref PlayerBf, Weapons[3])) return;
			CheckboardShoot(ref PlayerBf);
		}
// BrutalAI TODO, kein Zeit, enschuldigung
		//public void BrutalAI(ref Battlefield PlayerBf)
		//{
		//	//navic od predchozich AI vyuziva strategicky zbran bombardment
		//	findShipRotations(ref PlayerBf);
		//	if(BombardmentShoot(ref PlayerBf)) return;
		//	if (NearShipShoot(ref PlayerBf)) return;
		//	if (SpecialWeaponShoot(ref PlayerBf, Weapons[1])) return;
		//	if (SpecialWeaponShoot(ref PlayerBf, Weapons[3])) return;
		//	CheckboardShoot(ref PlayerBf);
		//}
		public bool BombardmentShoot(ref Battlefield PlayerBf)
		{
			if (Weapons[2].Uses == 0)
				return false;
			int[,] values = new int[PlayerBf.Size - 1, PlayerBf.Size - 1];

			for (int y = 1; y < PlayerBf.Size - 1; y++)
			{
				for (int x = 1; x < PlayerBf.Size - 1; x++)
				{
					values[x, y] = BombValue(x, y, ref PlayerBf);
				}
			}

			return true;
		}

		public bool SpecialWeaponShoot(ref Battlefield PlayerBf, Weapon weapon)
		{
			if (weapon.Uses == 0)
				return false;
			int[,] values = new int[PlayerBf.Size-1, PlayerBf.Size - 1];
			for (int y = 1; y < PlayerBf.Size - 1; y++)
			{
				for (int x = 1; x < PlayerBf.Size - 1; x++)
				{
					values[x, y] = BombValue(x, y, ref PlayerBf);
				}
			}
			int[] maxValueLocation =new int[2];
			int maxValue = 0;

			for (int y = 1; y < PlayerBf.Size - 1; y++)
			{
				for (int x = 1; x < PlayerBf.Size - 1; x++)
				{
					maxValue = Math.Max(values[x,y], maxValue);
					if (maxValue == values[x,y])
						maxValueLocation = [x,y];
				}

			}

			ShootField(ref PlayerBf, maxValueLocation, weapon);
			return true;
		}
		public bool RandomSpecialWeaponShoot(ref Battlefield PlayerBf, Weapon weapon)
        {
            if (weapon.Uses == 0)
                return false;
            int x, y;
            Random rand = new();
            x = rand.Next(1, PlayerBf.Size-1);
            y = rand.Next(1, PlayerBf.Size-1);
            while (BombValue(x,y, ref PlayerBf) < 9*9+1)
            {
                x = rand.Next(1, PlayerBf.Size - 1);
                y = rand.Next(1, PlayerBf.Size - 1);
            }
            ShootField(ref PlayerBf, [x,y], weapon);
            return true;
        }
        public int BombValue(int x, int y, ref Battlefield PlayerBf)
        {
            int value = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!PlayerBf.RevealedFields[x-1+i,y - 1 + j])
                        value+=9;
                }
            }
			for (int offsetY = 0; offsetY < 2; offsetY++)
			{
				for (int offsetX = 0; offsetX < 3; offsetX++)
				{
					value += BombBorderValue(x, y, offsetX - 1, offsetY + 3, ref PlayerBf);
					value += BombBorderValue(x, y, offsetX - 1, offsetY - 3, ref PlayerBf);
				}
			}
			for (int offsetX = 0; offsetX < 2; offsetX++)
			{
				for (int offsetY = 0; offsetY < 3; offsetY++)
				{
					value += BombBorderValue(x, y, offsetX - 3, offsetY - 1, ref PlayerBf);
					value += BombBorderValue(x, y, offsetX + 3, offsetY - 1, ref PlayerBf);
				}
			}
					return value;
        }
		public int BombBorderValue(int x, int y, int offsetX, int offsetY, ref Battlefield PlayerBf)
		{
			int value = 0;
			if (PlayerBf.Size > x + offsetX && 0 < x + offsetX &&
				PlayerBf.Size > y + offsetY && 0 < y + offsetY)
				if (!PlayerBf.RevealedFields[x + offsetX, y + offsetY])
					value++;
			return value;
		}

		public void CheckboardShoot(ref Battlefield PlayerBf)
        {
            if (Checkboard.Count > 0)
            {
                while (PlayerBf.RevealedFields[Checkboard[0][0], Checkboard[1][1]])
                    Checkboard.RemoveAt(0);
                ShootField(ref PlayerBf, Checkboard[0], Weapons[0]);
                Checkboard.RemoveAt(0);
            }
        }
        public void findShipRotations(ref Battlefield PlayerBf)
        {
			foreach (int[] field in ShipFields)
			{
				if (field[0] != 0)
					if (PlayerBf.RevealedFields[field[0] - 1, field[1]] && 
						(PlayerBf.Field[field[0] - 1, field[1]] == 's' ||
						PlayerBf.Field[field[0] - 1, field[1]] == 'd'))
					{
						ShipRotations[field[0] - 1, field[1]] = 0;
						ShipRotations[field[0], field[1]] = 0;
					}
				if (field[1] != 0)
					if (PlayerBf.RevealedFields[field[0], field[1] - 1] &&
						(PlayerBf.Field[field[0], field[1]-1] == 's' ||
						PlayerBf.Field[field[0], field[1]-1] == 'd'))
					{
						ShipRotations[field[0], field[1] - 1] = 1;
						ShipRotations[field[0], field[1]] = 1;
					}
				if (field[1] != PlayerBf.Size - 1)
					if (PlayerBf.RevealedFields[field[0], field[1] + 1] &&
						(PlayerBf.Field[field[0], field[1]+1] == 's' ||
						PlayerBf.Field[field[0], field[1]+1] == 'd'))
					{
						ShipRotations[field[0], field[1]+1] = 1;
						ShipRotations[field[0], field[1]] = 1;
					}
				if (field[0] != PlayerBf.Size - 1)
					if (PlayerBf.RevealedFields[field[0] + 1, field[1]] &&
						(PlayerBf.Field[field[0] + 1, field[1]] == 's' ||
						PlayerBf.Field[field[0] + 1, field[1]] == 'd'))
					{
						ShipRotations[field[0] + 1, field[1]] = 0;
						ShipRotations[field[0], field[1]] = 0;
					}

			}
		}
        public bool NearShipShoot(ref Battlefield PlayerBf)
        {
            while (ShipFields.Count > 0)
            {
                if (PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1]] && PlayerBf.Field[ShipFields[0][0], ShipFields[0][1]] == 's')
                {
                    ShootField(ref PlayerBf, [ShipFields[0][0], ShipFields[0][1]], Weapons[0]);
                    return true;
                }
                if (ShipFields[0][0] != 0)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] - 1, ShipFields[0][1]] && ShipRotations[ShipFields[0][0], ShipFields[0][1]] != 1)
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] - 1, ShipFields[0][1]], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][1] != 0)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1]-1] && ShipRotations[ShipFields[0][0], ShipFields[0][1]] != 0)
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] , ShipFields[0][1]- 1], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][1] != PlayerBf.Size-1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1] + 1] && ShipRotations[ShipFields[0][0], ShipFields[0][1]] != 0)
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0], ShipFields[0][1] + 1], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][0] != PlayerBf.Size - 1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] + 1, ShipFields[0][1]] && ShipRotations[ShipFields[0][0], ShipFields[0][1]] != 1)
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] + 1, ShipFields[0][1]], Weapons[0]);
                        return true;
                    }
                ShipFields.RemoveAt(0);
            }
            return false;
        }
        public void ShootField(ref Battlefield battlefield, int[] location, Weapon weapon)
        {
            if (weapon.Uses == 0)
                return;
            weapon.Location = location;
            weapon.UpdateFields();
            if (!weapon.Destructive)
                foreach (int[] field in weapon.Fields)
                {
                    battlefield.RevealedFields[field[0], field[1]] = true;
                    if (battlefield.Field[field[0], field[1]] == 's')
                        ShipFields.Add(field);
                }
            else
                foreach (int[] field in weapon.Fields)
                {
                    if(battlefield.DestroyField(field))
                        ShipFields.Add(field);
                }
            weapon.Uses--;
        }
    }
}
