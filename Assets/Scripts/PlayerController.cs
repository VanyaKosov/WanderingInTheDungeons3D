using Assets.Scripts;
using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IPlayer
{
    private const float animationBlendSpeed = 1.0f;
    private const int damage = 20;
    private const float attackWait = 0.8f; // 0.4f
    private const float attackRange = 2.0f;
    private const float attackDegreeLimit = 70.0f;

    public bool invulnerability;
    public float initialMoveSpeed;
    public float initialStepWait = 0.4f;
    public float mouseSensitivity;
    public HUDController HUDController;
    public GameController gameController;
    public AudioController audioController;
    public Camera playerCamera;
    public Animator weaponAnimator;
    public Animator playerAnimator;
    public GameObject pauseOverlay;
    public FlickerLight lightFlicker;
    public GameObject cameraRootPos;
    public AudioClip[] stepSounds;
    private int health = 100; // Normal 100
    private int maxHealth = 100; // Normal 100
    private CharacterController characterController;
    private float polar = 0;
    private float elevation = 0;
    private Coroutine attackCorutine;
    private bool atExit = false;

    public float MoveSpeed { get; set; }

    public float StepWait { get; set; }

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
            health = Mathf.Max(health, 0);
            if (health <= 0)
            {
                PlayerDeath();
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
        StepWait = initialStepWait;
        characterController = GetComponent<CharacterController>();
        StartCoroutine(PlayStepSounds());
    }

    private void Update()
    {
        TogglePauseMenu();
        if (gameController.Paused) { return; }

        if (!atExit)
        {
            PlayerLookAround();
            if (attackCorutine != null) { return; }
            PlayerAttack();
            PlayerMove();
        }
    }

    private void PlayerAttack()
    {
        bool leftClicked = Input.GetMouseButtonDown(0);
        if (!leftClicked) { return; }

        attackCorutine = StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        //weaponAnimator.SetTrigger("attack");
        //playerAnimator.SetTrigger("attack");
        playerAnimator.SetTrigger("attack3");
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

        //yield return WaitForAnimatorState.Do(weaponAnimator, "PlayerIdleAnimation");
        yield return WaitForAnimatorState.Do(playerAnimator, "Base State");
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

        Vector3 offset = (transform.forward * direction.x + Physics.gravity + transform.right * direction.z) * MoveSpeed;
        characterController.Move(Time.deltaTime * offset);

        Vector3 localOffset = transform.worldToLocalMatrix.MultiplyVector(offset);
        float velocityX = Mathf.Lerp(playerAnimator.GetFloat("xVelocity"), localOffset.x, animationBlendSpeed);
        float velocityY = Mathf.Lerp(playerAnimator.GetFloat("yVelocity"), localOffset.z, animationBlendSpeed);
        playerAnimator.SetFloat("xVelocity", velocityX); // localOffset.x
        playerAnimator.SetFloat("yVelocity", velocityY); // localOffset.z
    }

    private void PlayerLookAround()
    {
        playerCamera.transform.position = cameraRootPos.transform.position;

        polar += Input.GetAxis("Mouse X") * mouseSensitivity;
        elevation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        elevation = Mathf.Max(-90f, Mathf.Min(90f, elevation));

        playerCamera.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.right);

        transform.rotation = Quaternion.AngleAxis(polar, Vector3.up);
    }

    private void TogglePauseMenu()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) { return; }

        if (gameController.Paused)
        {
            gameController.Paused = false;
            pauseOverlay.SetActive(false);

            return;
        }

        gameController.Paused = true;
        pauseOverlay.SetActive(true);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
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

    private void PlayerDeath()
    {
        gameController.Paused = true;
        HUDController.fade.FadeIn(HUDController.blackFade, 1.5f);
        StartCoroutine(LoadDeathSceneInBackground());
    }

    private IEnumerator LoadDeathSceneInBackground()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Death");
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            if (load.progress >= 0.9f)
            {
                if (HUDController.blackFade.color.a >= 1.0f)
                {
                    load.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    private IEnumerator PlayStepSounds()
    {
        while (true)
        {
            while (true)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if (h != 0 || v != 0) { break; }

                yield return null;
            }

            int randomIndex = Random.Range(0, stepSounds.Length);
            AudioClip randomSound = stepSounds[randomIndex];
            audioController.PlaySound(randomSound, transform, 0.03f);

            yield return new WaitForSeconds(StepWait);
        }
    }
}
