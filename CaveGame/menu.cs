namespace Game
{
    class Menu
    {
        public bool ChoisedButton = true;
        public string Play { get; } = "Играть";
        public string Quit { get; } = "Выйти";
        public string symbol { get; } = "<";

        public int width { get; } = Console.WindowWidth;
        public int height { get; } = Console.WindowHeight;

        public void ShowMenuWord()
        {
            string[,] menus = new string[5, 14]
            {
    { "#"," ","#"," ","#","#"," ","#","#","#"," ","#"," ","#" },
    { "#","#","#"," ","#"," "," ","#"," ","#"," ","#"," ","#" },
    { "#","#","#"," ","#","#"," ","#"," ","#"," ","#"," ","#" },
    { "#"," ","#"," ","#"," "," ","#"," ","#"," ","#"," ","#" },
    { "#"," ","#"," ","#","#"," ","#"," ","#"," ","#","#","#" }
            };

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - (14 / 2), i);

                for (int j = 0; j < 14; j++)
                {
                    Console.Write(menus[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void ShowMenu()
        {
            int centerX = (Console.WindowWidth / 2) - (Play.Length / 2);
            int centerY = (Console.WindowHeight / 2) - 1;

            if (ChoisedButton)
            {
                Console.SetCursorPosition(centerX, centerY);
                Console.WriteLine(Play + symbol);
                Console.SetCursorPosition(centerX, centerY + 1);
                Console.WriteLine(Quit + " ");
            }
            else
            {
                Console.SetCursorPosition(centerX, centerY);
                Console.WriteLine(Play + " ");
                Console.SetCursorPosition(centerX, centerY + 1);
                Console.WriteLine(Quit + symbol);
            }
        }

        public bool GetInputMenu()
        {
            ShowMenuWord();

            while (true)
            {
                ShowMenu();
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (ChoisedButton == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    case ConsoleKey.W:
                        if (ChoisedButton == false)
                        {
                            ChoisedButton = true;
                        }
                        break;
                    case ConsoleKey.S:
                        if (ChoisedButton == true)
                        {
                            ChoisedButton = false;
                        }
                        break;
                }
            }
        }
    }
}