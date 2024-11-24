using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Battlefield
    {
        public int Size { get; set; }
        public int[] Location { get; set; }
        public char[,] Field { get; set; }
        public bool Active { get; set; }
        static bool Valid_tiles { get; set; }
        public Ship SelectedShip { get; set; }
        static Ship[] Ships { get; set; }
        public Battlefield(int size, int[] location)
        {
            Field = new char[size,size];
            Size = size;
            Active = false;
            Valid_tiles = true;
            Location = location;
            Ships = new Ship[6] {new(5, [0, 0]), new(4, [0, 0]), new(3, [0, 0]), new(2, [0, 0]), new(1, [0, 0]), new(1, [0, 0]) };
            SelectedShip = Ships[0];
            // nastavim bitevni pole na prazdna
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Field[i, j] = ' ';
                }
            }
            Field[4, 4] = 's';
        }
        public void DrawBattlefield(int[] selectedTile)
        {
            Console.SetCursorPosition(Location[0], Location[1]);
            Console.Write("┌");
            for (int i = 0; i < Size * 2; i++)
                Console.Write("─");
            Console.Write("┐");
            Console.WriteLine();
            for (var col = 0; col < Size; col++)
            {
                Console.SetCursorPosition(Location[0], Location[1] + col + 1);
                Console.Write("|");
                for (var row = 0; row < Size; row++)
                {
                    DrawField([col, row], selectedTile[0] == row && selectedTile[1] == col && Active);
                    //DrawField([col, row], selectedTile[0] == row && selectedTile[1] == col && Active);
                }
                //Console.Write("|\n");
                Console.Write("|\n");
            }
            Console.SetCursorPosition(Location[0], Location[1] + Size+1);
            Console.Write("└");
            for (int i = 0; i < Size * 2; i++)
                Console.Write("─");
            Console.Write("┘");
            Console.WriteLine();
        }
        public void DrawField(int[] location, bool is_tmp)
        {
            if (is_tmp)
                switch (Field[location[0], location[1]])
                {
                    default: // tmp tiles
                        bool validPlaced = true; 
                        for (int i = 0;i < SelectedShip.Size;i++)
                            switch (SelectedShip.Rotation%4)
                            {
                                case 0:
                                    if (!IsValidPlaced([location[0] + i, location[1]]))
                                        validPlaced = false; break;
                                case 1:
                                    if (!IsValidPlaced([location[0], location[1] + i]))
                                        validPlaced = false; break;
                                case 2:
                                    if (!IsValidPlaced([location[0] - i, location[1]]))
                                        validPlaced = false; break;
                                case 3:
                                    if (!IsValidPlaced([location[0], location[1] - i]))
                                        validPlaced = false; break;
                            }

                        if (validPlaced)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Valid_tiles = false;
                        }
                        break;

                    case 's': // error tmp tiles - cant be placed
                        Console.ForegroundColor = ConsoleColor.Red;
                        Valid_tiles = false;
                        break;
                }
            else
                switch (Field[location[0], location[1]])
                {
                    case ' ': // empty tiles
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case 's': // ship tiles
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 'd': // destroyed ship tiles
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
            Console.Write("██");
            Console.ResetColor();
        }
        public bool IsValidPlaced(int[] location)
        {
            if (location[0] != Size)
                if (Field[location[0] + 1, location[1]] == 's')
                    return false;
            if (location[1] != Size)
                if (Field[location[0], location[1] + 1] == 's')
                    return false;
            if (location[0] != 0)
                if (Field[location[0] - 1, location[1]] == 's')
                    return false;
            if (location[1] != 0)
                if (Field[location[0], location[1] - 1] == 's')
                    return false;
            return true;
        }

    }
}
