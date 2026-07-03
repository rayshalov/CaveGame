using CaveGame.Core;
using CaveGame.Edit;
using CaveGame.Entities;
using CaveGame.menu;
using CaveGame.Menu;
using System;
using System.Data;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace CaveGame
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "CaveGame";
            Console.CursorVisible = false;
            
            // Проверка ОС для установки размера окна (только для Windows)
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(122, 30);
                Console.SetBufferSize(122, 30);
            }

            // Показ интро (статический метод)
            Intro.ShowIntro();
            
            while (true)
            {
                GameMenu menu = new GameMenu();
                Settings set = new Settings();
                GameMap map = new GameMap();
                Person person = new Person('@', 1, 1);
                Light light = new Light(map, '*');
                ExitSymbol exit = new ExitSymbol(map);
                Cursor cursor = new Cursor(0, 0);
                GetInput input = new GetInput();
                GUI gui = new GUI();
                Render render = new Render();
                Editor edit = new Editor();
                ModeSelect mode = new ModeSelect();
                AudioManager audio = new AudioManager();

                int menuChoise = menu.GetInputMenu();

                List<Entity> list = new List<Entity>();
                list.Add(person);
                list.Add(light);
                list.Add(exit);

                for (int i = 1; i <= Settings.selectedQuantity; i++)
                {
                    list.Add(new Monster(map));
                }

                if (menuChoise == 0)
                {
                    Console.Clear();
                    int menuModeChoise = mode.GetInputMode();

                    if (menuModeChoise == 0)
                    {
                        Console.Clear();

                        while (exit.EndOfGame()) // сюжетный цикл
                        {
                            gui.FPSCounter();
                            Console.SetCursorPosition(0, 0);
                            foreach (var ent in list)
                            {
                                if (ent is Monster m)
                                {
                                    m.UpdatePosition(person, map, audio);
                                }
                            }
                            if (person.CheckCollisions(list, render))
                            {
                                break;
                            }
                            render.Draw(map, person, list);
                            gui.ShowFPS();
                            input.GetInputMenu(person, map, render, audio);
                        }
                            Console.Clear();
                            Thread.Sleep(2000);
                    }
                    else if (menuModeChoise == 1)
                    {
                        Console.Clear();
                        string[] maps = edit.GetCustomMaps(); // получение списка кастомных карт
                        
                        // Проверка: есть ли кастомные карты в папке с игрой
                        if (maps == null || maps.Length == 0) 
                        {
                            Console.Clear();
                            // Вывод сообщения об ошибке по центру экрана
                            int centerX = (Console.WindowWidth / 2) - ("Ошибка: кастомные карты не найдены!".Length / 2);
                            int centerY = (Console.WindowHeight / 2);
                            Console.SetCursorPosition(centerX, centerY);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка: кастомные карты не найдены!");
                            Console.ResetColor();
                            Console.SetCursorPosition(centerX, centerY + 1);
                            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                            Console.ReadKey(true);
                            Console.Clear();
                            continue; // возврат в главное меню
                        }
                        
                        int chosen = mode.GetInputCustomMap(maps, false);
                        if (chosen >= maps.Length)
                        {
                            Console.Clear();
                            continue;
                        }

                        edit.LoadMap(maps[chosen] + ".txt");

                        GameMap customMap = new GameMap(edit.customMap);

                        Console.Clear();

                        var (px, py) = customMap.FindSymbol('@'); // инициализация энтити 
                        if (px != -1)
                        {
                            person.SetPosition(px, py);
                            customMap.EraseSymbol(px, py);
                        }

                        var (lx, ly) = customMap.FindSymbol('*');
                        if (lx != -1)
                        {
                            light.SetPosition(lx, ly);
                            customMap.EraseSymbol(lx, ly);
                        }

                        var (mx, my) = customMap.FindSymbol('&');
                        while (mx != -1)
                        {
                            list.Add(new Monster(map, mx, my));
                            customMap.EraseSymbol(mx, my);
                            (mx, my) = customMap.FindSymbol('&');
                        }

                        var (ex, ey) = customMap.FindSymbol('X');
                        if (ex != -1)
                        {
                            exit.SetPosition(ex, ey);
                            customMap.EraseSymbol(ex, ey);
                        }

                        while (exit.EndOfGame()) // кастомный цикл
                        {
                            gui.FPSCounter();
                            Console.SetCursorPosition(0, 0);
                            foreach (var ent in list)
                            {
                                if (ent is Monster m)
                                {
                                    m.UpdatePosition(person, customMap, audio);
                                }
                            }
                            person.CheckCollisions(list, render);
                            render.Draw(customMap, person, list);
                            gui.ShowFPS();
                            input.GetInputMenu(person, customMap, render, audio);
                        }
                        Console.Clear();
                        Thread.Sleep(2000);
                    }
                    else if (menuModeChoise == 2)
                    {
                        Console.Clear();
                        continue;
                    }
                }
                else if (menuChoise == 1) // настройки
                {
                    Console.Clear();
                    if (set.GetInputSettings() == 4)
                    {
                        Console.Clear();
                        continue;
                    }
                }
                else if (menuChoise == 2) // редактор
                {
                    Console.Clear();
                    string[] maps = edit.GetCustomMaps();
                    int chosen = mode.GetInputCustomMap(maps, true);
                    if (chosen >= maps.Length)
                    {
                        edit.ClearMap();
                    }
                    else
                    {
                        edit.LoadMap(maps[chosen] + ".txt");
                    }

                    GameMap customMap = new GameMap(edit.customMap);

                    while (true)
                    {
                        Console.SetCursorPosition(0, 0);
                        render.DrawOnlyMap(edit.customMap, cursor, edit);
                        edit.ShowRedactorMenu();

                        if (edit.switchActiveInput)
                        {
                            input.GetRedactorInputMenu(edit, cursor);
                        }
                        else
                        {
                            int res = edit.GetInputRedactor();

                            if (res == 0)
                            {
                                edit.SaveMap("map1.txt");
                                Console.Clear();
                                string fileName = edit.GetFileName();
                                edit.SaveMap(fileName + ".txt");
                                Console.Clear();
                                int centerX = (Console.WindowWidth / 2) - ("Сохранено".Length / 2);
                                int centerY = (Console.WindowHeight / 2);
                                Console.SetCursorPosition(centerX, centerY);
                                Console.WriteLine("Сохранено");
                                Thread.Sleep(1000);
                                Console.Clear();
                                render.fieldOnScreen = false;
                            }
                            else if (res == 1)
                            {
                                Console.Clear();
                                break;
                            }
                            else if (res == 2)
                            {
                                edit.switchActiveInput = true;
                            }
                        }
                    }
                }
                else if (menuChoise == 3)
                {
                    break;
                }
            }
        }
    }
}
