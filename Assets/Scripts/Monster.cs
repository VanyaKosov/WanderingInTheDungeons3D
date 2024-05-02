using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private const int detectionRange = 2;

    public float speed;
    public CharacterController characterController;
    private Vector2Int mapPos;
    private Vector2Int nextMapPos;
    private Dungeon dungeon;
    private Stack<Vector2Int> path;
    private Vector3 worldTargetPos;
    private GameObject player;
    private PlayerController playerController;

    void Start()
    {
        path = dungeon.FindPath(mapPos, dungeon.GetRandomFreePos());

        nextMapPos = path.Pop();
        worldTargetPos = new Vector3(mapPos.x * GameController.cellOffset, 2, mapPos.y * GameController.cellOffset);

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        while (path.Count == 0)
        {
            var randomPos = dungeon.GetRandomFreePos();
            path = dungeon.FindPath(mapPos, randomPos);
        }

        if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            mapPos = nextMapPos;
            nextMapPos = path.Pop();

            worldTargetPos = new Vector3(nextMapPos.x * GameController.cellOffset, 2, nextMapPos.y * GameController.cellOffset);
        }

        //Vector3 speedOffset = ChasePlayer();
        Vector3 speedOffset = CalculateOffset();

        characterController.SimpleMove(speedOffset.normalized * speed);
    }

    private Vector3 CalculateOffset()
    {
        Vector3 offset = worldTargetPos - transform.position;

        if ((player.transform.position - transform.position).magnitude < detectionRange)
        {
            offset = player.transform.position - transform.position;

            /*if (mapPos == playerController.MapPos)
            {
                
            }*/
        }

        return offset;
    }

    /*private Vector3 ChasePlayer()
    {
        Vector2Int playerMapPos = player.GetComponent<PlayerController>().MapPos;
        int mapDistanceToPlayer = mapPos.ManhattanDistance(playerMapPos);
        if (mapDistanceToPlayer > detectionRange)
        {
            return worldTargetPos - transform.position;
        }

        if (mapPos != playerMapPos)
        {
            path = dungeon.FindPath(mapPos, playerMapPos);
            nextMapPos = path.Pop();
            worldTargetPos = new Vector3(nextMapPos.x * GameController.cellOffset, 2, nextMapPos.y * GameController.cellOffset);

            // Choose new random pos

            return worldTargetPos - transform.position;
        }

        return player.transform.position - transform.position;
    }*/

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
