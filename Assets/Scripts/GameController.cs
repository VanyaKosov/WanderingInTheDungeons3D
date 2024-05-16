using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject speedBootsPrefab;
    public GameObject radarPrefab;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject exitPrefab;
    public GameObject monsterPrefab;
    public GameObject player;
    public List<GameObject> monsters;
    private const float worldPlayerSpawnRadius = 12.0f;
    private Dictionary<Cells, GameObject> cellConverter;
    private Dungeon dungeon;
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
        generateItems();
    }

    private void GenerateDungeon(string[] inputDungeonMap)
    {
        //dungeon = new Dungeon(inputDungeonMap);
        dungeon = new Dungeon();
        Vector3 startPlayerPos = Converter.MapToWorldPos(dungeon.StartPlayerPos);
        startPlayerPos.y = player.transform.position.y;
        player.transform.position = startPlayerPos;

        for (int row = 0; row < dungeon.Width; row++)
        {
            for (int col = 0; col < dungeon.Width; col++)
            {
                Vector3 cellCords = Converter.MapToWorldPos(new Vector2Int(row, col));
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
            Vector3 worldPos = Converter.MapToWorldPos(randomPos);
            worldPos.y += Converter.spawnOffset;

            if ((worldPos - player.transform.position).magnitude < worldPlayerSpawnRadius) { continue; }

            monsters.Add(Instantiate(monsterPrefab, worldPos, Quaternion.identity));
            monsters[monsters.Count - 1].GetComponent<Monster>().Init(dungeon, randomPos);
        }
    }

    private void generateItems()
    {
        GameObject[] itemsToSpawn =
        {
            speedBootsPrefab,
            radarPrefab
        };

        foreach (GameObject itemPrefab in itemsToSpawn)
        {
            Vector2Int randomPos = dungeon.GetRandomFreePos();
            Vector3 worldPos = Converter.MapToWorldPos(randomPos);
            worldPos.y += Converter.spawnOffset;

            Instantiate(itemPrefab, worldPos, Quaternion.identity);
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
