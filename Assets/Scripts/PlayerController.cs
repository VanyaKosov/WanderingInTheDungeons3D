using Assets.Scripts.Core;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer
{
    public bool invulnerability;
    public float initialMoveSpeed;
    public Camera playerCamera;
    public float mouseSensitivity;
    public HUDController HUDController;
    private int health = 100;
    private int maxHealth = 100;
    private CharacterController characterController;
    private float polar = 0;
    private float elevation = 0;

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

    private void Start()
    {
        MoveSpeed = initialMoveSpeed;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMove();
        PlayerLookAround();
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

        characterController.Move(offset * Time.deltaTime * MoveSpeed);
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
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if (other.CompareTag("Monster"))
        {
            /*if (!Invulnerability)
            {
                hp -= other.gameObject.GetComponent<Monster>().Damage;

                if (hp <= 0)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }*/
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
}
