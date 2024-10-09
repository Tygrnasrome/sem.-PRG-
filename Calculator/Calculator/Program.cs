using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2024-2025.
 * Extended by students.
 */

namespace Calculator
{/*
             * ZADANI
             * Vytvor program ktery bude fungovat jako kalkulacka. Kroky programu budou nasledujici:
             * 1) Nacte vstup pro prvni cislo od uzivatele (vyuzijte metodu Console.ReadLine() - https://learn.microsoft.com/en-us/dotnet/api/system.console.readline?view=netframework-4.8.
             * 2) Zkonvertuje vstup od uzivatele ze stringu do integeru - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number.
             * 3) Nacte vstup pro druhe cislo od uzivatele a zkonvertuje ho do integeru. (zopakovani kroku 1 a 2 pro druhe cislo)
             * 4) Nacte vstup pro ciselnou operaci. Rozmysli si, jak operace nazves. Muze to byt "soucet", "rozdil" atd. nebo napr "+", "-", nebo jakkoliv jinak.
             * 5) Nadefinuj integerovou promennou result a prirad ji prozatimne hodnotu 0.
             * 6) Vytvor podminky (if statement), podle kterych urcis, co se bude s cisly dit podle zadane operace
             *    a proved danou operaci - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements.
             * 7) Vypis promennou result do konzole
             * 
             * Rozsireni programu pro rychliky / na doma (na poradi nezalezi):
             * 1) Vypis do konzole pred nactenim kazdeho uzivatelova vstupu co po nem chces (aby vedel, co ma zadat)
             * 2) a) Kontroluj, ze uzivatel do vstupu zadal, co mel (cisla, popr. ciselnou operaci). Pokud zadal neco jineho, napis mu, co ma priste zadat a program ukoncete.
             * 2) b) To same, co a) ale misto ukonceni programu opakovane cti vstup, dokud uzivatel nezada to, co ma
             *       - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#the-while-statement
             * 3) Umozni uzivateli zadavat i desetinna cisla, tedy prekopej kalkulacku tak, aby umela pracovat s floaty
             */

    internal class Program
    {
		static double[] values = {};
		static Relation[] relations = {};
		static string[] words = { };

		static Dictionary<string, double> variables = new Dictionary<string, double>();

		static bool newWordUsage = false;
		static void Main(string[] args)
        {
            Console.WriteLine("kalkulacka\n" );

            String input = "";


            do	//zadani vstupu dokud nebude spravny
            {
                Console.WriteLine("zadej priklad nebo nastav promennou");
                input = Console.ReadLine();
            } while (TryParseInput(input) && TryCalculate());


			Console.ReadKey();
        }
		static bool TryParseInput(string input)
		{
			//vrati true - neslo zpracovat 
			//false - pokracujeme k vypoctu
			char c;
			string word = "";
			string number = "";
			bool wordChange = false;


			int priority = 0; // jedna se pouze o prioritu zavorek, priorita matematickych operaci resi trida relation

			// mazani starych dat
			Array.Clear(Program.values, 0, Program.values.Length);
			Array.Clear(Program.relations, 0, Program.relations.Length);
			Array.Clear(Program.words, 0, Program.words.Length);

			//precte vstup a vrati true pokud byl vstup platny

			for (int i = 0; i < input.Length; i++)
			{
				//po znacich postupne precteme vstup
				//nejdriv testujeme cisla, relace, promenne + sqrt keyword,  
				c = input[i];
				
				if ((c >= 48 && c < 58) || c=='.')
				{//cislo
					Console.WriteLine($"number detect {word} {number} {c}");
					number += c;
					c = ' ';
				}else if (number!= "")
				{
					Program.values.Append(Convert.ToDouble(number));
					number = "";
				}
				wordChange = false;
				switch (c)
				{
					case '+': //pro vsechny matematicke operace krome sqrt
					case '-':
					case '*':
					case '/':
					case '^':
					case '=':
						string type = "";
						Program.relations.Append(new Relation(type += c, priority)); //nevim jak jednoduseji konvertovat char na string
						break;
					case '('://priorita zavorek
						priority += 2;
						break;
					case ')':
						priority -= 2;
						if (priority < 0)
						{ 
							Console.WriteLine("error: neuzavrene zavorky");
							return false;
						}
						break;
					case ' ': //ignoruj mezery a carky 
					case ',':
						break;
					default: //postupne sklada slovo ve string word
						word += c;
						wordChange = true;
						break;
				}
				if (!wordChange && word != "") //konec slova - ulozeni 
				{
					switch (word)
					{
						case "sqrt":
							Program.relations.Append(new Relation(word, priority));
							break;
						default: // pokud je nalezena promenna, tak se nahradi za cislo
							words.Append(word);
							if (!Program.variables.ContainsKey(word))
							{
								Console.WriteLine("nova promenna: " + word);
								newWordUsage = true;
							}
							else
								Program.values.Append(Program.variables[word]);
							break;
					}
					word = "";
				}
			}
            return true;
        }
		static bool TryCalculate()
		{
			foreach (string word in Program.words)
			{
				Console.WriteLine($"s - {word}");
			}
			foreach (double value in Program.values)
			{
				Console.WriteLine($"v - {value}");
			}
			//vrati true - neslo zpracovat 
			//false - vporadku

			//pokud nastavujeme hodnoty promennych
			bool doSetVariables = false;
			
			foreach (Relation relation in Program.relations) 
			{
				//musi byt obsazeno '='
				if(relation.Type == "=")
					doSetVariables = true;
			}
			if (doSetVariables)
			{
				double value = 0;
				//prirazujeme prvni hodnotu vsem novym promennym
				if (Program.values.Length >= 1)
					value = Program.values.First();
				else
				{
					Console.WriteLine("error: neni zadana hodnota");
					return true;
				}
				
				foreach (string word in Program.words)
				{
					Program.variables.Add(word, value);
				}
				Program.newWordUsage = false;
			}
			//pokud je vyuzita neznama promenna pripadne nespravny input
			if (Program.newWordUsage)
			{
				Console.WriteLine("error: neznama promenna - vyuziti neznamych znaku");
				Console.WriteLine("hodnoty do promennych se nastavuji:");
				Console.WriteLine("<nazev promenne> = <hodnota>");
				return false;
			}


			//todo
			//actual vypocet
			int maxPriority = 0;
			foreach (Relation relation in Program.relations)
				maxPriority = Math.Max(relation.Priority, maxPriority);

			for (int i = maxPriority; i > 0; i--)
			{
				for (int index = 0;index < Program.relations.Length;index++)
				{
					if (Program.relations[index].Priority == i)
					{
						if (Program.relations[index].ArgumentsNumber == 2)
						{
							Program.values[index] = Program.relations[index].Calculate(Program.values[index], Program.values[index + 1]);
							Program.values[index].
						}
						else
							Program.values[index+1] = Program.relations[index].Calculate(Program.values[index+1], 0);
					}
			}
			return true;
		}
	}
}
