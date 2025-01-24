using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPlayground
{
	internal class Rectangle
	{
		int w {  get; set; }
		int h { get; set; }
		public Rectangle(int w, int h)
		{
			this.w = w;
			this.h = h;
			if (w <= 0) this.w = 1;
			if (h <= 0) this.h = 1;
		}
		public int CalculateArea() 
		{ 
			int area = w*h;
			Console.WriteLine("Area is: " + area);
			return area; 
		}
		public bool ContainsPoint(int x, int y)
		{
			bool contained = x < w && y < h;
			if (contained)
				Console.WriteLine("Point ["+ x+","+y +"] is in rectangle");
            else
				Console.WriteLine("Point ["+ x+","+y +"] is not in rectangle");
            return contained;
		}
		public float CalculateAspectRation()
		{
			float aspect = w/h;
			if (aspect > 1)
				Console.WriteLine("Rectangle is wide");
			else if (aspect != 1)
				Console.WriteLine("Rectangle is high");
			else 
				Console.WriteLine("Its a square");


			return aspect;
		}

	}
}
