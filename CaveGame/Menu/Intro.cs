using System;

namespace CaveGame.Menu
{
    class Intro
    {
        private static string CaveGameStr = "CAVEGAME";
        public void ShowIntro()
        {
            int startX = (Console.WindowWidth / 2) - (CaveGameStr.Length / 2);
            int centerY = Console.WindowHeight / 2;

            for (int i = 0; i < CaveGameChar.Length; i++)
            {
                Console.SetCursorPosition(startX + i, centerY);
                Console.Write(CaveGameChar[i]);
                Thread.Sleep(350);
            }
            Thread.Sleep(1000);
            Console.Clear();
        }
    }
}
