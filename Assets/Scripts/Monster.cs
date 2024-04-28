using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Vector2Int mapPos;
    public CharacterController characterController;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;

    void Start()
    {
        path = dungeon.findPath(mapPos, dungeon.getRandomFreePos());
    }

    void Update()
    {
        while (path.Count == 0)
        {
            var randomPos = dungeon.getRandomFreePos();
            path = dungeon.findPath(mapPos, randomPos);
        }

        mapPos = path.Pop();
        transform.position = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        //characterController.SimpleMove()
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
