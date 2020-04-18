using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_of_life
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //try to assign variables form given args.
            if (args.Length > 0)
                int.TryParse(args[0], out world.w);
            if (args.Length > 1)
                int.TryParse(args[0], out world.h);
            if (args.Length > 2)
                int.TryParse(args[0], out world.speed);


            //---------------- a simple test ------------------
            for (int i = 0; i < 200; i++)
            {
                player p = new player((player.player_kind)(i % 5), "",new point(-1,-1),'\\', -1, -1, -1,-1,true) ;

                Console.WriteLine("name : {0}\ncharacter : {7}\nposition : {8}\nkind : {1}\nage : {2}\nmaxage : {3}\nweight : {4}\nhunger level : {5}\nis alive : {6}\n\n_________________________________________________\n\n", p.Name, p.Kind.ToString(), p.Age, p.MaxAge, p.Weight, p.Hunger, p.Alive.ToString(), p.Character,p.Position.ToString());
            }
        }
        
    }
    static class world
    {
        //public variables
        public static int w = 30, h = 30, speed = 1;
        public static List<player> players = new List<player>();
        #region configValues
        public static int __weightstart = 10, __weightend = 150, __agestart = 0, __hungerstart = 0, __hungerend = 10, __maxagestart = 10, __maxageend = 100, __namestart = 100000, __nameend = 900000;
        public static char[] __defaultKindCharacters = { 'M', 'F', 'f', 'A', 'a' };
        #endregion
        public static bool havePlayer(string name)
        {
            foreach (player x in players)
                if (x.Name == name)
                    return true;
            return false;
        }
        public static bool isPositionEmpty(point p)
        {
            foreach (player x in players)
                if (x.Position.x == p.x && x.Position.y == p.y)
                    return false;
            return true;
        }
    }
    class player
    {
        public enum player_kind { male=0, female=1, food=2, animalMale=3 ,animalFemale=4}

        public string Name;
        public bool Alive;
        public int Age, Hunger, Weight, MaxAge;
        public player_kind Kind;
        public char Character;
        public point Position;

        //-1 for int values will replace randomly
        //"" for string values will replace randomly
        //'\' for character values will replace randomly
        //(-1,-1) for points will replace randomly
        public player( player_kind kind,string name,point position,char character='\\', int age = 0, int hunger = 0, int weight = -1, int maxAge = -1, bool alive = true)
        {
            range r = new range(0, 1);
            
            //try to find a random name if name not assigned
            if (name == "")
            {
                r.Start = world.__namestart;
                r.End = world.__nameend;
                do
                {
                    Name = r.getRandFrom().ToString();
                } while (world.havePlayer(Name));
            }
            
            Kind = kind;

            //assign character accourding to kind if not assidned
            Character = character == '\\' ? world.__defaultKindCharacters[(int)kind]:character;

            //try to guess a maxage for player if not assigned
            if (maxAge > 0)
                MaxAge = maxAge;
            else
            {
                r.Start = world.__maxagestart; r.End = world.__maxageend;
                MaxAge = r.getRandFrom();
            }

            //try to guess an age for player if not assigned
            if (age == -1)
            {
                r.Start = world.__agestart;
                r.End = MaxAge;
                Age = r.getRandFrom();
            }

            //if age > maxage then player can`t live.
            Alive = Age < MaxAge ? alive : false;

            //try to guess a hunger level for player if not assigned
            r.Start = world.__hungerstart; r.End = world.__hungerend;
            Hunger = hunger == -1 ? r.getRandFrom() : hunger;

            //try to guess a weight for player if not assigned
            r.Start = world.__weightstart; r.End = world.__weightend;
            Weight = weight == -1 ? r.getRandFrom() : weight;

            //try to find a new empty position.
            if (position.x == -1 && position.y == -1 || !world.isPositionEmpty(position))
                do
                {

                    r.Start = 0;
                    r.End = world.w;
                    position.x = r.getRandFrom();
                    r.End = world.h;
                    position.y = r.getRandFrom();
                } while (!world.isPositionEmpty(position));
            Position=position;
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
        public string ToString()
        {
            return x.ToString() + ':' + y.ToString();
        }
    }
}
