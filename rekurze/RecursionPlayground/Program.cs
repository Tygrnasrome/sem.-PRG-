using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace RecursionPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
			int factorial, n, fibonacci;
			while (true)
			{
				Console.WriteLine("zadej n:");
				int.TryParse(Console.ReadLine(),out n); // Nacteme cislo n, pro ktere budeme pocitat jeho faktorial a n-ty prvek Fibonacciho posloupnosti.
				factorial = Factorial(n); // Prvni zavolani pro vypocet faktorialu, ulozeni do promenne factorial.
				fibonacci = Fibonacci(n); // Prvni zavolani pro vypocet Fibonacciho posloupnosti, ulozeni do promenne fibonacci.
				Console.WriteLine($"Pro cislo {n} je faktorial {factorial} a {n}. prvek Fibonacciho posloupnosti je {fibonacci}"); // Vypsani vysledku uzivateli.
			}
        }

        static int Factorial(int n)
        {
            if(n<1) return 1;
            return n*Factorial(n-1); 
        }

        static int Fibonacci(int n)
        {
			if (n < 3) return 1;
			return Fibonacci(n-1)+ Fibonacci(n - 2); 
        }
    }
}
