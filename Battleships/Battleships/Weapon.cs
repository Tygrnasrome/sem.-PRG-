using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Weapon
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }
        public int[] Location { get; set; }
        public Weapon(int[] location, int w, int h) 
        {
            Location = location;
            Width = w;
            Height = h;
            Rotation = 0;
        }
        public void MoveDown(int battlefieldHeight)
        {
            if ((Location[1] != battlefieldHeight - 1 && Math.Abs(Rotation % 2) != 1) ||
                (Location[1] <= battlefieldHeight - 1 - Size && Rotation % 2 == 1))
            {
                Location[1] += 1;
                UpdateFields();
            }
        }
        public void MoveUp()
        {
            if ((Location[1] != 0) || (Location[1] >= 0 + Size))
            {
                Location[1] -= 1;
                UpdateFields();
            }
        }
        public void MoveRight(int battlefieldWidht)
        {
            if ((Location[0] != battlefieldWidht - 1 && Rotation % 2 == 1) || (Location[0] + Size <= battlefieldWidht - 1 && Rotation % 2 == 0))
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
        public void Rotate()
        {
            Rotation++;
        }
        public void Shoot()
        {
            //TODO
        }
    }
}
