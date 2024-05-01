using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public Vector2Int mapPos;
    public Vector2Int nextMapPos;
    public CharacterController characterController;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;
    public Vector3 worldTargetPos;

    void Start()
    {
        path = dungeon.findPath(mapPos, dungeon.getRandomFreePos());
        //path = dungeon.findPath(mapPos, new Vector2Int(7, 7));

        print(path.Count);

        //mapPos = path.Pop();
        nextMapPos = path.Pop();
        worldTargetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
    }

    void Update()
    {
        while (path.Count == 0)
        {
            var randomPos = dungeon.getRandomFreePos();
            path = dungeon.findPath(mapPos, randomPos);
        }

        //print((int)transform.position.x + " " + (int)worldTargetPos.x + " " + (int)transform.position.z + " " + (int)worldTargetPos.z);
        //if (((int)transform.position.x == (int)worldTargetPos.x) && ((int)transform.position.z == (int)worldTargetPos.z))
        if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            mapPos = nextMapPos;
            nextMapPos = path.Pop();

            worldTargetPos = new Vector3(nextMapPos.x * GameController.cellOffset, 2, nextMapPos.y * GameController.cellOffset);

            //mapPos = path.Pop();
            //targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        }

        //characterController.SimpleMove(targetPos * speed * Time.deltaTime);
        var offset = new Vector3((nextMapPos.x - mapPos.x) * GameController.cellOffset, 2,
                (nextMapPos.y - mapPos.y) * GameController.cellOffset);
        characterController.SimpleMove(offset * speed * Time.deltaTime);
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
