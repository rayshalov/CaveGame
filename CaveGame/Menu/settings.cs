using System;
using System.Collections.Generic;
using System.Linq;
using CaveGame.Entities;
using CaveGame.Core;
using CaveGame.Edit;

namespace CaveGame.Menu
{
    class Settings
    {
        public int selectedButton = 0;
        public static int selectedSpeed = 3;
        public static int selectedQuantity = 1;
        public static bool selectedRender = false;
        public static bool selectedVolumeMode = false;
        public string amountMonsters { get; } = "Количество монстров: ";
        public string monstersSpeed { get; } = "Скорость монстров: ";
        public string render { get; } = "Ограниченная видимость: ";
        public string volume { get; } = "Звуковое сопровождение: ";
        public string back { get; } = "Назад";
        public string symbol { get; } = "<";

        public int width = Console.WindowWidth;
        public int height = Console.WindowHeight;

        private Dictionary<int, string> speedList = new Dictionary<int, string>
        {
            {1, "высокая"},
            {2, "умеренная"},
            {3, "медленная"}
        };

        private Dictionary<bool, string> YesNoList = new Dictionary<bool, string>
        {
            {false, "да"},
            {true, "нет"}
        };

        public void ShowSettings()
        {
            int centerX = (Console.WindowWidth / 2) - (amountMonsters.Length / 2);
            int centerY = (Console.WindowHeight / 2) - 1;

            Console.SetCursorPosition(centerX, centerY);
            Console.WriteLine(monstersSpeed + speedList[selectedSpeed] + (selectedButton == 0 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 1);
            Console.WriteLine(amountMonsters + selectedQuantity + (selectedButton == 1 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 2);
            Console.WriteLine(render + YesNoList[selectedRender] + (selectedButton == 2 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 3);
            Console.WriteLine(volume + YesNoList[selectedVolumeMode] + (selectedButton == 3 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 4);
            Console.WriteLine(back + (selectedButton == 4 ? symbol : "  "));
        }

        public int GetInputSettings()
        {
            while (true)
            {

                if (width != Console.WindowWidth || height != Console.WindowHeight) // не доделал
                {
                    width = Console.WindowWidth;
                    height = Console.WindowHeight;
                    Console.Clear();
                }

                ShowSettings();

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (selectedButton == 4)
                        {
                            return 4;
                        }
                        break;
                    case ConsoleKey.W:
                        if (selectedButton > 0)
                        {
                            selectedButton--;
                        }
                        break;
                    case ConsoleKey.S:
                        if (selectedButton < 4)
                        {
                            selectedButton++;
                        }
                        break;
                    case ConsoleKey.A:
                        if (selectedButton == 0)
                        {
                            if (selectedSpeed < 3)
                            {
                                selectedSpeed++;
                            }
                        }
                        else if (selectedButton == 1)
                        {
                            if (selectedQuantity > 0)
                            {
                                selectedQuantity--;
                            }
                        }
                        else if (selectedButton == 2)
                        {
                            selectedRender = !selectedRender;
                        }
                        else if (selectedButton == 3)
                        {
                            selectedVolumeMode = !selectedVolumeMode;
                        }
                        break;
                    case ConsoleKey.D:
                        if (selectedButton == 0)
                        {
                            if (selectedSpeed > 1)
                            {
                                selectedSpeed--;
                            }
                        }
                        else if (selectedButton == 1)
                        {
                            if (selectedQuantity < 10)
                            {
                                selectedQuantity++;
                            }
                        }
                        else if (selectedButton == 2)
                        {
                            selectedRender = !selectedRender;
                        }
                        else if (selectedButton == 3)
                        {
                            selectedVolumeMode = !selectedVolumeMode;
                        }
                        break;
                }
            }
        }
    }
}
