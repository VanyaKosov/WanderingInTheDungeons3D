using Assets.Scripts;
using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IPlayer
{
    private const int damage = 10;
    private const float attackWait = 0.4f;
    private const float attackRange = 1.5f;
    private const float attackDegreeLimit = 60.0f;

    public bool invulnerability;
    public float initialMoveSpeed;
    public Camera playerCamera;
    public float mouseSensitivity;
    public HUDController HUDController;
    public Animator weaponAnimator;
    public GameController gameController;
    private int health = 100;
    private int maxHealth = 100;
    private CharacterController characterController;
    private float polar = 0;
    private float elevation = 0;
    private Coroutine attackCorutine;
    private bool atExit = false;

    public float MoveSpeed { get; set; }

    public Vector2Int MapPos
    {
        get => Converter.WorldToMapPos(transform.position);
    }

    public int Health
    {
        get => health;
        set
        {
            if (invulnerability) { return; }

            health = Mathf.Min(value, MaxHealth);
            if (health <= 0)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
    }

    public bool AtExit
    {
        get => atExit;
    }

    private void Start()
    {
        MoveSpeed = initialMoveSpeed;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!atExit)
        {
            PlayerMove();
            PlayerLookAround();
            PlayerAttack();
        }
    }

    private void PlayerAttack()
    {
        if (attackCorutine != null) { return; }
        bool leftClicked = Input.GetMouseButtonDown(0);
        if (!leftClicked) { return; }

        attackCorutine = StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        weaponAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(attackWait);

        for (int i = 0; i < gameController.monsters.Count; i++)
        {
            GameObject monster = gameController.monsters[i];
            Vector3 monsterDirection = monster.transform.position - transform.position;

            if (monsterDirection.magnitude > attackRange) { continue; }

            float monsterDegrees = Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.forward, monsterDirection.normalized)) * Mathf.Rad2Deg);
            if (monsterDegrees <= attackDegreeLimit)
            {
                Monster monsterController = monster.GetComponent<Monster>();
                monsterController.Health -= damage;
                if (monsterController.Health <= 0)
                {
                    gameController.monsters.Remove(monster);
                }
            }
        }

        yield return WaitForAnimatorState.Do(weaponAnimator, "PlayerIdleAnimation");
        attackCorutine = null;
    }

    private void PlayerMove()
    {
        float left = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(forward, 0, left);
        if (direction.sqrMagnitude > 1)
        {
            direction = Vector3.Normalize(direction);
        }

        Vector3 offset = transform.forward * direction.x + Physics.gravity + transform.right * direction.z;

        characterController.Move(MoveSpeed * Time.deltaTime * offset);
    }

    private void PlayerLookAround()
    {
        polar += Input.GetAxis("Mouse X") * mouseSensitivity;
        elevation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        elevation = Mathf.Max(-90f, Mathf.Min(90f, elevation));

        playerCamera.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.right);

        transform.rotation = Quaternion.AngleAxis(polar, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            atExit = true;
            HUDController.fade.FadeIn(HUDController.whiteFade, 1.5f);
            StartCoroutine(LoadEndSceneInBackground());


        }
        else if (other.CompareTag("Monster"))
        {

        }
        else if (other.CompareTag("SpeedBoots"))
        {
            SpeedBoots speedBoots = new SpeedBoots();
            speedBoots.apply(this);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Radar"))
        {
            HUDController.radarIsActive = true;
            Destroy(other.gameObject);
        }
    }

    private IEnumerator LoadEndSceneInBackground()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Ending");
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            if (load.progress >= 0.9f)
            {
                if (HUDController.whiteFade.color.a >= 1.0f)
                {
                    load.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
