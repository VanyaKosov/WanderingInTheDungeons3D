using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject floor;

    public GameObject wall;

    public int segmentLength;

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
        for (int row = 0; row > dungeonMap.Length; row++)
        {
            print(row);
            for (int col = 0; col < dungeonMap[0].Length; col++)
            {
                print(col);
                char segment = dungeonMap[row][col];
                if (segment == ' ')
                {
                    Instantiate(floor, new Vector3(row * segmentLength, 0, col * segmentLength),
                        Quaternion.identity);
                }
                else if (segment == '*')
                {
                    Instantiate(wall, new Vector3(row * segmentLength, 0, col * segmentLength),
                        Quaternion.identity);
                }
            }
        }
    }
}
