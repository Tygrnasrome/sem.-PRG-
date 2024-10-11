using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
		//static double[] values = {};
		//static Relation[] relations = {};
		//static string[] words = { };

		static List<double> values = new List<double>();
		static List<Relation> relations = new List<Relation>();
		static List<string> words = new List<string>();
		static Dictionary<string, double> variables = new Dictionary<string, double>();

		static bool newWordUsage = false;
		static bool keepRunning = true;
		static void Main(string[] args)
        {
            Console.WriteLine("Kalkulacka\n");
			Console.WriteLine("Pro seznam operaci napiste \"help\", pro vypis promennych napiste \"var\"\n");

			variables.Add("PI", Math.PI);

            String input = "";
			bool isValidInput = false;
			while (keepRunning)
			{
				do  //zadani vstupu dokud nebude spravny
				{
					Console.WriteLine("Zadej priklad nebo nastav promennou");
					input = Console.ReadLine();
					ClearConsole(input);

					isValidInput = !TryParseInput(input) || !TryCalculate();
					
				} while (isValidInput);
			}

			Console.ReadKey();
        }
		static void ClearConsole(string input)
		{
			//vymaze konsoli
			Console.Clear();
			Console.WriteLine("Kalkulacka\n");
			Console.WriteLine("Pro seznam operaci napiste \"help\", pro vypis promennych napiste \"var\"\n");
			Console.WriteLine(input);
		}
		static bool TryParseInput(string input)
		{
			//vrati true - neslo zpracovat 
			//false - pokracujeme k vypoctu
			char c;
			string word = "";
			string number = "";
			bool wordChange = false;
			bool isMinusNum = false;
			bool wasRelationLast = true;

			int priority = 0; // jedna se pouze o prioritu zavorek, priorita matematickych operaci resi trida relation

			// mazani starych dat
			Program.values.Clear();
			Program.relations.Clear();
			Program.words.Clear();

			//precte vstup a vrati true pokud byl vstup platny

			for (int i = 0; i < input.Length; i++)
			{
				//po znacich postupne precteme vstup
				//nejdriv testujeme cisla, relace, promenne + sqrt keyword,  
				c = input[i];
				
				if ((c >= 48 && c < 58) || c==',')
				{//cislo
					//pokud pred cislem byl minus, ktery neni operace
					if(isMinusNum)
					{
						number += '-';
						isMinusNum = false;
					}
					wasRelationLast = false;
					number += c;
					c = ' ';
				}else if (number!= "")
				{
					Program.values.Add(Convert.ToDouble(number));
					number = "";
					isMinusNum = false;
				}
				
					
				wordChange = false;
				switch (c)
				{
					case '+': //pro vsechny matematicke operace krome sqrt
					case '*':
					case '/':
					case '^':
					case '=':
						Program.relations.Add(new Relation(Convert.ToString(c), priority)); 
						wasRelationLast = true;
						break;
					case '('://ignorace zavorek a minusu (musi byt na konci)
					case ')':
					case '-':
					case ' ': //ignoruj mezery 
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
						case "sin":
						case "cos":
						case "tan":
						case "abs":
						case "binary":
							Program.relations.Add(new Relation(word, priority));
							Program.values.Add(0);
							break;
						case "log":
						case "numSystem":
							Program.relations.Add(new Relation(word, priority));
							break;
						case "help":
							WriteHelp();
							words.Add(word);
							break;
						case "var":
							WriteVariables();
							words.Add(word);
							break;
						default: // pokud je nalezena promenna, tak se nahradi za cislo
							words.Add(word);
							if (!Program.variables.ContainsKey(word))
								newWordUsage = true;
							else
							{
								if(isMinusNum)
									Program.values.Add(-Program.variables[word]);
								else
									Program.values.Add(Program.variables[word]);
								wasRelationLast = false;
								isMinusNum = false;
							}

							break;
					}
					word = "";
				}
				switch (c)
				{
					case '-':
						//pokud je minus pred cislem, tak je cislo a ne operace
						if (wasRelationLast)
							isMinusNum = true;
						else
							Program.relations.Add(new Relation(Convert.ToString(c), priority));
						break;
					case '('://priorita zavorek NUTNO na konci 
						priority += 3;
						break;
					case ')':
						priority -= 3;
						if (priority < 0)
						{
							Console.WriteLine("> error: neuzavrene zavorky");
							return false;
						}
						break;
				}
			}


			//pokud skoncil input, ale je to konec slova nebo cisla
			if (number != "")
			{
				Double value = 0;
				if (!Double.TryParse(number, out value))
					Double.Parse(number, System.Globalization.NumberStyles.AllowDecimalPoint);

				Program.values.Add(value);
			}
			if (word != "")
			{
				switch (word)
				{
					case "help":
						WriteHelp();
						words.Add(word);
						break;
					case "var":
						WriteVariables();
						words.Add(word);
						break;
					default: // pokud je nalezena promenna, tak se nahradi za cislo
						words.Add(word);
						if (!Program.variables.ContainsKey(word))
						{
							Console.WriteLine("nova promenna: " + word);
							newWordUsage = true;
						}
						else
							Program.values.Add(Program.variables[word]);
						break;
				}
			}
				

			return true;
        }
		static bool TryCalculate()
		{
			{ 
			/* developer print
			foreach (string word in Program.words)
			{
				Console.WriteLine($"word: {word}");
			}
			foreach (double value in Program.values)
			{
				Console.WriteLine($"value: {value}");
			}
			foreach (Relation relation in Program.relations)
			{
				Console.WriteLine($"Relation: {relation.Type}, prio: {relation.Priority}");
			}
			*/
			}
			//vrati true - neslo zpracovat 
			//false - vporadku

			//v pripade ze neni zadana hodnota - error
			if (Program.values.Count == 0)
			{
				bool errorMessage = true;
				foreach (string word in Program.words)
				{
					//nevypise error, pokud je zadan help nebo var
					if(word == "help" || word == "var")
						errorMessage = false;
				}
				if (errorMessage)
				{
					Console.WriteLine("> error: nespravny vstup\n->napis \"help\" - pro vice informaci\n");
				}
				return true;
			}
			bool output;
			if(VarSetLogic(out output))
				return output;

			//realny vypocet
			return Calculation();
			
		}
		
		static bool VarSetLogic(out bool output)
		{
			//vraci true, pokud je dulezity output
			//pri false neni output vyuzit

			//nastaveni hodnot promennych
			bool doSetVariables = false;

			foreach (Relation relation in Program.relations)
			{
				//musi byt obsazeno '='
				if (relation.Type == "=")
					doSetVariables = true;
			}
			if (doSetVariables)
			{
				double value = 0;
				//prirazujeme prvni hodnotu vsem novym promennym
				if (Program.values.Count >= 1)
					value = Program.values.First();
				else
				{
					Console.WriteLine("> error: neni zadana hodnota");
					output = true;
					return true;
				}

				foreach (string word in Program.words)
				{
					Console.WriteLine($"> hodnota {word} nastavena na: {value}\n");
					Program.variables.Add(word, value);
				}
				Program.newWordUsage = false;
				output = false;
				return true;
			}
			//pokud je vyuzita neznama promenna pripadne nespravny input
			if (Program.newWordUsage)
			{
				Console.WriteLine("> error: neznama promenna - vyuziti neznamych znaku");
				Console.WriteLine("> hodnoty do promennych se nastavuji:");
				Console.WriteLine("> <nazev promenne> = <hodnota>\n");
				Program.newWordUsage = false;
				output = true;
				return true;
			}
			output = false;
			return false;
		}
		static bool Calculation()
		{
			//vypocet	
			int maxPriority = 0;
			foreach (Relation relation in Program.relations)
			{
				maxPriority = Math.Max(relation.Priority, maxPriority);
			}
			for (int i = maxPriority; i > -1; i--)
			{
				for (int index = 0; index < Program.relations.Count; index++)
				{
					if (Program.relations[index].Priority == i)
					{
						if (Program.values.Count < index+2)
						{
							Console.WriteLine("> error: nespravny vstup (moc argumentu na malo cisel)\n->napis \"help\" - pro vice informaci\n");
							return true;
						}
						//pouzije okolni dve hodnoty pro vypocet a vysledek ulozi do prvni
						//druhou hodnotu pak vymaze
						Program.values[index] = Program.relations[index].Calculate(Program.values[index], Program.values[index + 1]);
						Program.values.RemoveAt(index + 1);
						//nakonec vymaze prvek z listu relaci
						Program.relations.RemoveAt(index);
						index--;
					}
				}
			}
			if (Program.values.Count == 0)
			{
				Console.WriteLine("> error: nespravny vstup\n");
				WriteHelp();
				return true;
			}

			Console.WriteLine("= " + Program.values[0]);
			return false;
		}

		

		static void WriteHelp()
		{
			Console.WriteLine("\n---------------- Help ----------------");
			Console.WriteLine("Priklad zadejte bez =");
			Console.WriteLine("Povolene operace: \n+, -, *, /, ^, sqrt(x), abs(x), log(baze,x), sin(x), cos(x), tan(x)");
			Console.WriteLine("Goniometricke fce jsou v radianech");
			Console.WriteLine("binary(x) - prevede x do dvojkove soustavy\nnumSystem(base,x) - prevede x do libovolne soustavy o zakladu base 1-9");
			Console.WriteLine("Prevody systemu jsou fce a nepodporuji matematicke operace");
			Console.WriteLine("Hodnoty do promennych se nastavuji:");
			Console.WriteLine("<nazev promenne> = <hodnota>");
			Console.WriteLine("Pro vypis promennych napiste \"var\"");
			Console.WriteLine("--------------------------------------\n");
		}
		static void WriteVariables()
		{
			Console.WriteLine("\n---------------- Promenne ----------------");
			Console.WriteLine("Ulozene promenne: ");
			foreach (var variable in Program.variables)
			{
				Console.WriteLine(variable.Key + ": " + variable.Value);
			}
			Console.WriteLine("\n");
			Console.WriteLine("------------------------------------------\n");
		}
	}
}
