    using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    internal class Dungeon
    {
        private Dictionary<char, Cells> cellConverter = new Dictionary<char, Cells>
        {
            { ' ', Cells.Empty },
            { '#', Cells.Wall },
            { 'E', Cells.Exit }
        };

        private readonly Cells[,] map;

        public List<Monster> monsters;

        private int monsterCount;

        public Dungeon(string[] inputMap)
        {
            map = TranslateMap(inputMap);
            monsters = addMonsters();
        }

        public Cells this[int row, int col]
        {
            get => map[row, col];
        }

        public int Width
        {
            get => map.GetLength(1);
        }

        public int Height
        {
            get => map.GetLength(0);
        }

        private List<Monster> addMonsters()
        {
            List<Monster> newMonsters = new List<Monster>();
            monsterCount = Width * Height / 20;

            for (int i = 0; i < monsterCount; i++)
            {
                System.Random random = new System.Random();
                int randomX = random.Next(Width);
                int randomY = random.Next(Height);
                
                if (this[randomX, randomY] != Cells.Empty)
                {
                    i--;
                    continue;
                }

                newMonsters.Add(new Monster(randomX, randomY));
            }

            return newMonsters;
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
