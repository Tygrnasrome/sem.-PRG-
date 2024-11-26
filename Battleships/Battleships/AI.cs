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
                    NormalAI(ref PlayerBf);
                    break;
                case 3:
                    HardAI(ref PlayerBf);
                    break;
            }
        }
        public void VeryEasyAI(ref Battlefield PlayerBf)
        {
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
        public void NormalAI(ref Battlefield PlayerBf)
        {
            if(NearShipShoot(ref PlayerBf))return;
            CheckboardShoot(ref PlayerBf);
        }
        public void HardAI(ref Battlefield PlayerBf)
        {
            findShipRotations(ref PlayerBf);
            if (RandomSpecialWeaponShoot(ref PlayerBf, Weapons[1])) return;
            if (RandomSpecialWeaponShoot(ref PlayerBf, Weapons[3])) return;
            if (NearShipShoot(ref PlayerBf)) return;
            CheckboardShoot(ref PlayerBf);
        }
        public bool RandomSpecialWeaponShoot(ref Battlefield PlayerBf, Weapon weapon)
        {
            if (weapon.Uses == 0)
                return false;
            int x, y;
            Random rand = new();
            x = rand.Next(1, PlayerBf.Size-1);
            y = rand.Next(1, PlayerBf.Size-1);
            while (BombValue(x,y, ref PlayerBf) < 8)
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
                        value++;
                }
            }
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
            // TODO
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
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] - 1, ShipFields[0][1]])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] - 1, ShipFields[0][1]], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][1] != 0)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1]-1])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] , ShipFields[0][1]- 1], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][1] != PlayerBf.Size-1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1] + 1])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0], ShipFields[0][1] + 1], Weapons[0]);
                        return true;
                    }
                if (ShipFields[0][0] != PlayerBf.Size - 1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] + 1, ShipFields[0][1]])
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
