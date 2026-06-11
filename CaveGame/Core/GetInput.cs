using System;
using System.Collections.Generic;
using System.Linq;
using CaveGame.Entities;
using CaveGame.Menu;
using CaveGame.Edit;
using System.Runtime.InteropServices;

namespace CaveGame.Core
{
    class GetInput
    {
        private DateTime lastMoveTime = DateTime.Now;
        private DateTime lastSwitchTime = DateTime.Now;
        private DateTime lastStepTime = DateTime.MinValue;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        private bool IsKeyDown(int vKey)
        {
            return (GetAsyncKeyState(vKey) & 0x8000) != 0;
        }
        public void GetInputMenu(Person person, GameMap map, Render render, AudioManager audio)
        {

            if ((DateTime.Now - lastMoveTime).TotalMilliseconds < 150)
            {
                return;
            }

            person.PersonLastPosition();

            bool moved = false;

            int oldX = person.entityX;
            int oldY = person.entityY;

            if (IsKeyDown(0x57))
            {
                person.TryMovePerson(person.entityY - 1, person.entityX, map);
                moved = true;
            }
            if (IsKeyDown(0x53))
            {
                person.TryMovePerson(person.entityY + 1, person.entityX, map);
                moved = true;
            }
            if (IsKeyDown(0x41))
            {
                person.TryMovePerson(person.entityY, person.entityX - 1, map);
                moved = true;
            }
            if (IsKeyDown(0x44))
            {
                person.TryMovePerson(person.entityY, person.entityX + 1, map);
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

                //bool actuallyMoved = (person.entityX != oldX || person.entityY != oldY);

                //if (actuallyMoved && (DateTime.Now - lastStepTime).TotalMilliseconds >= 300)
                //{
                //    audio.PlayRandomSteps(1.0f);
                //    lastStepTime = DateTime.Now;
                //}
            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public void GetRedactorInputMenu(Editor edit, Cursor cursor)
        {
            if ((DateTime.Now - lastMoveTime).TotalMilliseconds < 100)
            {
                return;
            }

            cursor.PersonLastPosition();

            bool moved = false;

            if (IsKeyDown(0x57))
            {
                cursor.TryMoveCursor(cursor.entityY - 1, cursor.entityX, edit);
                moved = true;
            }
            if (IsKeyDown(0x53))
            {
                cursor.TryMoveCursor(cursor.entityY + 1, cursor.entityX, edit);
                moved = true;
            }
            if (IsKeyDown(0x41))
            {
                cursor.TryMoveCursor(cursor.entityY, cursor.entityX - 1, edit);
                moved = true;
            }
            if (IsKeyDown(0x44))
            {
                cursor.TryMoveCursor(cursor.entityY, cursor.entityX + 1, edit);
                moved = true;
            }
            if (IsKeyDown(0x51))
            {
                if ((DateTime.Now - lastSwitchTime).TotalMilliseconds >= 200)
                {
                    cursor.SwapChar(edit.SwitchSymbol());
                    lastSwitchTime = DateTime.Now;
                }
            }
            if (IsKeyDown(0x45))
            {
                if ((DateTime.Now - lastSwitchTime).TotalMilliseconds >= 200)
                {
                    cursor.SwapChar('█');
                    lastSwitchTime = DateTime.Now;
                }
            }
            if (IsKeyDown(0x20))
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
            if (IsKeyDown(0x09))
            {
                edit.switchActiveInput = false;
            }

            if (moved)
            {
                lastMoveTime = DateTime.Now;
            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
