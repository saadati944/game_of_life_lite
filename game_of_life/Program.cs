﻿using System;
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


        static void Main(string[] args)
        {
            //try to asign variables form given args.
            if (args.Length > 0)
                int.TryParse(args[0], out w);
            if (args.Length > 1)
                int.TryParse(args[0], out h);
            if (args.Length > 2)
                int.TryParse(args[0], out speed);

            

        }
    }
    class player
    {
        public enum player_kind { male,female,food,animal }

        public string Name;
        public bool alive;
        public int Age, Hunger, Weight,MaxAge;
        public player_kind Kind;

            //-1 for int values will replace with random
            //"" for string values will replace with random
        public player(string name, player_kind kind, int age = 0,int hunger=0,int weight=-1, int maxAge=-1, bool alive = true)
        {

        }
    }

}
