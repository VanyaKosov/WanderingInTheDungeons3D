using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    internal class Dungeon
    {
        private const int cellsPerMonster = 10;

        private System.Random randgen = new System.Random();
        private Dictionary<char, Cells> cellConverter = new Dictionary<char, Cells>
        {
            { ' ', Cells.Empty },
            { '#', Cells.Wall },
            { 'E', Cells.Exit }
        };
        private readonly Cells[,] map;
        private int monsterCount;

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

        public Vector2Int getRandomFreePos()
        {
            while (true)
            {
                Vector2Int cords = new Vector2Int(randgen.Next(Width), randgen.Next(Height));

                if (this[cords.x, cords.y] != Cells.Empty)
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
