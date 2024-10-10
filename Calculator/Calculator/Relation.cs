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

		public int ArgumentsNumber { get; set; }
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
					Priority++;
					break;
				case "sqrt":
				case "abs":
					Priority+= 2;
					break;
			}
			if(Type == "sqrt")
			{
				ArgumentsNumber = 1;
			}else
				ArgumentsNumber = 2;
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
					return Math.Sqrt(b);
				case "abs":
					return Math.Abs(b);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
