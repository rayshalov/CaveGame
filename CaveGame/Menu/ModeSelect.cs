using System;

namespace CaveGame.Menu
{
    class ModeSelect
    {
        private int selectedButton = 0;
        private int selectedCustomMap = 0;
        private string story = "История";
        private string custom = "Кастомная карта";
        private string symbol = "<";
        private string back = "Назад";

        public void ShowMode()
        {
            int centerX = (Console.WindowWidth / 2) - (story.Length / 2);
            int centerY = (Console.WindowHeight / 2) - 1;

            Console.SetCursorPosition(centerX, centerY);
            Console.WriteLine(story + (selectedButton == 0 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 1);
            Console.WriteLine(custom + (selectedButton == 1 ? symbol + "  " : "  "));

            Console.SetCursorPosition(centerX, centerY + 2);
            Console.WriteLine(back + (selectedButton == 2 ? symbol + "  " : "  "));
        }

        public void SelectCustomMap(string[] str)
        {
            for (int i = 0 ; i <= str.Length; i++)
            {
                if (i < str.Length)
                {
                    int centerX = (Console.WindowWidth / 2) - (str.Length / 2);
                    int centerY = (Console.WindowHeight / 2 - (str.Length / 2)) + i;

                    Console.SetCursorPosition(centerX, centerY);
                    Console.WriteLine(str[i] + (selectedCustomMap == i ? symbol + "  " : "  "));
                }
                else
                {
                    int centerX = (Console.WindowWidth / 2) - (str.Length / 2);
                    int centerY = (Console.WindowHeight / 2 - (str.Length / 2)) + i;

                    Console.SetCursorPosition(centerX, centerY);
                    Console.WriteLine(back + (i == str.Length ? symbol + "  " : "  "));
                }
            }
        }

        public void SelectAndEditCustomMap(string[] str)
        {
            for (int i = 0; i <= str.Length; i++)
            {
                if (i == str.Length)
                {
                    int centerX = (Console.WindowWidth / 2) - (str.Length / 2);
                    int centerY = (Console.WindowHeight / 2 - (str.Length / 2)) + i;

                    Console.SetCursorPosition(centerX, centerY);
                    if (selectedCustomMap == str.Length)
                    {
                        Console.WriteLine("Новая карта" + symbol);
                    }
                    else
                    {
                        Console.WriteLine("Новая карта" + " ");
                    }
                }
                else
                {
                    int centerX = (Console.WindowWidth / 2) - (str.Length / 2);
                    int centerY = (Console.WindowHeight / 2 - (str.Length / 2)) + i;

                    Console.SetCursorPosition(centerX, centerY);
                    Console.WriteLine(str[i] + (selectedCustomMap == i ? symbol + "  " : "  "));
                }
            }
        }

        public int GetInputMode()
        {
            while (true)
            {
                ShowMode();

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        return selectedButton;
                    case ConsoleKey.W:
                        if (selectedButton > 0)
                        {
                            selectedButton--;
                        }
                        break;
                    case ConsoleKey.S:
                        if (selectedButton < 2)
                        {
                            selectedButton++;
                        }
                        break;
                }
            }
        }

        public int GetInputCustomMap(string[] str, bool flag)
        {
            while (true)
            {
                if (flag)
                {
                    SelectAndEditCustomMap(str);
                }
                else
                {
                    SelectCustomMap(str);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        return selectedCustomMap;
                    case ConsoleKey.W:
                        if (selectedCustomMap > 0)
                        {
                            selectedCustomMap--;
                        }
                        break;
                    case ConsoleKey.S:
                        if (flag)
                        {
                            if (selectedCustomMap < str.Length)
                            {
                                selectedCustomMap++;
                            }
                        }
                        else
                        {
                            if (selectedCustomMap < str.Length - 1)
                            {
                                selectedCustomMap++;
                            }
                        }
                        break;
                }
            }
        }
    }
}
