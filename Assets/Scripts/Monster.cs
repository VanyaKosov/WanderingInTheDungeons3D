using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private const int detectionRangeWorld = 12;

    public float speed;
    public CharacterController characterController;
    private Vector2Int mapPos;
    private Vector2Int nextMapPos;
    private Dungeon dungeon;
    private Stack<Vector2Int> path = new Stack<Vector2Int>();
    private Vector3 worldTargetPos;
    private GameObject player;
    private PlayerController playerController;

    void Start()
    {
        //GenerateRandomPath();

        //nextMapPos = path.Pop();
        //worldTargetPos = Converter.MapToWorldPos(nextMapPos);
        //worldTargetPos.y += Converter.spawnOffset;

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector3 playerDirection = ChasePlayer();
        if (playerDirection != new Vector3(0, 0, 0))
        {
            characterController.SimpleMove(playerDirection * speed);

            return;
        }

        if (path.Count == 0)
        {
            GenerateRandomPath();
        }

        if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            mapPos = nextMapPos;
            nextMapPos = path.Pop();

            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnOffset;
        }

        Vector3 speedOffset = CalculateOffset();

        characterController.SimpleMove(speedOffset * speed);
    }

    private Vector3 CalculateOffset()
    {
        if ((player.transform.position - transform.position).magnitude < detectionRangeWorld)
        {
            path = dungeon.FindPath(mapPos, playerController.MapPos);

            nextMapPos = path.Pop(); // Sometimes causes empty stack exception
            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnOffset;
        }

        Vector3 offset = worldTargetPos - transform.position;

        return offset.normalized;
    }

    private Vector3 ChasePlayer()
    {
        Vector3 playerDirection = new Vector3(0, 0, 0);

        if (playerController.MapPos == mapPos)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }

        return playerDirection;
    }

    private void GenerateRandomPath()
    {
        Vector2Int randomPos;
        do
        {
            randomPos = dungeon.GetRandomFreePos();
        } while (randomPos == mapPos);
        path = dungeon.FindPath(mapPos, randomPos);
    }

    public void Init(Dungeon dungeon, Vector2Int mapPos)
    {
        this.dungeon = dungeon;
        this.mapPos = mapPos;
    }
}
