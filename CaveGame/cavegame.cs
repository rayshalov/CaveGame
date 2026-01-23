using Game;
using System;
using System.Globalization;
using System.Threading;
using System.Runtime.InteropServices;

namespace CaveGame
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "CaveGame";
            Console.CursorVisible = false;

            Menu menu = new Menu();
            GameMap map = new GameMap();
            Person person = new Person(map);
            GetInput input = new GetInput();
            GUI gui = new GUI();
            Render render = new Render();

            if (menu.GetInputMenu())
            {
                Console.Clear();

                while (true)
                {
                    gui.FPSCounter();
                    Console.SetCursorPosition(0, 0);
                    render.Draw(map, person, person.CheckPosLight());
                    gui.ShowFPS(map);
                    input.GetInputMenu(person, map, render, gui);
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
        private string[] map { get; } = new string[]
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
    "########################################################################################################################"
        };

        public int mapWidth => map[0].Length;
        public int mapHeight => map.Length;

        public char GetCharOfMap(int y, int x)
        {
            return map[y][x];
        }
    }

    class Render
    {
        public bool visionFlag { get; private set; } = false;
        public bool fieldOnScreen = false;
        private DateTime lightEnd = DateTime.MinValue;

        //чит-костыль(мб убрать потом)
        public void Cheat()
        {

            Console.Clear();
            visionFlag = !visionFlag;


            if (visionFlag) lightEnd = DateTime.Now.AddYears(1);
            else lightEnd = DateTime.MinValue;
        }

        public void Draw(GameMap map, Person person, bool flag)
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

            if (visionFlag)
            {
                if (fieldOnScreen == false)
                {
                    for (int i = 0; i < map.mapHeight; i++)
                    {
                        for (int j = 0; j < map.mapWidth; j++)
                        {
                            Console.Write(map.GetCharOfMap(i, j));
                        }

                        if (i < map.mapHeight - 1)
                        {
                            Console.WriteLine();
                        }
                    }
                }

                fieldOnScreen = true;

                Console.SetCursorPosition(person.personX, person.personY);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(person.person);
                Console.ResetColor();

                if (person.personX != person.lastPersonX || person.personY != person.lastPersonY)
                {
                    Console.SetCursorPosition(person.lastPersonX, person.lastPersonY);
                    Console.WriteLine(map.GetCharOfMap(person.lastPersonY, person.lastPersonX));
                }    


                if (person.lightX != -1 && person.lightY != -1)
                {
                    Console.SetCursorPosition(person.lightX, person.lightY);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(person.light);
                    Console.ResetColor();
                }


            }
            else
            {
                fieldOnScreen = false;

                for (int y = -4; y <= 4; y++)
                {
                    flag = true;

                    for (int x = -4; x <= 4; x++)
                    {
                        if (person.personY + y >= 0 && person.personY + y < map.mapHeight && person.personX + x >= 0 && person.personX + x < map.mapWidth)
                        {
                            if (flag)
                            {
                                Console.SetCursorPosition(person.personX + x, person.personY + y);
                                flag = false;
                            }
                            if (y == -4 || y == 4 || x == -4 || x == 4)
                            {
                                Console.Write(" ");
                            }
                            else
                            {
                                if (person.personY + y == person.personY && person.personX + x == person.personX)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write(person.person);
                                    Console.ResetColor();
                                }
                                else if (person.personY + y == person.lightY && person.personX + x == person.lightX)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write(person.light);
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.Write(map.GetCharOfMap(person.personY + y, person.personX + x));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class Entity
    {
        public char entity;
        public int entityX;
        public int entityY;
        public int lastEntityX;
        public int lastEntityY;
    }

    class Person
    {
        private static Random rnd = new Random();

        public Person(GameMap map)
        {
            while (true)
            {
                if (map.GetCharOfMap(lightY, lightX) == '#')
                {
                    lightX = rnd.Next(1, map.mapWidth - 2);
                    lightY = rnd.Next(1, map.mapHeight - 2);
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
        public int lastPersonX { get; private set; } = 1;
        public int lastPersonY { get; private set; } = 1;


        public char light { get; } = '*';
        public int lightX { get; private set; } = 0;
        public int lightY { get; private set; } = 0;

        public bool CheckPosLight()
        {
            if (lightX == personX && lightY == personY)
            {
                lightY = lightX = -1;
                return true;
            }
            return false;
        }

        public void PersonLastPosition()
        {
            lastPersonX = personX;
            lastPersonY = personY;
        }

        public void TryMovePerson(int newY, int newX, GameMap map)
        {

            if (newX >= 0 && newX <= map.mapWidth - 2 && newY >= 0 && newY <= map.mapHeight - 2)
            {
                if (map.GetCharOfMap(newY, newX) != '#')
                {
                    personX = newX;
                    personY = newY;
                }
            }
        }
    }

    class GetInput
    {
        private DateTime lastMoveTime = DateTime.Now;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        private bool IsKeyDown(int vKey)
        {
            return (GetAsyncKeyState(vKey) & 0x8000) != 0;
        }
        public void GetInputMenu(Person person, GameMap map, Render render, GUI gui)
        {

            if ((DateTime.Now - lastMoveTime).TotalMilliseconds < 100)
            {
                return;
            }

            person.PersonLastPosition();

            bool moved = false;

            if (IsKeyDown(0x57))
            {
                person.TryMovePerson(person.personY - 1, person.personX, map);
                moved = true;
            }
            if (IsKeyDown(0x53))
            {
                person.TryMovePerson(person.personY + 1, person.personX, map);
                moved = true;
            }
            if (IsKeyDown(0x41))
            {
                person.TryMovePerson(person.personY, person.personX - 1, map);
                moved = true;
            }
            if (IsKeyDown(0x44))
            {
                person.TryMovePerson(person.personY, person.personX + 1, map);
                moved = true;
            }

            if (IsKeyDown(0x20))
            {
                render.Cheat();
                Thread.Sleep(200);
            }

            if (moved)
            {
                lastMoveTime = DateTime.Now;
            }

            while (Console.KeyAvailable) Console.ReadKey(true);
        }
    }

    class GUI
    {
        private int frames;
        private int FPS;
        private DateTime lastTime = DateTime.Now;

        public void FPSCounter()
        {
            frames++;
        }

        public void ShowFPS(GameMap map)
        {

            if ((DateTime.Now - lastTime).TotalSeconds >= 1.0)
            {
                FPS = frames;
                frames = 0;
                lastTime = DateTime.Now;

                Console.SetCursorPosition(0, map.mapHeight);
                Console.Write($"FPS: {FPS} ");
            }
        }
    }
}