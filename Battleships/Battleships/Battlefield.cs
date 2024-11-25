using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
		public int SelectedShipNum { get; set; }
        private Ship[] Ships { get; set; }
		public bool[,] RevealedFields { get; set; }
		public Battlefield(int size, int[] location)
        {
            Field = new char[size,size];
			RevealedFields = new bool[size, size];
			Size = size;
            Active = false;
            Valid_tiles = true;
            Location = location;
            Ships = new Ship[5] {new(5, [0, 0]), new(4, [0, 0]), new(3, [0, 0]), new(3, [0, 0]), new(2, [0, 0])};
            SelectedShip = Ships[0];
			SelectedShipNum = 0;
			// nastavim bitevni pole na prazdna
			for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Field[i, j] = ' ';
                }
            }
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					RevealedFields[i, j] = false;
				}
			}
		}
        public void DrawBattlefield(string headline,bool selecting = true, bool revealed = true)
        {
			SelectedShip.ValidPlacement = true;
            Console.SetCursorPosition(Location[0], Location[1]);
            Console.Write("┌");
			Console.Write(headline);
            for (int i = 0; i < Size * 2 - headline.Length; i++)
                Console.Write("─");
            Console.Write("┐");
            Console.WriteLine();
            for (var y = 0; y < Size; y++)
            {
                Console.SetCursorPosition(Location[0], Location[1] + y + 1);
                Console.Write("|");
                for (var x = 0; x < Size; x++)
                {
                    DrawField([x, y], SelectedShip.IsShipField([x, y]) && selecting, revealed);
                }
                Console.Write("|\n");
            }
            Console.SetCursorPosition(Location[0], Location[1] + Size+1);
            Console.Write("└");
            for (int i = 0; i < Size * 2; i++)
                Console.Write("─");
            Console.Write("┘");
            Console.WriteLine();
        }
        public void DrawField(int[] location, bool is_tmp, bool revealed)
        {
			if (!revealed && !RevealedFields[location[0], location[1]])
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("░░");
				Console.ResetColor();
				return;
			}
			if (is_tmp)
                switch (Field[location[0], location[1]])
                {
                    default: // tmp tiles
						Valid_tiles = true;
						if (!IsValidPlaced([location[0], location[1]]))
							Valid_tiles = false;

						if (Valid_tiles)
							Console.ForegroundColor = ConsoleColor.Green;
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							SelectedShip.ValidPlacement = false;
						}
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
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                }

            Console.Write("██");
            Console.ResetColor();
        }
        public bool IsValidPlaced(int[] location)
        {
			if (Field[location[0], location[1]] == 's')
				return false;
			if (location[0] != Size-1)
                if (Field[location[0] + 1, location[1]] == 's')
                    return false;
            if (location[1] != Size-1)
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
		public void PlaceSelectedShip()
		{
			if (SelectedShip.ValidPlacement == false)
				return; 

			for (int i = 0; i < SelectedShip.Size; i++)
			{
				Field[SelectedShip.ShipFields[i,0], SelectedShip.ShipFields[i, 1]] = 's';
			}
			for (int i = 0; i < Ships.Length; i++)
			{
				if (i == SelectedShipNum)
				{
					SelectedShip.Placed = true;
					Ships[i] = SelectedShip;
				}
			}
			SelectNextShip();
		}
		public void SelectNextShip()
		{
			for (int i = 0; i < Ships.Length; i++)
			{
				if (!Ships[i].Placed)
				{
					SelectedShip = Ships[i];
					SelectedShipNum = i;
					return;
				}
			}
		}
		public bool	AllShipsPlaced()
		{

			for (int i = 0; i < Ships.Length; i++)
			{
				if (Ships[i].Placed == false) 
					return false;
			}
			return true;
		}
		public bool AllShipsDestroyed()
		{
			foreach (var field in Field)
			{
				if (field == 's')
					return false;
			}
			return true;
		}
		public void PlaceAllShips()
		{
			int x, y, rotation;
			Random rand = new Random();
			while (!AllShipsPlaced())
			{
                for (int i = 0; i < Ships.Length; i++)
                {
					if (Ships[i].Placed)
						continue;
					rotation = rand.Next(2);
					switch (rotation)
					{
						case 0:
							x = rand.Next(Size - Ships[i].Size);
							y = rand.Next(Size);
							break;
						case 1:
							x = rand.Next(Size);
							y = rand.Next(Size - Ships[i].Size);
							break;
						default:
							x = 0;
							y = 0;
							break;
					}
					SelectedShipNum = i;
					Ships[i].Location = [x, y];
					Ships[i].Rotation = rotation;
					SelectedShip = Ships[i];
					SelectedShip.UpdateFields();
					bool validPlaced = true;
					for (int j = 0; j < SelectedShip.Size; j++)
					{
						if (!IsValidPlaced([SelectedShip.ShipFields[j, 0], SelectedShip.ShipFields[j, 1]]))
							validPlaced = false;
					}
					if (validPlaced)
					{
						SelectedShip.ValidPlacement = true;
						PlaceSelectedShip();
					}

				}
			}
		}
		public void DestroyField(int[] location)
		{
			if (Field[location[0], location[1]] == 's')
			{
				Field[location[0], location[1]] = 'd';
			}
			RevealedFields[location[0], location[1]] = true;
		}
		public void DrawSelectedField(int[] location)
		{
			Console.SetCursorPosition(location[0]*2 + Location[0] + 1, location[1] + Location[1] + 1);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("██");
			Console.ResetColor();
		}
	}
}
