using Assets.Scripts;
using Assets.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private const int detectionRangeMap = 3;
    private const float attackWait = 0.9f;
    private const float attackRange = 1.5f;
    private const float attackDegreeLimit = 80.0f;

    public float speed;
    public CharacterController characterController;
    public Animator animator;
    private int health = 100;
    private int MaxHealth = 100;
    private Vector2Int nextMapPos;
    private Dungeon dungeon;
    private Stack<Vector2Int> path = new Stack<Vector2Int>();
    private Vector3 worldTargetPos;
    private GameObject player;
    private PlayerController playerController;
    private int damage = 10;
    private Coroutine attackCoroutine;
    private bool dead = false;

    public int Damage
    {
        get => damage;
    }

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Min(value, MaxHealth);
            if (health <= 0)
            {
                //Destroy(gameObject);
                dead = true;
                animator.SetBool("dead", true);
                CapsuleCollider collider = GetComponent<CapsuleCollider>();
                collider.enabled = false;
                characterController.enabled = false;
            }
        }
    }

    public Vector2Int MapPos
    {
        get => Converter.WorldToMapPos(transform.position);
    }

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (dead) { return; }

        if (playerController.playerIsDead)
        {
            //animator.GetCurrentAnimatorClipInfo(0)[0].clip.sp;
        }

        if (!playerController.AtExit)
        {
            Attack();
            if (attackCoroutine != null) { return; }
            Move();
        }
    }

    private void Attack()
    {
        if (attackCoroutine != null) { return; }
        if ((player.transform.position - transform.position).magnitude > attackRange) { return; }

        attackCoroutine = StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(attackWait);

        Vector3 playerDirection = player.transform.position - transform.position;
        if (playerDirection.magnitude < attackRange)
        {
            float playerDegrees = Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward, playerDirection.normalized)) * Mathf.Rad2Deg);
            if (playerDegrees <= attackDegreeLimit)
            {
                playerController.Health -= damage;
            }
        }

        yield return WaitForAnimatorState.Do(animator, "run");
        attackCoroutine = null;
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
            worldTargetPos.y += Converter.spawnVerticalOffset;
        }
        else if ((transform.position - worldTargetPos).sqrMagnitude <= 1)
        {
            nextMapPos = path.Pop();

            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += transform.position.y;
        }

        Vector3 speedOffset = CalculateOffset();

        characterController.SimpleMove(speedOffset * speed);

        transform.LookAt(transform.position + speedOffset, Vector3.up);
    }

    private Vector3 CalculateOffset()
    {
        if (playerController.MapPos.ManhattanDistance(MapPos) <= detectionRangeMap)
        {
            path = dungeon.FindPath(MapPos, playerController.MapPos);

            nextMapPos = path.Pop();
            worldTargetPos = Converter.MapToWorldPos(nextMapPos);
            worldTargetPos.y += Converter.spawnVerticalOffset;
        }

        Vector3 offset = worldTargetPos - transform.position;

        return offset.normalized;
    }

    private Vector3? ChasePlayer()
    {
        if ((playerController.transform.position - transform.position).magnitude < Converter.cellOffset || playerController.MapPos == MapPos)
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
        } while (randomPos == MapPos);
        path = dungeon.FindPath(MapPos, randomPos);
    }

    public void Init(Dungeon dungeon)
    {
        this.dungeon = dungeon;
    }
}
