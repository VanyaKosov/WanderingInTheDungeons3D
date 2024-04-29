using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public Vector2Int mapPos;
    public Vector2Int moveMapPos;
    public CharacterController characterController;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;
    public Vector3 targetPos;

    void Start()
    {
        //path = dungeon.findPath(mapPos, dungeon.getRandomFreePos());
        path = dungeon.findPath(mapPos, new Vector2Int(7, 7));
        //mapPos = path.Pop();
        moveMapPos = path.Pop();
        //targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        targetPos = new Vector3(mapPos.y * GameController.cellOffset, 2, mapPos.x * GameController.cellOffset);

        //targetPos = new Vector3((moveMapPos.x - mapPos.x) * GameController.cellOffset, 2,
        //        (moveMapPos.y - mapPos.y) * GameController.cellOffset);
        //targetPos = new Vector3((moveMapPos.y - mapPos.y) * GameController.cellOffset, 2,
        //        (moveMapPos.x - mapPos.x) * GameController.cellOffset);
    }

    void Update()
    {
        /*while (path.Count == 0)
        {
            var randomPos = dungeon.getRandomFreePos();
            path = dungeon.findPath(mapPos, randomPos);
        }*/

        if (((int)transform.position.x == (int)targetPos.x) && ((int)transform.position.z == (int)targetPos.z))
        {
            mapPos = moveMapPos;
            moveMapPos = path.Pop();

            //targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
            targetPos = new Vector3(mapPos.y * GameController.cellOffset, 2, mapPos.x * GameController.cellOffset);
            //targetPos = new Vector3((mapPos.x - moveMapPos.x) * GameController.cellOffset, 2, 
            //    (mapPos.y - moveMapPos.y) * GameController.cellOffset);

            //mapPos = path.Pop();
            //targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        }
        //transform.position = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);

        var v = new Vector3((moveMapPos.y - mapPos.y) * GameController.cellOffset, 2,
                (moveMapPos.x - mapPos.x) * GameController.cellOffset);
        //characterController.SimpleMove(targetPos * speed * Time.deltaTime);
        characterController.SimpleMove(v * speed * Time.deltaTime);
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
