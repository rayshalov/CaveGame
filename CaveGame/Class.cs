using System;

namespace CaveGame.Core
{
    static class StoryCore
    {
        public static void ShowMessage(int x, int y, string msg)
        {
            Console.SetCursorPosition(x, y);
            foreach (char chr in msg)
            {
                Console.Write(chr);
                Thread.Sleep(200);
            }
        }
    }
}
