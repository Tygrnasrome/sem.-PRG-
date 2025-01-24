using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
	internal class Cannon : Weapon
	{
		public override int Rotation { get; set; }
		public override int[,] Offset { get; set; }
		public override int[] Location { get; set; }
		public override int Uses { get; set; }
		public override List<int[]> Fields { get; set; }
        public override bool Destructive { get; set; }
        public override string Name { get; set; }
        public Cannon()
		{
			Name = "Delo (1x1)";
            Destructive = true;
            Uses = -1;
			Fields = new List<int[]>();
			Offset = new int[2, 4];
			SetOffset([0, 0, 0, 0]);
			Location = new[] { 0, 0 };
			Rotation = 0;
			UpdateFields();
		}
		public override void UpdateFields()
		{
			Fields.Clear();
			Fields.Add([Location[0], Location[1]]);
		}
	}
}
