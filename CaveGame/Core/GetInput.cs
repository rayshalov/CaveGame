using System;
using System.Collections.Generic;
using System.Linq;
using CaveGame.Entities;
using CaveGame.Menu;
using CaveGame.Edit;
using System.Runtime.InteropServices;
using System.Threading;

namespace CaveGame.Core
{
    class GetInput
    {
        private DateTime lastMoveTime = DateTime.Now;
        private DateTime lastSwitchTime = DateTime.Now;
        private DateTime lastStepTime = DateTime.MinValue;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        private Dictionary<ConsoleKey, DateTime> recentKeys = new Dictionary<ConsoleKey, DateTime>();

        private void PollConsoleKeys()
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                recentKeys[key] = DateTime.Now;
            }
        }

        private bool IsKeyDownCrossPlatform(ConsoleKey key)
        {
            if (recentKeys.TryGetValue(key, out var time))
            {
                return (DateTime.Now - time).TotalMilliseconds < 300;
            }
            return false;
        }

        private bool IsKeyDown(int vKey)
        {
            if (!OperatingSystem.IsWindows())
            {
                return false;
            }
            return (GetAsyncKeyState(vKey) & 0x8000) != 0;
        }

        public void GetInputMenu(Person person, GameMap map, Render render, AudioManager audio)
        {
            if (!OperatingSystem.IsWindows())
            {
                PollConsoleKeys();
            }

            if ((DateTime.Now - lastMoveTime).TotalMilliseconds < 150)
            {
                return;
            }

            person.PersonLastPosition();

            bool moved = false;

            int oldX = person.entityX;
            int oldY = person.entityY;

            bool up, down, left, right, space;

            if (OperatingSystem.IsWindows())
            {
                up = IsKeyDown(0x57);
                down = IsKeyDown(0x53);
                left = IsKeyDown(0x41);
                right = IsKeyDown(0x44);
                space = IsKeyDown(0x20);

            }
            else
            {
                up = IsKeyDownCrossPlatform(ConsoleKey.W);
                down = IsKeyDownCrossPlatform(ConsoleKey.S);
                left = IsKeyDownCrossPlatform(ConsoleKey.A);
                right = IsKeyDownCrossPlatform(ConsoleKey.D);
                space = IsKeyDownCrossPlatform(ConsoleKey.Spacebar);
            }

            if (up)
            {
                person.TryMovePerson(person.entityY - 1, person.entityX, map);
                moved = true;
            }
            if (down)
            {
                person.TryMovePerson(person.entityY + 1, person.entityX, map);
                moved = true;
            }
            if (left)
            {
                person.TryMovePerson(person.entityY, person.entityX - 1, map);
                moved = true;
            }
            if (right)
            {
                person.TryMovePerson(person.entityY, person.entityX + 1, map);
                moved = true;
            }

            if (space)
            {
                render.Cheat();
                Thread.Sleep(200);
            }

            if (moved)
            {
                lastMoveTime = DateTime.Now;
            }

            if (OperatingSystem.IsWindows())
            {
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
            else
            {
                recentKeys.Clear();
            }
        }

        public void GetRedactorInputMenu(Editor edit, Cursor cursor)
        {
            if (!OperatingSystem.IsWindows())
            {
                PollConsoleKeys();
            }

            if ((DateTime.Now - lastMoveTime).TotalMilliseconds < 100)
            {
                return;
            }

            cursor.PersonLastPosition();

            bool moved = false;

            bool up, down, left, right, switchSym, erase, action, tab;

            if (OperatingSystem.IsWindows())
            {
                up = IsKeyDown(0x57);
                down = IsKeyDown(0x53);
                left = IsKeyDown(0x41);
                right = IsKeyDown(0x44);
                switchSym = IsKeyDown(0x51);
                erase = IsKeyDown(0x45);
                action = IsKeyDown(0x20);
                tab = IsKeyDown(0x09);
            }
            else
            {
                up = IsKeyDownCrossPlatform(ConsoleKey.W);
                down = IsKeyDownCrossPlatform(ConsoleKey.S);
                left = IsKeyDownCrossPlatform(ConsoleKey.A);
                right = IsKeyDownCrossPlatform(ConsoleKey.D);
                switchSym = IsKeyDownCrossPlatform(ConsoleKey.Q);
                erase = IsKeyDownCrossPlatform(ConsoleKey.E);
                action = IsKeyDownCrossPlatform(ConsoleKey.Spacebar);
                tab = IsKeyDownCrossPlatform(ConsoleKey.Tab);
            }

            if (up)
            {
                cursor.TryMoveCursor(cursor.entityY - 1, cursor.entityX, edit);
                moved = true;
            }
            if (down)
            {
                cursor.TryMoveCursor(cursor.entityY + 1, cursor.entityX, edit);
                moved = true;
            }
            if (left)
            {
                cursor.TryMoveCursor(cursor.entityY, cursor.entityX - 1, edit);
                moved = true;
            }
            if (right)
            {
                cursor.TryMoveCursor(cursor.entityY, cursor.entityX + 1, edit);
                moved = true;
            }
            if (switchSym)
            {
                if ((DateTime.Now - lastSwitchTime).TotalMilliseconds >= 200)
                {
                    cursor.SwapChar(edit.SwitchSymbol());
                    lastSwitchTime = DateTime.Now;
                }
            }
            if (erase)
            {
                if ((DateTime.Now - lastSwitchTime).TotalMilliseconds >= 200)
                {
                    cursor.SwapChar('█');
                    lastSwitchTime = DateTime.Now;
                }
            }
            if (action)
            {
                if (cursor.entity == '█')
                {
                    edit.TryErase(cursor.entityY, cursor.entityX);
                }
                else
                {
                    edit.TryDraw(cursor.entityY, cursor.entityX);
                }
            }
            if (tab)
            {
                edit.switchActiveInput = false;
            }

            if (moved)
            {
                lastMoveTime = DateTime.Now;
            }

            if (!OperatingSystem.IsWindows())
            {
                recentKeys.Clear();
            }
        }
    }
}