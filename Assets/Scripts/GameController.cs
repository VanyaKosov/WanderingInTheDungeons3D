using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject floor;

    public GameObject wall;

    public GameObject exit;

    private Dictionary<Cells, GameObject> cellConverter;

    public float cellOffset;

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

    private Dungeon dungeon;

    private void Start()
    {
        FillCellConverter();

        GenerateDungeon(testDungeonMap);
    }

    private void GenerateDungeon(string[] inputDungeonMap)
    {
        dungeon = new Dungeon(testDungeonMap);

        for (int row = 0; row < dungeon.Width; row++)
        {
            for (int col = 0; col < dungeon.Height; col++)
            {
                Vector3 cellCords = new Vector3(row * cellOffset, 0, col * cellOffset);
                Instantiate(cellConverter[dungeon[row, col]], cellCords, Quaternion.identity);
            }
        }
    }

    private void FillCellConverter()
    {
        cellConverter = new Dictionary<Cells, GameObject>();

        cellConverter.Add(Cells.Empty, floor);
        cellConverter.Add(Cells.Wall, wall);
        cellConverter.Add(Cells.Exit, exit);
    }
}
