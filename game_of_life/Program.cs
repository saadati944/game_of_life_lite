using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_of_life
{
    
    class Program
    {
        //public variables
        static int w = 30, h = 30, speed = 1;
        public static List<player> players = new List<player>();
        #region configValues
        public static int __weightstart = 10, __weightend = 150, __agestart = 0, __hungerstart = 0,__hungerend=10,__maxagestart=10,__maxageend=100,__namestart=100000,__nameend=900000;
        #endregion


        static void Main(string[] args)
        {
            //try to assign variables form given args.
            if (args.Length > 0)
                int.TryParse(args[0], out w);
            if (args.Length > 1)
                int.TryParse(args[0], out h);
            if (args.Length > 2)
                int.TryParse(args[0], out speed);


            //---------------- a simple test ------------------
            for (int i = 0; i < 20; i++)
            {
                player p = new player((player.player_kind)(i % 5), "", -1, -1, -1,-1,true) ;

                Console.WriteLine("name : {0}\nkind : {1}\nage : {2}\nmaxage : {3}\nweight : {4}\nhunger level : {5}\nis alive : {6}\n\n_________________________________________________\n\n", p.Name, p.Kind.ToString(), p.Age, p.MaxAge, p.Weight, p.Hunger, p.Alive.ToString());
            }
        }
        public static bool havePlayer(string name)
        {
            foreach (player x in players)
                if (x.Name == name)
                    return true;
            return false;
        }
    }
    class player
    {
        public enum player_kind { male=0, female=1, food=2, animalMale=3 ,animalFemale=4}

        public string Name;
        public bool Alive;
        public int Age, Hunger, Weight, MaxAge;
        public player_kind Kind;

        //-1 for int values will replace with random
        //"" for string values will replace with random
        public player( player_kind kind,string name, int age = 0, int hunger = 0, int weight = -1, int maxAge = -1, bool alive = true)
        {
            range r = new range(0, 1);
            
            //try to find a random name if name not assigned
            if (name == "")
            {
                r.Start = Program.__namestart;
                r.End = Program.__nameend;
                do
                {
                    name = r.getRandFrom().ToString();
                } while (Program.havePlayer(name));
            }
            
            Kind = kind;

            //try to guess a maxage for player if not assigned
            if (maxAge > 0)
                MaxAge = maxAge;
            else
            {
                r.Start = Program.__maxagestart; r.End = Program.__maxageend;
                MaxAge = r.getRandFrom();
            }

            //try to guess an age for player if not assigned
            if (age == -1)
            {
                r.Start = Program.__agestart;
                r.End = MaxAge;
                Age = r.getRandFrom();
            }

            //if age > maxage then player can`t live.
            Alive = Age < MaxAge ? alive : false;

            //try to guess a hunger level for player if not assigned
            r.Start = Program.__hungerstart; r.End = Program.__hungerend;
            Hunger = hunger == -1 ? r.getRandFrom() : hunger;

            //try to guess a weight for player if not assigned
            r.Start = Program.__weightstart; r.End = Program.__weightend;
            weight = weight == -1 ? r.getRandFrom() : weight;

        }
    }

    class range
    {
        public int Start, End;
        public static Random r = new Random();
        public range(int start, int end)
        {
            Start = start;
            End = end;
        }
        public int getRandFrom()
        {
            return r.Next(Start, End+1);
        }
        public static int getRandFrom(int start, int end)
        {
            return r.Next(start, end+1);
        }
    }
    class point
    {
        public int x, y;
        public point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
