using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private const int detectionRangeWorld = 12;
    //private const float attackLength = 2.467f;
    private const float attackLength = 2.0f;
    private const float attackWait = 1.1f;
    private const float attackRange = 1.5f;

    public float speed;
    public CharacterController characterController;
    public Animator animator;
    private Vector2Int mapPos;
    private Vector2Int nextMapPos;
    private Dungeon dungeon;
    private Stack<Vector2Int> path = new Stack<Vector2Int>();
    private Vector3 worldTargetPos;
    private GameObject player;
    private PlayerController playerController;
    private int damage = 10;
    private float attackState = 0;

    public int Damage
    {
        get => damage;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        Attack();
        if (attackState >= attackLength)
        {
            attackState = 0f;
        }

        if (attackState > 0f) {
            attackState += Time.deltaTime;

            return; 
        }

        Move();
    }

    private void Attack()
    {
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;

        if (attackState >= attackWait - 0.1f && attackState <= attackWait + 0.1f)
        {
            if (playerController.invulnerability) { return; }
            if (distanceToPlayer > attackRange)
            {
                return;
            }
            playerController.hp -= damage;
            if (playerController.hp <= 0)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        if (attackState > 0f)
        {
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("attack");
            attackState = 0.001f;
        }
    }

    private void Move()
    {
        Vector3? playerDirection = ChasePlayer();
        if (playerDirection != null)
        {
            characterController.SimpleMove((Vector3)playerDirection * speed);
            transform.LookAt(player.transform, Vector3.up);

            return;
        }

        if (path.Count == 0)
        {
            GenerateRandomPath();

            nextMapPos = path.Pop();
            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnOffset;
        }
        else if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            mapPos = nextMapPos;
            nextMapPos = path.Pop();

            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnOffset;
        }

        Vector3 speedOffset = CalculateOffset();

        characterController.SimpleMove(speedOffset * speed);

        transform.LookAt(transform.position + speedOffset, Vector3.up);
        //transform.LookAt(player.transform, Vector3.up);
    }

    private Vector3 CalculateOffset()
    {
        if ((player.transform.position - transform.position).magnitude < detectionRangeWorld)
        {
            path = dungeon.FindPath(mapPos, playerController.MapPos);

            nextMapPos = path.Pop();
            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnOffset;
        }

        Vector3 offset = worldTargetPos - transform.position;

        return offset.normalized;
    }

    private Vector3? ChasePlayer()
    {
        if ((playerController.transform.position - transform.position).magnitude < Converter.cellOffset)
        {
            return (player.transform.position - transform.position).normalized;
        }

        return null;
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
