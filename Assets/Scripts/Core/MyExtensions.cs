using System.Collections.Generic;
using System.Linq;
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
            List<Vector2Int> cellsAround = currentPos.CellsAround().ToList();

            while (cellsAround.Count > 0)
            {
                int randomIndex = randgen.Next(cellsAround.Count);
                yield return cellsAround[randomIndex];
                cellsAround.RemoveAt(randomIndex);
            }
        }
    }
}
