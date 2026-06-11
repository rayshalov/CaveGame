using System;
using System.Collections.Generic;
using System.Linq;
using CaveGame.Core;
using CaveGame.Menu;
using CaveGame.Edit;

namespace CaveGame.Entities
{
    class Entity
    {
        public char entity { get; protected set; }
        public int entityX { get; protected set; }
        public int entityY { get; protected set; }
        public int lastEntityX { get; protected set; }
        public int lastEntityY { get; protected set; }

        protected DateTime lastMoveTime = DateTime.MinValue;
        protected DateTime lastStepTime = DateTime.MinValue;

        private Queue<(int, int)> visitedCells = new Queue<(int, int)>();
        private int visionRadius = 20;
        private int memorySize = 50;

        private static Random rnd = new Random();

        public void PersonLastPosition()
        {
            lastEntityX = entityX;
            lastEntityY = entityY;
        }

        public virtual void UpdatePosition(Person player, GameMap map, AudioManager audio) // применять checkcollisions 
        {
            if ((DateTime.Now - lastMoveTime).TotalSeconds < (double)Settings.selectedSpeed / 10.0)
            {
                return;
            }

            lastMoveTime = DateTime.Now;

            int distanceToPlayer = Math.Abs(entityX - player.entityX) + Math.Abs(entityY - player.entityY);

            visitedCells.Enqueue((entityX, entityY));

            if (visitedCells.Count > memorySize)
            {
                visitedCells.Dequeue();
            }

            PersonLastPosition();

            var directions = new List<(int x, int y)>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int nx = entityX + dx;
                    int ny = entityY + dy;

                    if (nx >= 0 && nx < map.mapWidth && ny >= 0 && ny < map.mapHeight && map.GetCharOfMap(ny, nx) != '#')
                    {
                        directions.Add((nx, ny));
                    }
                }
            }

            (int x, int y) nextStep;

            if (distanceToPlayer <= visionRadius)
            {
                var bestMoves = directions.OrderBy(m =>
                    Math.Abs(m.x - player.entityX) + Math.Abs(m.y - player.entityY)
                ).ToList();

                nextStep = bestMoves.FirstOrDefault(m => !visitedCells.Contains(m));

                if (nextStep == default)
                {
                    nextStep = bestMoves[0];
                }
            }
            else
            {
                var freshMoves = directions.Where(m => !visitedCells.Contains(m)).ToList();

                if (freshMoves.Count > 0)
                {
                    nextStep = freshMoves[rnd.Next(freshMoves.Count)];
                }
                else
                {
                    nextStep = directions[rnd.Next(directions.Count)];
                }
            }

            float volume = 1.0f - (distanceToPlayer / 30.0f);

            if (volume > 0)
            {
                if ((DateTime.Now - lastStepTime).TotalMilliseconds >= 200)
                {
                    audio.PlayRandomSteps(volume); //xz поиграться с шагами у @ и &
                    lastStepTime = DateTime.Now;
                }
            }

            entityX = nextStep.x;
            entityY = nextStep.y;
        }

        public bool CheckCollisions(List<Entity> list, Render render)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var ent = list[i];

                if (ent != this && ent.entityX == this.entityX && ent.entityY == this.entityY)
                {
                    if (ent is Light light)
                    {
                        render.ActivateVision();
                        list.RemoveAt(i);
                    }
                    if (ent is Monster monster)
                    {
                        return true;
                    }
                    if (ent is ExitSymbol exit)
                    {
                        list.RemoveAt(i);
                        ent.entityY = -1;
                    }
                }
            }
            return false;
        }

        public void SetPosition(int x, int y)
        {
            entityX = x;
            entityY = y;
        }
    }
}
