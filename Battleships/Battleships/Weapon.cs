using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Battleships
{
	public abstract class Weapon
	{
		public abstract int[,] Offset { get; set; }
		public abstract int Rotation { get; set; }
		public abstract int[] Location { get; set; }
		public abstract int Uses { get; set; }
		public abstract List<int[]> Fields { get; set; }
        public abstract bool Destructive { get; set; }
        public abstract string Name { get; set; }
        protected Weapon()
		{
            Name = "general";
            Destructive = true;
			Fields = new List<int[]>();
			Offset = new int[2, 4];
			Location = new[] { 2, 2 };
			Rotation = 0;
		}
		public virtual void SetOffset(int[] offset)
		{
			int[] tmpOffset = new int[] {offset[0], offset[1], offset[2], offset[3]};
			for (int i = 0; i < 2; i++)
			{
				for (int rotation = 0; rotation < 4; rotation++)
				{
					Offset[i, rotation] = tmpOffset[(rotation-i+40)%4];
				}
			}
		}

		public abstract void UpdateFields();
        public virtual void MoveDown(int battlefieldHeight)
        {
            if ((Location[1] != battlefieldHeight - 1 - Offset[Rotation%2,1]))
            {
                Location[1] += 1;
                UpdateFields();
            }
        }
        public virtual void MoveUp()
        {
            if ((Location[1] -Offset[Rotation % 2, 3] > 0))
            {
                Location[1] -= 1;
                UpdateFields();
            }
        }
        public virtual void MoveRight(int battlefieldWidht)
        {
            if (Location[0] < battlefieldWidht - 1 - Offset[Rotation % 2, 0])
            {
                Location[0] += 1;
                UpdateFields();
            }
        }
        public virtual void MoveLeft()
        {
			if (Location[0] -Offset[Rotation % 2, 2] != 0)
			{
                Location[0] -= 1;
                UpdateFields();
            }
        }
        public virtual void Rotate(int battlefieldHeight, int battlefieldWidth)
        {
			if(Rotation%2 == 0 && Location[1] > battlefieldHeight - Offset[0,0]-1)
				Location[1] = battlefieldHeight - Offset[0, 0] - 1;
            if (Rotation % 2 == 1 && Location[0] > battlefieldHeight - Offset[1, 1]-1)
                Location[0] = battlefieldHeight - Offset[0, 0] - 1;
            Rotation++;
			UpdateFields();
        }
		public virtual void DrawWeapon(int[] location)
		{
			if (Uses == 0)
				return;
			Console.ForegroundColor = ConsoleColor.DarkBlue;
			foreach (int[] field in Fields)
			{
				Console.SetCursorPosition(field[0]*2+1+location[0], field[1]+location[1] + 1);
				Console.Write("██");
			}
			Console.ResetColor();
		}
    }
}
