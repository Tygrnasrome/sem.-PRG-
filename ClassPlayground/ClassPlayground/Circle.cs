using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPlayground
{
	internal class Circle
	{
		int r {  get; set; }
		public Circle(int r)
		{
			this.r = r;
			if (r <= 0) this.r = 1;
		}
		public double CalculateArea()
		{
			double area = r*r*Math.PI;
			Console.WriteLine("Area is: " + area);
			return area;
		}
		public bool ContainsPoint(int x, int y)
		{
			bool contained = Math.Sqrt(Math.Pow(x,2)+ Math.Pow(y, 2)) <= this.r;
			if (contained)
				Console.WriteLine("Point [" + x + "," + y + "] is in circle");
			else
				Console.WriteLine("Point [" + x + "," + y + "] is not in circle");
			return contained;
		}
	}
}
