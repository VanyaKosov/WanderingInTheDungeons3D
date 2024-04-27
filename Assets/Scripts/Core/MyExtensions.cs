using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public static class MyExtensions
    {
        public static int ManhattanDistance(this Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        public static IEnumerable<Vector2Int> CellsAround(this Vector2Int v)
        {
            yield return new Vector2Int(v.x - 1, v.y);
            yield return new Vector2Int(v.x + 1, v.y);
            yield return new Vector2Int(v.x, v.y - 1);
            yield return new Vector2Int(v.x, v.y + 1);
        }
    }
}
