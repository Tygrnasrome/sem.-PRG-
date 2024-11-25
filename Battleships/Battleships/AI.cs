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
        public List<int[]> Checkboard { get; set; }


        public AI(int size) 
        {
            ShipFields = new List<int[]>();
            Checkboard = new List<int[]>();
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
            if(NearShipShoot(ref PlayerBf))
                return;

            CheckboardShoot(ref PlayerBf);

        }
        public void CheckboardShoot(ref Battlefield PlayerBf)
        {
            if (Checkboard.Count > 0)
            {
                while (PlayerBf.RevealedFields[Checkboard[0][0], Checkboard[1][1]])
                    Checkboard.RemoveAt(0);
                ShootField(ref PlayerBf, Checkboard[0]);
                Checkboard.RemoveAt(0);
            }
        }
        public bool NearShipShoot(ref Battlefield PlayerBf)
        {
            while (ShipFields.Count > 0)
            {
                if (ShipFields[0][0] != 0)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] - 1, ShipFields[0][1]])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] - 1, ShipFields[0][1]]);
                        return true;
                    }
                if (ShipFields[0][1] != 0)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1]-1])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] , ShipFields[0][1]- 1]);
                        return true;
                    }
                if (ShipFields[0][1] != PlayerBf.Size-1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0], ShipFields[0][1] + 1])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0], ShipFields[0][1] + 1]);
                        return true;
                    }
                if (ShipFields[0][0] != PlayerBf.Size - 1)
                    if (!PlayerBf.RevealedFields[ShipFields[0][0] + 1, ShipFields[0][1]])
                    {
                        ShootField(ref PlayerBf, [ShipFields[0][0] + 1, ShipFields[0][1]]);
                        return true;
                    }
                ShipFields.RemoveAt(0);
            }
            return false;
        }
        public void ShootField(ref Battlefield PlayerBf, int[] location)
        {
            PlayerBf.DestroyField(location);
            if (PlayerBf.Field[location[0], location[1]] == 'd')
                ShipFields.Add(location);
        }
    }
}
