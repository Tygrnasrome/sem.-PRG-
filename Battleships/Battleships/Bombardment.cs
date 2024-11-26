using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
	internal class Bombardment : Weapon
	{
		public override int Rotation { get; set; }
		public override int[,] Offset { get; set; }
		public override int[] Location { get; set; }
		public override int Uses { get; set; }
		public override List<int[]> Fields { get; set; }
		public Bombardment(int useCount)
		{
			Uses = useCount;
			Fields = new List<int[]>();
			Offset = new int[2, 4];
			SetOffset([4, 0, 0, 0]);
			Location = new[] { 2, 2 };
			Rotation = 0;
		}
		public override void UpdateFields()
		{
			Fields.Clear();
			for (int i = 0; i < 5; i++)
			{
				if(Rotation%2 ==  0)
					Fields.Add([Location[0] + i, Location[1]]);
				else
					Fields.Add([Location[0], Location[1] + i]);
			}
		}

		public void Shoot(ref Battlefield battlefield)
		{
			if(Uses == 0) return;
			foreach (int[] field in Fields)
			{
				battlefield.DestroyField(field);
			}
			Uses--;
		}
	}
}
