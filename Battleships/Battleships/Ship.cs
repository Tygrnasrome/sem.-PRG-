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
        static bool Placed { get; set; }
        private int[] Location { get; set; }
        public int Rotation { get; set; }

        public Ship(int size, int[] location)
        {
            Size = size;
            Placed = false;
            Location = location;
            Rotation = 0;
        }

        public void MoveDown(int battlefieldHeight)
        {
            if ((Location[1] != battlefieldHeight - 1 && Rotation%4 != 1) || (Location[1] <= battlefieldHeight - 1 - Size && Rotation % 4 == 1))
                Location[1] += 1;
        }
        public void MoveUp()
        {
            if ((Location[1] != 0 && Rotation % 4 != 3) || (Location[1] >= 0+ Size && Rotation % 4 == 3))
                Location[1] -= 1;
        }
        public void MoveRight(int battlefieldWidht)
        {
            if ((Location[0] != battlefieldWidht - 1 && Rotation % 4 != 0) || (Location[0] <= battlefieldWidht - 1 - Size && Rotation % 4 == 0))
                Location[0] += 1;
        }
        public void MoveLeft()
        {
            if ((Location[0] != 0 && Rotation % 4 != 2) || (Location[0] >= 0 + Size && Rotation % 4 == 2))
                Location[0] -= 1;
        }
    }
}
