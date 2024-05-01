using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public CharacterController characterController;
    private Vector2Int mapPos;
    private Vector2Int nextMapPos;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;
    private Vector3 worldTargetPos;

    void Start()
    {
        path = dungeon.findPath(mapPos, dungeon.getRandomFreePos());

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

        if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            mapPos = nextMapPos;
            nextMapPos = path.Pop();

            worldTargetPos = new Vector3(nextMapPos.x * GameController.cellOffset, 2, nextMapPos.y * GameController.cellOffset);
        }

        Vector3 offset = worldTargetPos - transform.position;

        characterController.SimpleMove(offset.normalized * speed);
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
