using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public Camera playerCamera;
    public float mouseSensitivity;
    private CharacterController characterController;
    private float polar = 0;
    private float elevation = 0;
    private Vector2Int mapPos;

    public Vector2Int MapPos
    {
        get => mapPos;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        UpdateMapPos();
    }

    private void Update()
    {
        PlayerMove();
        PlayerLookAround();
        UpdateMapPos();
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

        characterController.Move(offset * Time.deltaTime * playerSpeed);
    }

    private void UpdateMapPos()
    {
        int x = Mathf.FloorToInt(transform.position.x / GameController.cellOffset);
        int y = Mathf.FloorToInt(transform.position.y / GameController.cellOffset);
        mapPos = new Vector2Int(x, y);
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
    }
}
