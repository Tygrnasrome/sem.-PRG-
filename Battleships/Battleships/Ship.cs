using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Ship
    {
        public int Size { get; set; }
        public bool Placed { get; set; }
        public int[] Location { get; set; }
        public int Rotation { get; set; }
		public int[,] ShipFields { get; set; }
		public bool ValidPlacement { get; set; }

        public Ship(int size, int[] location)
        {
            Size = size;
            Placed = false;
            Location = location;
			ShipFields = new int[size, 2];
			UpdateFields();
            Rotation = 0;
			ValidPlacement = true;
        }
		public void UpdateFields()
		{
			for (int i = 0; i < Size; i++)
			{
				switch (Rotation%2)
				{
					case 0:
						ShipFields[i, 0] = Location[0] + i;
						ShipFields[i, 1] = Location[1];
						break;
					case 1:
						ShipFields[i, 0] = Location[0];
						ShipFields[i, 1] = Location[1] + i;
						break;
				}
			}
		}
		public void Rotate(int battlefieldSize)
		{
			Rotation++;

			switch (Rotation % 2)
			{
				case 0:
					if (Location[0] + Size > battlefieldSize)
						Location[0] = battlefieldSize - Size;
					break;
				case 1:
					if (Location[1] + Size > battlefieldSize)
						Location[1] = battlefieldSize - Size;
					break;

			}
			UpdateFields();
		}
        public void MoveDown(int battlefieldHeight)
        {
            if ((Location[1] != battlefieldHeight - 1 && Math.Abs(Rotation%2) != 1) || 
				(Location[1] <= battlefieldHeight - 1 - Size && Rotation % 2 == 1))
            { 
				Location[1] += 1; 
				UpdateFields();
			}
        }
        public void MoveUp()
        {
            if ((Location[1] != 0 ) || (Location[1] >= 0+ Size))
			{
				Location[1] -= 1;
				UpdateFields();
			}
		}
        public void MoveRight(int battlefieldWidht)
        {
            if ((Location[0] != battlefieldWidht - 1 && Rotation % 2 == 1) || (Location[0]+Size <= battlefieldWidht - 1 && Rotation % 2 == 0))
			{
				Location[0] += 1;
				UpdateFields();
			}
		}
        public void MoveLeft()
        {
            if ((Location[0] != 0) || (Location[0] >= 0 + Size))
			{
				Location[0] -= 1;
				UpdateFields();
			}
		}

		public bool IsShipField(int[] location)
		{
			bool isShip = false;
			for (int i = 0; i < Size; i++) 
				if (ShipFields[i,0] == location[0] && ShipFields[i, 1] == location[1])
					isShip = true;
			return isShip;
		}
	}

}
