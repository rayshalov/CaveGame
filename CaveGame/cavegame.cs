using Game;
using System;
using System.Globalization;
using System.Threading;

namespace CaveGame
{
    class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            Menu menu = new Menu();
            GameMap map = new GameMap();
            Person person = new Person(map.map);
            GetInput input = new GetInput();

            if (menu.GetInputMenu())
            {
                Console.Clear();
                while (true)
                {
                    Console.SetCursorPosition(0, 0);
                    map.GetMap(person, person.CheckPosLight(map.map));
                    input.GetInputMenu(map.map, person, map);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }

    class GameMap
    {
        public bool visionFlag { get; private set; } = false;

        public void Cheat(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
            {
                Console.Clear();
                visionFlag = !visionFlag;
            }

            if (visionFlag) lightEnd = DateTime.Now.AddYears(1);
            else lightEnd = DateTime.MinValue;
        }

        private DateTime lightEnd = DateTime.MinValue;

        public string[] map { get; } = new string[]
        {
    "########################################################################################################################",
    "#        #######                  #                              #             #                  #                    #",
    "#        #     #        #####     #        ######                                                 #                    #",
    "#        #     #     ####         #   #####                                                       #                    #",
    "#        #     #                  #                              #             #                  #                    #",
    "#              #######            #                              #             #                  #                    #",
    "#                                 #   #             ########     #             #                                       #",
    "#                                 #   #             #            #     #       #     #########  ################   #####",
    "#                         ####    #   ####          #            #     #       #######                                 #",
    "#        #                #       #           #######            #      #      #          #                  #         #",
    "#        ###########      #       #   ####          #            #      #      #          #                  #         #",
    "#####              #      #       #   #             #            #      #      #          #                  #         #",
    "#           #      #              ###               #            #             #     ######                  #         #",
    "###         #      #                        #                                           #                    #         #",
    "#        #####     #              ###        #                   #             #        #                              #",
    "#        #         #      #       #           #         #        #             #                                       #",
    "#        #                #       #            #        ##########             #          #########                    #",
    "#                  #      ####    #                     #        #             #          #                            #",
    "#         #        #      #       #      #              #        #             #          #                            #",
    "#         #        #              #      #                       #             #          #                            #",
    "#    ######        #              #      #        #######        #             #          #            ##########  #####",
    "#         #        #              #      #     ####   #                        #          #            #               #",
    "#         #        ########  ######                   #          #    ####     #                       #               #",
    "#         #        #              #                   #          #  ###        #          #            #               #",
    "#         #        #              #     ###           #          #             #          #            #               #",
    "#   #       #      #              #       #         ######       #             #          #                            #",
    "#   #       #      #                      #                      #             #          #            #               #",
    "#   #       #      #              #       #                      #             #          #            #               #",
    "#   #       #      #              #       #                      #             #                       #               #",
    "########################################################################################################################"
        };

        public void GetMap(Person person, bool flag)
        {
            if (flag)
            {
                lightEnd = DateTime.Now.AddSeconds(5);
                visionFlag = true;
            }
            else if (visionFlag && DateTime.Now > lightEnd)
            {
                visionFlag = false;
                Console.Clear();
            }

            if (visionFlag) // если подобрал *, то тру
            {
                for (int i = 0; i < 30; i++)    //полная прорисовка карты
                {
                    for (int j = 0; j < 120; j++)
                    {
                        if (i == person.personY && j == person.personX)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write('@');
                            Console.ResetColor();
                        }
                        else if (i == person.lightY && j == person.lightX)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('*');
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(map[i][j]);
                        }
                    }

                    if (i < 29)
                    {
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                for (int y = -4; y <= 4; y++)
                {
                    for (int x = -4; x <= 4; x++)
                    {
                        if (person.personY + y >= 0 && person.personY + y < 30 && person.personX + x >= 0 && person.personX + x < 120)
                        {
                            Console.SetCursorPosition(person.personX + x, person.personY + y);

                            if (y == -4 || y == 4 || x == -4 || x == 4)
                            {
                                Console.Write(" ");
                            }
                            else
                            {

                                if (person.personY + y == person.personY && person.personX + x == person.personX)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write('@');
                                    Console.ResetColor();
                                }
                                else if (person.personY + y == person.lightY && person.personX + x == person.lightX)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write('*');
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.Write(map[person.personY + y][person.personX + x]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    class Person
    {
        public Person(string[] map)
        {
            Random rnd = new Random();

            while (true)
            {
                if (map[lightY][lightX] == '#')
                {
                    lightX = rnd.Next(1, 118);
                    lightY = rnd.Next(1, 28);
                }
                else
                {
                    break;
                }
            }
        }

        public char person { get; } = '@';
        public int personX { get; private set; } = 1;
        public int personY { get; private set; } = 1;

        public char light { get; } = '*';
        public int lightX { get; private set; } = 0;
        public int lightY { get; private set; } = 0;

        public bool CheckPosLight(string[] map)
        {
            if (lightX == personX && lightY == personY)
            {
                lightY = lightX = -1;
                return true;
            }
            return false;
        }

        public void TryMovePerson(int newY, int newX, string[] map)
        {
            if ((Math.Abs(personX - newX) == 1 || Math.Abs(personX - newX) == 0) && (Math.Abs(personY - newY) == 1 || Math.Abs(personY - newY) == 0))
            {
                if (newX >= 0 && newX <= 118 && newY >= 0 && newY <= 28)
                {
                    if (map[newY][newX] != '#')
                    {
                        personX = newX;
                        personY = newY;
                    }
                }
            }
        }
    }

    class GetInput
    {
        public void GetInputMenu(string[] map, Person person, GameMap cfg)
        {

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                    person.TryMovePerson(person.personY - 1, person.personX, map);
                    break;
                case ConsoleKey.S:
                    person.TryMovePerson(person.personY + 1, person.personX, map);
                    break;
                case ConsoleKey.A:
                    person.TryMovePerson(person.personY, person.personX - 1, map);
                    break;
                case ConsoleKey.D:
                    person.TryMovePerson(person.personY, person.personX + 1, map);
                    break;
                case ConsoleKey.Spacebar:
                    cfg.Cheat(ConsoleKey.Spacebar);
                    break;
            }
        }
    }
}

