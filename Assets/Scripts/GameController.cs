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
    public GameObject mazePiecesParent;
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

        GenerateDungeon();
        GenerateMonsters();
        generateItems();
    }

    private void GenerateDungeon(string[] inputDungeonMap)
    {
        dungeon = new Dungeon(inputDungeonMap);

        for (int row = 0; row < dungeon.Width; row++)
        {
            for (int col = 0; col < dungeon.Width; col++)
            {
                Vector3 cellCords = Converter.MapToWorldPos(new Vector2Int(row, col));
                Instantiate(cellConverter[dungeon[col, row]], cellCords, Quaternion.identity);
            }
        }
    }

    private void GenerateDungeon()
    {
        dungeon = new Dungeon();
        Vector3 startPlayerPos = Converter.MapToWorldPos(dungeon.StartPlayerPos);
        startPlayerPos.y = player.transform.position.y;
        player.transform.position = startPlayerPos;

        for (int row = 0; row < dungeon.Width; row++)
        {
            for (int col = 0; col < dungeon.Width; col++)
            {
                Vector3 cellCords = Converter.MapToWorldPos(new Vector2Int(row, col));
                Instantiate(cellConverter[dungeon[col, row]], cellCords, Quaternion.identity, mazePiecesParent.transform);
            }
        }

        Vector2Int portalPos = GetRandomFarFromPlayerPos();
        Instantiate(exitPrefab, Converter.MapToWorldPos(portalPos), Quaternion.identity);
    }

    private void GenerateMonsters()
    {
        monsters = new List<GameObject>();
        for (int i = 0; i < dungeon.MonsterCount; i++)
        {
            Vector2Int randomPos = GetRandomFarFromPlayerPos();
            Vector3 worldPos = Converter.MapToWorldPos(randomPos);
            worldPos.y += Converter.spawnVerticalOffset;

            if ((worldPos - player.transform.position).magnitude < worldPlayerSpawnRadius) { continue; }

            monsters.Add(Instantiate(monsterPrefab, worldPos, Quaternion.identity));
            monsters[monsters.Count - 1].GetComponent<Monster>().Init(dungeon);
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
            Vector2Int randomPos = GetRandomFarFromPlayerPos();
            Vector3 worldPos = Converter.MapToWorldPos(randomPos);
            worldPos.y += Converter.spawnVerticalOffset;

            Instantiate(itemPrefab, worldPos, Quaternion.identity);
        }
    }

    private Vector2Int GetRandomFarFromPlayerPos()
    {
        while (true)
        {
            Vector2Int randomPos = dungeon.GetRandomFreePos();
            Vector3 randomWorldPos = Converter.MapToWorldPos(randomPos);
            if ((randomWorldPos - player.transform.position).magnitude < worldPlayerSpawnRadius) { continue; }

            return randomPos;
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
