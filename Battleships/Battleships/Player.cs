using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Battleships
{
	internal class Player
	{
		public List<Weapon> Weapons { get; set; }
		public Weapon SelectedWeapon { get; set; }
		public Player(List<Weapon> weapons) 
		{ 
			Weapons = weapons;
			SelectedWeapon = weapons[0];
		}
		public bool Shoot(int[] location, ref Battlefield battlefield)
		{
			if (SelectedWeapon.Uses ==0)
				return false;
			foreach (int[] field in SelectedWeapon.Fields)
			{
				battlefield.DestroyField(field);
			}
			SelectedWeapon.Uses--;
			if (SelectedWeapon.Uses == 0)
				ChangeWeapon(0);
			return true;
		}

		public void ChangeWeapon(int number)
		{
			if (number >= 0 && number < Weapons.Count)
			{
				if(Weapons[number].Uses != 0)
					SelectedWeapon = Weapons[number];
			}
			SelectedWeapon.UpdateFields();
		}
	}
}
