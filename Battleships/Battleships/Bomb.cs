using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Bomb : Weapon
    {
        public override int Rotation { get; set; }
        public override int[,] Offset { get; set; }
        public override int[] Location { get; set; }
        public override int Uses { get; set; }
        public override List<int[]> Fields { get; set; }
        public override bool Destructive { get; set; }
        public override string Name { get; set; }
        public Bomb(int useCount)
        {
            Name = "Bomba (3x3)";
            Destructive = true;
            Uses = useCount;
            Fields = new List<int[]>();
            Offset = new int[2, 4];
            SetOffset([1, 1, 1, 1]);
            Location = new[] { 2, 2 };
            Rotation = 0;
        }
        public override void UpdateFields()
        {
            Fields.Clear();
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Fields.Add([Location[0] - 1 + x, Location[1] - 1 + y]);
                }
            }
        }

        public void Shoot(ref Battlefield battlefield)
        {
            foreach (int[] field in Fields)
            {
                battlefield.DestroyField(field);
            }
        }

    }
}
