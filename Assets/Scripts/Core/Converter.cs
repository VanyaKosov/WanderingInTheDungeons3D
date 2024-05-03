using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Converter
    {
        public const float cellOffset = 6;
        public const float spawnOffset = 2;

        public static Vector2Int WorldToMapPos(Vector3 worldPos)
        {
            Vector3 mapPos = worldPos / cellOffset;
            return new Vector2Int((int)mapPos.x, (int)mapPos.z);
        }

        public static Vector3 MapToWorldPos(Vector2Int mapPos)
        {
            return new Vector3(mapPos.x * cellOffset + cellOffset / 2, 0, mapPos.y * cellOffset + cellOffset / 2);
        }
    }
}
