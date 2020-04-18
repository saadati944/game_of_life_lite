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
            for (int i = 0; i < 100; i++)
            {
                player p = new player((player.player_kind)(i % 5), "" ,new point(-1,-1),(player.player_direction)range.getRandFrom(0,6),'\\', -1,-1, -1, -1,-1,true) ;
                world.players.Add(p);
                Console.WriteLine("name : {0}\ncharacter : {7}\nposition : {8}\nkind : {1}\ndirection : {9}\nage : {2}\nmaxage : {3}\nweight : {4}\nhunger level : {5}\nis alive : {6}\n\n_________________________________________________\n\n", p.Name, p.Kind.ToString(), p.Age, p.MaxAge, p.Weight, p.Hunger, p.Alive.ToString(), p.Character,p.Position.ToString(),p.Direction.ToString());
            }
            /*player p = new player((player.player_kind)(0), "", new point(0, 29), (player.player_direction)range.getRandFrom(0, 6), '3', -1, -1, -1, -1, true);
            world.players.Add(p);
            p = new player((player.player_kind)(1), "", new point(0, 0), player.player_direction.e, '0', -1, -1, -1, -1, true);
            world.players.Add(p);
            p = new player((player.player_kind)(2), "", new point(29, 0), (player.player_direction)range.getRandFrom(0, 6), '1', -1, -1, -1, -1, true);
            world.players.Add(p);
            p = new player((player.player_kind)(2), "", new point(29, 29), (player.player_direction)range.getRandFrom(0, 6), '2', -1, -1, -1, -1, true);
            world.players.Add(p);*/
            Console.Write("press enter to exit");
            Console.ReadLine();
            Console.Clear();
            while (true)
            {
                world.nextGen();
                world.draw();
                Console.SetCursorPosition(0, 0);
                Console.Write(world.convertBuffer());
                System.Threading.Thread.Sleep(20);
            }
        }
        
    }
    static class world
    {
        public static char[,] buffer, nullBuffer;
        public static long generation=0, year=0;
        //public variables
        public static int w = 30, h = 30, speed = 1;
        public static List<player> players = new List<player>();
        #region configValues
        public static int __weightstart = 10, __weightend = 150, __agestart = 0, __hungerstart = 0, __hungerend = 10, __maxagestart = 10, __maxageend = 100, __namestart = 100000, __nameend = 900000,__genstoyear=30;
        public static char[] __defaultKindCharacters = { 'M', 'F', 'f', 'A', 'a' };
        #endregion
        public static void nextGen()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Alive == false)
                    continue;

                //moving codes
                if (players[i].Kind != player.player_kind.food)
                switch (players[i].Direction)
                {
                    case player.player_direction.n:
                        if (players[i].Position.y > 0)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                                players[i].Position.y--;
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        /*if (players[i].Position.y > 0)
                            players[i].Position.y--;
                        else
                        {
                            switch (range.getRandFrom(0, 2))
                            {
                                case 0:
                                    players[i].Direction = player.player_direction.s;
                                    break;
                                case 1:
                                    if (players[i].Position.x > 0)
                                        players[i].Direction = player.player_direction.se;
                                    break;
                                case 2:
                                    if (players[i].Position.x < w - 1)
                                        players[i].Direction = player.player_direction.sw;
                                    break;
                                default:
                                    break;
                            }
                        }*/
                        break;
                    case player.player_direction.ne:
                        if (players[i].Position.x < w - 1 && players[i].Position.y > 0)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                            {
                                players[i].Position.x++;
                                players[i].Position.y--;
                            }
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        break;
                    case player.player_direction.e:
                        if (players[i].Position.x < w - 1)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                                players[i].Position.x++;
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        /*if (players[i].Position.x < w - 1)
                            players[i].Position.x++;
                        else
                        {
                            switch (range.getRandFrom(0, 2))
                            {
                                case 0:
                                    players[i].Direction = player.player_direction.w;
                                    break;
                                case 1:
                                    if (players[i].Position.y > 0)
                                        players[i].Direction = player.player_direction.nw;
                                    break;
                                case 2:
                                    if (players[i].Position.x < w - 1)
                                        players[i].Direction = player.player_direction.sw;
                                    break;
                                default:
                                    break;
                            }
                        }*/
                        break;
                    case player.player_direction.se:
                        if (players[i].Position.x < h - 1 && players[i].Position.y < w - 1)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                            {
                                players[i].Position.x++;
                                players[i].Position.y++;
                            }
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        break;
                    case player.player_direction.s:
                        if (players[i].Position.y < h - 1)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                                players[i].Position.y++;
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        /*if (players[i].Position.y < h-1)
                            players[i].Position.y++;
                        else
                        {
                            switch (range.getRandFrom(0, 2))
                            {
                                case 0:
                                    players[i].Direction = player.player_direction.n;
                                    break;
                                case 1:
                                    if (players[i].Position.x > 0)
                                        players[i].Direction = player.player_direction.ne;
                                    break;
                                case 2:
                                    if (players[i].Position.x < w - 1)
                                        players[i].Direction = player.player_direction.nw;
                                    break;
                                default:
                                    break;
                            }
                        }*/
                        break;
                    case player.player_direction.sw:

                        if (players[i].Position.x > 0 && players[i].Position.y < w - 1)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                            {
                                players[i].Position.x--;
                                players[i].Position.y++;
                            }
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        break;
                    case player.player_direction.w:
                        if (players[i].Position.x > 0)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                                players[i].Position.x--;
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        /*if (players[i].Position.x > 0)
                            players[i].Position.x--;
                        else
                        {
                            switch (range.getRandFrom(0, 2))
                            {
                                case 0:
                                    players[i].Direction = player.player_direction.e;
                                    break;
                                case 1:
                                    if (players[i].Position.y > 0)
                                        players[i].Direction = player.player_direction.nw;
                                    break;
                                case 2:
                                    if (players[i].Position.y < h - 1)
                                        players[i].Direction = player.player_direction.sw;
                                    break;
                                default:
                                    break;
                            }
                        }*/
                        break;
                    case player.player_direction.nw:
                        if (players[i].Position.x > 0 && players[i].Position.y > 0)
                        {
                            if (range.getRandFrom(0, 6) == 0)
                                players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                            else
                            {
                                players[i].Position.x--;
                                players[i].Position.y--;
                            }
                        }
                        else
                            players[i].Direction = (player.player_direction)range.getRandFrom(0, 6);
                        break;
                    default:
                        break;
                }
            }
            generation++;
            if (generation % __genstoyear == 0)
                ++year;
        }
        public static void draw()
        {
            if(nullBuffer==null||nullBuffer.GetLength(0)!=w||nullBuffer.GetLength(1)!=h)
                nullBuffer = emptyBuffer();
            buffer = (char[,])nullBuffer.Clone();
            foreach (player p in players)
            {
                buffer[p.Position.x, p.Position.y] = p.Character;
            }
        }
        public static string convertBuffer()
        {
            StringBuilder map = new StringBuilder();
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++) 
                {
                    map.Append(buffer[i,j]);
                }
                if(j!=h-1) map.Append('\n');
            }
            return map.ToString();
        }
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
        public static char[,] emptyBuffer()
        {
            char[,] bfr = new char[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    bfr[i, j] = ' ';
            return bfr;
        }
        public static int getPlayerAt(point position)
        {
            for (int i=0;i<players.Count;i++)
                if (players[i].Position.x == position.x && players[i].Position.y == position.y)
                    return i;
            return -1;
        }
    }
    class player
    {
        public enum player_kind { male=0, female=1, food=2, animalMale=3 ,animalFemale=4}
        public enum player_direction {n=0,ne=1,e=2,se=3,s=4,sw=5,w=6,nw=7 }

        public string Name;
        public bool Alive;
        public int Age, Hunger, Weight, MaxAge;
        public int Injure;
        public player_kind Kind;
        public player_direction Direction;
        public char Character;
        public point Position;

        //-1 for int values will replace randomly
        //"" for string values will replace randomly
        //'\' for character values will replace randomly
        //(-1,-1) for points will replace randomly
        public player( player_kind kind,string name,point position,player_direction direction,char character='\\', int age = 0,int injure=0, int hunger = 0, int weight = -1, int maxAge = -1, bool alive = true)
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
            Direction = direction;

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

            //assign injure level
            Injure = injure > -1 ? Injure = injure : range.getRandFrom(0, 10);

            //if age > maxage then player can`t live.
            Alive = Age < MaxAge ? alive : false;

            //try to guess a hunger level for player if not assigned
            r.Start = world.__hungerstart; r.End = world.__hungerend;
            Hunger = hunger == -1 ? r.getRandFrom() : hunger;

            //try to guess a weight for player if not assigned
            r.Start = world.__weightstart; r.End = world.__weightend;
            Weight = weight == -1 ? r.getRandFrom() : weight;

            //try to find a new empty position.
            if (position.x == -1 || position.y == -1 || !world.isPositionEmpty(position))
                do
                {
                    r.Start = 0;
                    r.End = world.w-1;
                    position.x = r.getRandFrom();
                    r.End = world.h-1;
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
