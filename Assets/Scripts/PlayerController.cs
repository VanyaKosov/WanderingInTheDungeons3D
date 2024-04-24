using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;

    public Camera playerCamera;

    public float mouseSensitivity;

    private float polar = 0;

    private float elevation = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    void LateUpdate()
    {
        PlayerMove();

        PlayerLookAround();
    }

    private void PlayerMove()
    {
        float left = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(forward, 0, left);
        if (direction.magnitude > 1 || direction.magnitude < -1)
        {
            direction = Vector3.Normalize(direction);
        }
        transform.position += transform.forward * direction.x * playerSpeed * Time.deltaTime;
        transform.position += transform.right * direction.z * playerSpeed * Time.deltaTime;
    }

    private void PlayerLookAround()
    {
        polar += Input.GetAxis("Mouse X") * mouseSensitivity;
        elevation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        elevation = Mathf.Max(-90f, Mathf.Min(90f, elevation));

        playerCamera.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(polar, Vector3.up);
    }
}
