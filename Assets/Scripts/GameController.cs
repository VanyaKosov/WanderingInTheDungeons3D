using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float cellOffset = 6;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject exitPrefab;
    public GameObject monsterPrefab;
    private Dictionary<Cells, GameObject> cellConverter;
    private Dungeon dungeon;
    private List<GameObject> monsters;
    private string[] testDungeonMap =
    {
        "#########",
        "# #     #",
        "# # ### #",
        "# # #   #",
        "# # #####",
        "#       #",
        "# ##### #",
        "#   #E  #",
        "#########"
    };

    private void Start()
    {
        FillCellConverter();

        GenerateDungeon(testDungeonMap);
        generateMonsters();
    }

    private void GenerateDungeon(string[] inputDungeonMap)
    {
        dungeon = new Dungeon(inputDungeonMap);

        for (int row = 0; row < dungeon.Width; row++)
        {
            for (int col = 0; col < dungeon.Width; col++)
            {
                Vector3 cellCords = new Vector3(row * cellOffset, 0, col * cellOffset);
                Instantiate(cellConverter[dungeon[col, row]], cellCords, Quaternion.identity);
            }
        }
    }

    private void generateMonsters()
    {
        monsters = new List<GameObject>();
        for (int i = 0; i < dungeon.MonsterCount; i++)
        {
            Vector2Int randomPos = dungeon.GetRandomFreePos();
            Vector3 worldPos = new Vector3(randomPos.x * cellOffset, 2, randomPos.y * cellOffset);

            monsters.Add(Instantiate(monsterPrefab, worldPos, Quaternion.identity));
            monsters[monsters.Count - 1].GetComponent<Monster>().Init(dungeon, randomPos);
        }
    }

    private void FillCellConverter()
    {
        cellConverter = new Dictionary<Cells, GameObject>()
        {
            { Cells.Empty, floorPrefab },
            { Cells.Wall, wallPrefab },
            { Cells.Exit, exitPrefab }
        };
    }
}
