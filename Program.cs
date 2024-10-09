using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int answerId = 0;
            Random rnd = new Random();
            int aiId = 0;
            Console.WriteLine("zadej na kolik bodu hrajeme");
            int maxSkore = Convert.ToInt32(Console.ReadLine());
            int playerSkore = 0;
            int aiSkore = 0;

            while(playerSkore < maxSkore && aiSkore < maxSkore)
            {
            Console.WriteLine("kamen, nuzky, papir...");


            while (PlayerWon(answerId, aiId) == 0 || answerId == -1)
            {
                Console.WriteLine("zadej");
                switch (Console.ReadLine())
                {
                    case "n":
                    case "nuzky":
                    case "1":
                            answerId = 1;
                        break;
                    case "k":
                    case "kamen":
                    case "0":
                            answerId = 0;
                        break;
                    case "p":
                    case "papir":
                    case "2":
                            answerId = 2;
                        break;
                    default:
                        answerId = -1;
                        Console.WriteLine("nerozumim, asi nebudes ta nejostrejsi tuzka v penalu");
                        break;
                }
                
                aiId = rnd.Next(3);
                writeAnswer(aiId);
            }
                if (PlayerWon(answerId, aiId) == 1)
                {
                    Console.WriteLine("no tak jsi vyhral, a co?");
                    Console.WriteLine("skore hrace: " + ++playerSkore);
                }
                else
                {
                    Console.WriteLine("prohral jsi. gg ez, skill issue");
                    Console.WriteLine("skore ai: " + ++aiSkore);
                }
                answerId = 0;
                aiId = 0;
            }
            Console.WriteLine("no tak jsi vyhral, a co?");
            Console.WriteLine("prohral jsi. gg ez, skill issue");
            Console.ReadKey();
        }
        static void writeAnswer(int id)
        {
            switch (id)
                {
                case 0:
                    Console.WriteLine("kamen");
                    break;
                case 1:
                    Console.WriteLine("nuzky");
                    break;
                case 2:
                    Console.WriteLine("papir");
                    break;
            }
        }
        static int PlayerWon(int answerId, int aiId)
        {
            //kamen = 0
            //nuzky = 1
            //papir = 2
            while (true)
            {
                switch (answerId)
                {
                    case 0: //hrac dal kamen
                        if(aiId == 1) return 1;
                        if (aiId == 2) return -1;
                        break;
                    case 1: //hrac dal nuzky
                        if (aiId == 2) return 1;
                        if (aiId == 0) return -1;
                        break;
                    case 2: //hrac dal papir
                        if (aiId == 0) return 1;
                        if (aiId == 1) return -1;
                        break;
                }
                return 0; //remiza
            }
        }
    }
}
