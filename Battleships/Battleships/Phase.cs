using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal abstract class Phase
    {
        public virtual Battlefield Player { get; set; }
        public virtual Battlefield Ai { get; set; }
        public Phase(ref Battlefield player, ref Battlefield ai)
        {
            Player = player;
            Ai = ai;
        }
        public abstract bool isCompleted { get; set; }
        public virtual void Init()
        {
            isCompleted = false;
        }
        public abstract void UserInput();
        public abstract void Render();
        public abstract void Update();
        public virtual void Start()
        {
            Init();
            while (!isCompleted)
            {
                Render();
                UserInput();
                Update();
            }
        }
        public virtual void WriteBattleships()
        {
            // ChatGTP generated
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("░░░░░   ░░░░░  ░░░░░  ░░░░░  ░      ░░░░░   ░░░░   ░   ░  ░░░   ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░      ░       ░   ░   ░    ░   ░  ░      ");
            Console.WriteLine("░░░░░   ░░░░░    ░      ░    ░      ░░░░    ░░░░   ░░░░░   ░    ░░░░    ░░░░  ");
            Console.WriteLine("░   ░   ░   ░    ░      ░    ░      ░           ░  ░   ░   ░    ░           ░ ");
            Console.WriteLine("░░░░░   ░   ░    ░      ░    ░░░░░  ░░░░░   ░░░░   ░   ░  ░░░   ░       ░░░░  \n");
            Console.ResetColor();
        }
    }
}
