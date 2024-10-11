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
					Priority++;
					break;
				case "sin":
				case "cos":
				case "tan":
				case "log":
				case "^":
				case "sqrt":
				case "abs":
				case "binary":
					Priority+= 2;
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
					return Math.Sqrt(b);
				case "abs":
					return Math.Abs(b);
				case "log":
					return Math.Log(b,a);
				case "sin":
					return Math.Sin(b);
				case "cos":
					return Math.Cos(b);
				case "tan":
					return Math.Tan(b);
				case "binary":
					return Convert.ToDouble(NumSystemConvertFromDecimal(2, Convert.ToInt32(b)));
				case "numSystem":
					return Convert.ToDouble(NumSystemConvertFromDecimal(Convert.ToInt32(a), Convert.ToInt32(b)));
				
				default:
					throw new NotImplementedException();
			}

		}
		static string NumSystemConvertFromDecimal(int numSystem, int decimalNumber)
		{
			//nefunguje na desetinna cisla a numSystem > 9
			// from stack overflow https://stackoverflow.com/questions/2954962/convert-integer-to-binary-in-c-sharp
			if (numSystem > 9)
				return "";
			int remainder;
			string result = string.Empty;
			while (decimalNumber > 0)
			{
				remainder = decimalNumber % numSystem;
				decimalNumber /= numSystem;
				result = remainder.ToString() + result;
			}
			return result;
		}
	}
}
