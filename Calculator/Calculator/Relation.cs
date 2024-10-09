using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
	internal class Relation
	{	
		public string Type { get; set; }
		public int Priority { get; set; }
		public Relation(string type, int priority)
		{
			Type = type;
			//u nasobeni a deleni jde priorita o 1 vys
			Priority = priority;
			switch (Type)
			{
				case "/":
				case "*":
				case "^":
				case "sqrt":
					Priority++;
					break;
			}
		}
		public double Calculate(double a,  double b)
		{
			switch (Type)
			{
				case "+":
					return a + b;
				case "-":
					return a - b;
				case "/":
					return a / b;
				case "*":
					return a * b;
				case "^":
					return Math.Pow(a, b);
				case "sqrt":
					return Math.Sqrt(a);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
