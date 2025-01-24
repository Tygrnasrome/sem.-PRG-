using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
/* 3) Vytvoř třídu Student, kterou budeme reprezentovat studenta
 *    Přidej třídě Student proměnné - year pro aktuální ročník studenta
 *                                  - id pro identifikační číslo studenta
 *                                  - subjects pro seznam předmětů studenta (bude to slovník (https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/),
 *                                    který bude mít jako klíč string a jako hodnotu List (https://www.geeksforgeeks.org/c-sharp-list-class/) známek)
 *                                  - name pro jméno studenta
 *    Přidej třídě Student čtyři funkce - AddSubject, která jako vstupní parametr přijme název předmětu a přidá nový klíč do subjects
 *                                      - AddGrade, která jako vstupní parametr přijme název předmětu a známku a přidá podle názvu předmětu další známku danému předmětu
 *                                      - CalculateSubjectGrade, která jako stupní parametr přijme název předmětu a spočítá průměrnou známku na vysvědčení z daného předmětu
 *                                      - CaculateTotalGrade, která spočítá studijní průměr (průměr všech známek)
 *    Přidej třídě Student konstruktor, který bude přijímat dva parametry - jméno a ročník studenta
 *                                                                        - Při jeho zavolání nastav jméno a ročník podle vstupních parametrů, id vygeneruj podobně jako accountNumber
 *                                                                          ve tříde BankAccount, a subjects nastav na nový prázdný List
 * 
 * 3) BONUS - Až vytvoříš Student, přidej možnost mít u známek váhy. Způsob nechám na tobě, možností je víc. Můžeš celou třídu překopat na známky pouze s váhou, a nebo můžeš zachovat
 *            možnost přidávat známky bez váhy s tím, že ty budou mít nějakou výchozí váhu automaticky, a přidáš varianty funkcí na přidávání známek s váhou
 */
namespace ClassPlayground
{
	internal class Student
	{
		int year {  get; set; }
		int id { get; set; }
		Dictionary<string, List<int[]>> subjects { get; set; }
		string name { get; set; }
		public Student(string name, int year) 
		{
			Random rnd = new Random();
			if (name == null) throw new ArgumentNullException("name");
			this.name = name;
			this.year = year;
			if(year <= 0) this.year = 1;
			subjects = new Dictionary<string, List<int[]>>();
			id = rnd.Next(10000000, 99999999);
		}
		public void AddSubject(string subject)
		{
			if (subjects.ContainsKey(subject))
			{
				Console.WriteLine("Predmet s nazvem " + subject + "uz je zapsan u studenta" + name);
				return;
			}
			subjects[subject] = new List<int[]>();
			Console.WriteLine("Predmet s nazvem " + subject + "byl zapsan studentovi " + name);
		}
		public void AddGrade(int grade, string subject, int weight = 1)
		{
			if (!subjects.ContainsKey(subject))
			{
				Console.WriteLine("Predmet s nazvem " + subject + "neni zapsan u studenta " + name);
				return;
			}
			subjects[subject].Add(new int[2] { grade, weight });
		}
		public double CalculateSubjectGrade(string subject)
		{
			if (!subjects.ContainsKey(subject))
			{
				Console.WriteLine("Predmet s nazvem " + subject + "neni zapsan u studenta " + name);
				return -1;
			}
			if (subjects[subject].Count == 0)
			{
				Console.WriteLine("Predmet s nazvem " + subject + "nema zapsane zadne znamky u studenta " + name);
				return -1;
			}
			double average = 0;
			int sum = 0; int count = 0;
			foreach (int[] grade in subjects[subject])
			{
				sum += grade[0] * grade[1];
				count+= grade[1];
			}
			average = sum / count;
			Console.WriteLine(name + " ma z predmetu " + subject + " prumer " + average);
			return  average;
		}
		public double CaculateTotalGrade()
		{
			double average = 0;
			double sum = 0; int count = 0;
			foreach (string grade in subjects.Keys)
			{
				
				sum += CalculateSubjectGrade(grade);
				count++;
			}
			average = sum / count;
			Console.WriteLine(name + " ma studijni prumer: " + average);
			return average;
		}
	}
}
