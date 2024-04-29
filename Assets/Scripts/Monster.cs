using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public Vector2Int mapPos;
    public CharacterController characterController;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;
    private Vector3 targetPos;

    void Start()
    {
        path = dungeon.findPath(mapPos, dungeon.getRandomFreePos());
        mapPos = path.Pop();
        targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
    }

    void Update()
    {
        while (path.Count == 0)
        {
            var randomPos = dungeon.getRandomFreePos();
            path = dungeon.findPath(mapPos, randomPos);
        }

        if (transform.position == targetPos)
        {
            mapPos = path.Pop();
            targetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        }
        //transform.position = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);
        characterController.SimpleMove(targetPos * speed * Time.deltaTime);
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
