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
            for (int i = 0; i < 20; i++)
            {
                player p = new player((player.player_kind)(i % 5), "", new point(-1, -1), (player.player_direction)range.getRandFrom(0, 6), '\\', -1, -1, -1,-1, -1, -1, true);
                world.players.Add(p);
                Console.WriteLine("name : {0}\ncharacter : {7}\nposition : {8}\nkind : {1}\ndirection : {9}\nage : {2}\nmaxage : {3}\nweight : {4}\nhunger level : {5}\nis alive : {6}\n\n_________________________________________________\n\n", p.Name, p.Kind.ToString(), p.Age, p.MaxAge, p.Weight, p.Hunger, p.Alive.ToString(), p.Character, p.Position.ToString(), p.Direction.ToString());
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
                Console.Title = $"generation : {world.generation} & year : {world.year} & players : {world.players.Count}";
                System.Threading.Thread.Sleep(world.__waitAfterNewGen);
            }
        }

    }
    static class world
    {
        public static char[,] buffer, nullBuffer;
        public static long generation = 0, year = 0;
        //public variables
        public static int w = 30, h = 15, speed = 1;
        public static List<player> players = new List<player>();
        #region configValues
        public static int __weightstart = 10, __weightend = 150, __agestart = 0, __hungerstart = 0, __hungerend = 10, __maxagestart = 20, __maxageend = 100, __namestart = 100000, __nameend = 900000, __genstoyear = 20,__childstart=0,__childend=3,__maxPlayers=200,__waitAfterNewGen=0,__maxmaxage=100;
        public static char[] __defaultKindCharacters = { 'M', 'F', 'f', 'A', 'a' };
        #endregion

        private static int[] kinds = { 0, 0, 0, 0, 0 };
        public static void nextGen()
        {
            kinds = new int[]{ 0, 0, 0, 0, 0 };
            if (players.Count == 0)
            {
                gameOver();
                return;
            }

            List<player> toRemove = new List<player>();
            List<player> toAdd = new List<player>();
            for (int i = 0; i < players.Count; i++)
            {
                if (generation % __genstoyear == 0)
                {
                    if(players[i].Age<__maxmaxage)
                    players[i].Age++; 
                if(players[i].Kind!=player.player_kind.animalMale|| players[i].Kind != player.player_kind.animalFemale)
                        players[i].Age++;
                }


                //remove dead players
                if (players[i].Weight <= 1)
                {
                    toRemove.Add(players[i]);
                    continue;
                }
                if (players[i].Age >= players[i].MaxAge)
                {
                    players[i].Alive = false;
                    switch (players[i].Kind)
                    {
                        case player.player_kind.male:
                            players[i].Kind = player.player_kind.food;
                            players[i].Character = __defaultKindCharacters[2];
                            players[i].Age = 0;
                            break;
                        case player.player_kind.female:
                            players[i].Kind = player.player_kind.food;
                            players[i].Character = __defaultKindCharacters[2];
                            players[i].Age = 0;
                            break;
                        case player.player_kind.food:
                            toRemove.Add(players[i]);
                            continue;
                        //break;
                        case player.player_kind.animalMale:
                            players[i].Kind = player.player_kind.food;
                            players[i].Character = __defaultKindCharacters[2];
                            players[i].Age = 0;
                            break;
                        case player.player_kind.animalFemale:
                            players[i].Kind = player.player_kind.food;
                            players[i].Character = __defaultKindCharacters[2];
                            players[i].Age = 0;
                            break;
                        default:
                            break;
                    }
                }
                if (players[i].Alive == false)
                    continue;
                if (players[i].Kind != player.player_kind.food && players[i].Weight == __weightend)
                {
                    players[i].MaxAge += 1;
                    players[i].Weight = (int)(players[i].Weight*0.9);
                    players[i].Child++;
                }
                //living.
                switch (players[i].Kind)
                {
                    case player.player_kind.male:
                        foreach (player  x in players)
                        {
                            if (x.Kind == player.player_kind.animalMale || x.Kind == player.player_kind.male || x.Kind == player.player_kind.animalFemale||x.Position.x!=players[i].Position.x|| x.Position.y != players[i].Position.y)
                                continue;
                            if (x.Kind == player.player_kind.food && players[i].Weight < __weightend)
                            {
                                if (x.Weight <= __weightend - players[i].Weight)
                                {
                                    players[i].Weight += x.Weight;
                                    x.Weight = 0;
                                }
                                else 
                                {
                                    players[i].Weight += __weightend - players[i].Weight;
                                    x.Weight -= __weightend - players[i].Weight;
                                }
                            }
                            else if(__maxPlayers>players.Count&&players[i].Child>0&&x.Kind == player.player_kind.female&&x.Child>0)
                            {

                                player p = new player((player.player_kind)range.getRandFrom(0,1), "", new point(-1, -1), (player.player_direction)range.getRandFrom(0, 6), '\\', 0, 0, -1, 0, x.Weight/2+players[i].Weight/2, -1, true);
                                toAdd.Add(p);
                                players[i].Child--;
                                players[i].Weight = players[i].Weight / 2;
                                    x.Child--;
                                x.Weight = x.Weight / 2;
                                if (players[i].Weight == 1)
                                    toRemove.Add(players[i]);
                                if (x.Weight == 1)
                                    toRemove.Add(x);
                            }
                        }
                        break;
                    case player.player_kind.female:
                        foreach (player x in players)
                        {
                            if (x.Kind == player.player_kind.animalMale || x.Kind == player.player_kind.female || x.Kind == player.player_kind.male || x.Kind == player.player_kind.animalFemale || x.Position.x != players[i].Position.x || x.Position.y != players[i].Position.y)
                                continue;
                            if (x.Kind == player.player_kind.food && players[i].Weight < __weightend)
                            {
                                if (x.Weight <= __weightend - players[i].Weight)
                                {
                                    players[i].Weight += x.Weight;
                                    x.Weight = 0;
                                }
                                else
                                {
                                    players[i].Weight += __weightend - players[i].Weight;
                                    x.Weight -= __weightend - players[i].Weight;
                                }
                            }
                        }
                        break;
                    case player.player_kind.animalMale:
                        foreach (player x in players)
                        {
                            if (x.Kind == player.player_kind.animalMale  || x.Position.x != players[i].Position.x || x.Position.y != players[i].Position.y)
                                continue;
                            if (x.Kind == player.player_kind.food/*|| x.Kind == player.player_kind.male|| x.Kind == player.player_kind.female */&& players[i].Weight < __weightend)
                            {
                                if (x.Weight <= __weightend - players[i].Weight)
                                {
                                    players[i].Weight += x.Weight;
                                    x.Weight = 0;
                                }
                                else
                                {
                                    players[i].Weight += __weightend - players[i].Weight;
                                    x.Weight -= __weightend - players[i].Weight;
                                }
                            }
                            else if (__maxPlayers > players.Count && players[i].Child > 0 && x.Kind == player.player_kind.animalFemale && x.Child > 0)
                            {

                                player p = new player((player.player_kind)range.getRandFrom(3, 4), "", new point(-1, -1), (player.player_direction)range.getRandFrom(0, 6), '\\', 0, 0, -1, 0, x.Weight / 2 + players[i].Weight / 2, -1, true);
                                toAdd.Add(p);
                                    players[i].Child--;
                                players[i].Weight = players[i].Weight / 2;
                                    x.Child--;
                                x.Weight = x.Weight / 2;
                                if (players[i].Weight == 1)
                                    toRemove.Add(players[i]);
                                if(x.Weight==1)
                                    toRemove.Add(x);
                            }
                        }
                        break;
                    case player.player_kind.animalFemale:
                        foreach (player x in players)
                        {
                            if (x.Kind == player.player_kind.animalFemale || x.Kind == player.player_kind.animalMale || x.Position.x != players[i].Position.x || x.Position.y != players[i].Position.y)
                                continue;
                            if (x.Kind == player.player_kind.food /*|| x.Kind == player.player_kind.male || x.Kind == player.player_kind.female*/ && players[i].Weight < __weightend)
                            {
                                if (x.Weight <= __weightend - players[i].Weight)
                                {
                                    players[i].Weight += x.Weight;
                                    x.Weight = 0;
                                }
                                else
                                {
                                    players[i].Weight += __weightend - players[i].Weight;
                                    x.Weight -= __weightend - players[i].Weight;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

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
                            if (players[i].Position.x < w - 1 && players[i].Position.y < h - 1)
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

                            if (players[i].Position.x > 0 && players[i].Position.y < h - 1)
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

                //loging
                kinds[(int)players[i].Kind]++;
                kinds[2] = players.Count - kinds.Sum() + kinds[2];
                if (kinds.Sum() == kinds[2])
                    kinds[2] = players.Count;
            }

            Console.WriteLine($"\nmales : {kinds[0]} , females : {kinds[1]} , foods : {kinds[2]} , animalMale : {kinds[3]} , animalFemale : {kinds[4]}                      ");

            generation++;
            if (generation % __genstoyear == 0)
                ++year;
            foreach (player x in toRemove)
                players.Remove(x);
            players.AddRange(toAdd);
        }
        public static void gameOver()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("your world ended in {0} generations ({1} years).", generation, year);
            Console.ReadLine();
            
        }
        public static void draw()
        {
            if (nullBuffer == null || nullBuffer.GetLength(0) != w || nullBuffer.GetLength(1) != h)
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
                    map.Append(buffer[i, j]);
                }
                if (j != h - 1) map.Append('\n');
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
            for (int i = 0; i < players.Count; i++)
                if (players[i].Position.x == position.x && players[i].Position.y == position.y)
                    return i;
            return -1;
        }
    }

    class player
    {
        public enum player_kind { male = 0, female = 1, food = 2, animalMale = 3, animalFemale = 4 }
        public enum player_direction { n = 0, ne = 1, e = 2, se = 3, s = 4, sw = 5, w = 6, nw = 7 }

        public string Name;
        public bool Alive;
        public int Age, Hunger, Weight, MaxAge,Child;
        public int Injure;
        public player_kind Kind;
        public player_direction Direction;
        public char Character;
        public point Position;

        //-1 for int values will replace randomly
        //"" for string values will replace randomly
        //'\' for character values will replace randomly
        //(-1,-1) for points will replace randomly
        public player(player_kind kind, string name, point position, player_direction direction=player_direction.nw, char character = '\\', int age = 0, int injure = 0,int child=2, int hunger = 0, int weight = -1, int maxAge = -1, bool alive = true)
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
            Character = character == '\\' ? world.__defaultKindCharacters[(int)kind] : character;

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

            //assign childs count;
            if (child > -1)
                Child = child;
            else
            {
                r.Start = world.__childstart;
                r.End = world.__childend;
                Child = r.getRandFrom();
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
                    r.End = world.w - 1;
                    position.x = r.getRandFrom();
                    r.End = world.h - 1;
                    position.y = r.getRandFrom();
                } while (!world.isPositionEmpty(position));
            Position = position;
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
            return r.Next(Start, End + 1);
        }
        public static int getRandFrom(int start, int end)
        {
            return r.Next(start, end + 1);
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
