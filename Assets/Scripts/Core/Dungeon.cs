using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Dungeon
    {
        private const int cellsPerMonster = 30;

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
            //monsterCount = 1;
        }

        public Cells this[Vector2Int pos]
        {
            get => this[pos.y, pos.x];
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

        public Stack<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
        {
            if (this[start] == Cells.Wall || this[start] == Cells.Exit)
            {
                throw new ArgumentException("Invalid start position: " + start);
            }

            if (this[target] == Cells.Wall || this[target] == Cells.Exit)
            {
                throw new ArgumentException("Invalid target position: " + target);
            }

            if (start == target)
            {
                throw new ArgumentException("Start and target are the same: " + start);
            }

            int[,] stepMap = new int[Height, Width];
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    stepMap[row, col] = int.MaxValue;
                }
            }
            PriorityQueue<Vector2Int> positions = new PriorityQueue<Vector2Int>((a, b) => a.ManhattanDistance(target) < b.ManhattanDistance(target));
            positions.Push(start);
            stepMap[start.y, start.x] = 0;

            while (positions.Count > 0)
            {
                Vector2Int pos = positions.Pop();

                if (pos == target)
                {
                    Stack<Vector2Int> path = new Stack<Vector2Int>();

                    path.Push(pos);

                    while (stepMap[pos.y, pos.x] > 1)
                    {
                        foreach (Vector2Int cell in pos.CellsAround())
                        {
                            if (stepMap[cell.y, cell.x] == stepMap[pos.y, pos.x] - 1)
                            {
                                path.Push(cell);
                                pos = cell;

                                break;
                            }
                        }
                    }
                    //path.Push(currentPos);

                    return path;
                }

                foreach (Vector2Int cell in pos.CellsAround())
                {
                    if (this[cell] == Cells.Wall || this[cell] == Cells.Exit) { continue; }
                    if (stepMap[cell.y, cell.x] <= stepMap[pos.y, pos.x]) { continue; }
                    stepMap[cell.y, cell.x] = stepMap[pos.y, pos.x] + 1;

                    positions.Push(cell);
                }
            }

            throw new InvalidOperationException("Path not found. Start: " + start + ", Target: " + target);
        }

        public Vector2Int GetRandomFreePos()
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
    }
}
