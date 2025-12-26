using Game;
using System.Threading;

namespace CaveGame
{
    class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;
            bool visibleFlag = true;
            Person person = new Person();
            Menu menu = new Menu();
            GameMap map = new GameMap();
            GetInput input = new GetInput();

            if (menu.GetInputMenu())
            {
                Console.Clear();
                while (true)
                {
                    Console.SetCursorPosition(0, 0);
                    map.GetMap(person, visibleFlag);
                    person.GetLightPos(map.map);
                    input.GetInputMenu(map.map, person);
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
        public string[] rawMap = new string[]
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



        public string[,] map = new string[30, 120];

        public void GetMap(Person person, bool flag)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 120; j++)
                {
                    if (i == person.personY && j == person.personX)
                    {
                        map[i, j] = person.person;
                    }
                    else
                    {
                        map[i, j] = rawMap[i][j].ToString();
                    }
                }
            }

            switch (flag) // если подобрал *, то тру
            {
                case true:
                    for (int i = 0; i < 30; i++)    //полная прорисовка карты
                    {
                        for (int j = 0; j < 120; j++)
                        {
                            Console.Write(map[i, j]);
                        }

                        if (i < 29)
                        {
                            Console.WriteLine();
                        }
                    }
                    break;
                    //реализовать временное отображение карты при подборе *
                case false:
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
                                    Console.Write(map[person.personY + y, person.personX + x]);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }


    class Person
    {
        public string person = "@";
        public int personX = 1;
        public int personY = 1;

        public string light = "*";
        public int lightX = 0;
        public int lightY = 0;

        public void GetLightPos(string[,] map)
        {
            Random rnd = new Random();

            while (true)
            {   
                if (map[lightY, lightX] == "#" || map[lightY, lightX] == "@")
                {
                    lightX = rnd.Next(1, 118);
                    lightY = rnd.Next(1, 28);
                }
                else
                {
                    break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            map[lightY, lightX] = light;
            Console.ResetColor();
        }
    }

    class GetInput
    {
        public void GetInputMenu(string[,] map, Person person)
        {

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                    if (person.personY > 1 && map[person.personY - 1, person.personX] != "#")
                    {
                        person.personY--;
                    }
                    break;
                case ConsoleKey.S:
                    if (person.personY < 28 && map[person.personY + 1, person.personX] != "#")
                    {
                        person.personY++;
                    }
                    break;
                case ConsoleKey.A:
                    if (person.personX > 1 && map[person.personY, person.personX - 1] != "#")
                    {
                        person.personX--;
                    }
                    break;
                case ConsoleKey.D:
                    if (person.personX < 118 && map[person.personY, person.personX + 1] != "#")
                    {
                        person.personX++;
                    }
                    break;
            }
        }
    }
}

