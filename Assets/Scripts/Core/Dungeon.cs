using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Dungeon
    {
        private const int cellsPerMonster = 10;

        private System.Random randgen = new System.Random();
        private readonly Cells[,] map;
        private int monsterCount;
        private Dictionary<char, Cells> cellConverter = new Dictionary<char, Cells>
        {
            { ' ', Cells.Empty },
            { '#', Cells.Wall },
            { 'E', Cells.Exit }
        };

        public Dungeon(string[] inputMap)
        {
            map = TranslateMap(inputMap);
            monsterCount = Width * Height / cellsPerMonster;
        }

        public Cells this[int row, int col]
        {
            get => map[row, col];
        }

        public int MonsterCount
        {
            get => monsterCount;
        }

        public int Width
        {
            get => map.GetLength(1);
        }

        public int Height
        {
            get => map.GetLength(0);
        }

        public Stack<Vector2Int> findPath(Vector2Int start, Vector2Int target)
        {
            Stack<Vector2Int> path = new Stack<Vector2Int>();
            int[,] stepMap = new int[Height, Width];
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    stepMap[row, col] = int.MaxValue;
                }
            }
            SortedSet<Vector2Int> positions = new SortedSet<Vector2Int>(new ByDistance(target)) { start };
            stepMap[start.y, start.x] = 0;

            while (positions.Count > 0)
            {
                Vector2Int pos = positions.First();
                positions.Remove(pos);

                if (pos == target)
                {
                    Vector2Int currentPos = new Vector2Int(pos.x, pos.y);
                    path.Push(currentPos);

                    while (stepMap[currentPos.y, currentPos.x] > 1) // was 0
                    {
                        foreach (Vector2Int cell in currentPos.CellsAround())
                        {
                            if (stepMap[cell.y, cell.x] == stepMap[currentPos.y, currentPos.x] - 1)
                            {
                                path.Push(cell);
                                currentPos = cell;

                                break;
                            }
                        }
                    }
                    //path.Push(currentPos);

                    return path;
                }

                foreach (Vector2Int cell in pos.CellsAround())
                {
                    if (map[cell.y, cell.x] == Cells.Wall || map[cell.y, cell.x] == Cells.Exit) { continue; }
                    if (stepMap[cell.y, cell.x] < stepMap[pos.y, pos.x] + 1) { continue; }
                    stepMap[cell.y, cell.x] = stepMap[pos.y, pos.x] + 1;

                    positions.Add(cell);
                }
            }

            return path;
        }

        public Vector2Int getRandomFreePos()
        {
            while (true)
            {
                Vector2Int cords = new Vector2Int(randgen.Next(Width), randgen.Next(Height));

                if (this[cords.y, cords.x] != Cells.Empty)
                {
                    continue;
                }

                return cords;
            }
        }

        private Cells[,] TranslateMap(string[] inputMap)
        {
            Cells[,] newMap = new Cells[inputMap.Length, inputMap[0].Length];

            for (int row = 0; row < inputMap.Length; row++)
            {
                for (int col = 0; col < inputMap[row].Length; col++)
                {
                    newMap[row, col] = cellConverter[inputMap[row][col]];
                }
            }

            return newMap;
        }

        private class ByDistance : IComparer<Vector2Int>
        {
            private readonly Vector2Int target;

            public ByDistance(Vector2Int target)
            {
                this.target = target;
            }

            public int Compare(Vector2Int a, Vector2Int b)
            {
                return a.ManhattanDistance(target) - b.ManhattanDistance(target);
            }
        }
    }
}
