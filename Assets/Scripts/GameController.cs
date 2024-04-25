using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject floor;

    public GameObject wall;

    public float segmentHorizontalOffset;

    private string[] dungeonMap =
    {
        "************",
        "* *   *  *  ",
        "* * * **   *",
        "*   *    * *",
        "************"
    };

    void Start()
    {
        GenerateDungeon(dungeonMap);
    }

    void Update()
    {

    }

    private void GenerateDungeon(string[] dungeonMap)
    {
        for (int row = 0; row < dungeonMap.Length; row++)
        {
            for (int col = 0; col < dungeonMap[0].Length; col++)
            {
                char segment = dungeonMap[row][col];
                Vector3 segmentCords = new Vector3(row * segmentHorizontalOffset, 0, col * segmentHorizontalOffset);
                if (segment == ' ')
                {
                    Instantiate(floor, segmentCords, Quaternion.identity);
                }
                else if (segment == '*')
                {
                    Instantiate(wall, segmentCords, Quaternion.identity);
                }
            }
        }
    }
}
