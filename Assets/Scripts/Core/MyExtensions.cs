using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class MyExtensions
    {
        private static readonly System.Random randgen = new System.Random();

        public static int ManhattanDistance(this Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public static IEnumerable<Vector2Int> CellsAround(this Vector2Int currentPos)
        {
            yield return new Vector2Int(currentPos.x - 1, currentPos.y);
            yield return new Vector2Int(currentPos.x + 1, currentPos.y);
            yield return new Vector2Int(currentPos.x, currentPos.y - 1);
            yield return new Vector2Int(currentPos.x, currentPos.y + 1);
        }

        public static IEnumerable<Vector2Int> RandomCellsAround(this Vector2Int currentPos)
        {
            List<Vector2Int> cellsAround = new List<Vector2Int>();
            foreach (Vector2Int pos in currentPos.CellsAround())
            {
                cellsAround.Add(pos);
            }

            for (int i = 0; i < 4; i++)
            {
                int randomIndex = randgen.Next(cellsAround.Count);
                yield return cellsAround[randomIndex];
                cellsAround.RemoveAt(randomIndex);
            }
        }
    }
}
