using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public float playerSpeed;

    //public Camera playerCamera;

    //public float mouseSensitivity;

    //private float polar = 0;

    //private float elevation = 0;

    //private Rigidbody playerRigidbody;

    private void Start()
    {
        //playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //PlayerMove();
    }

    private void LateUpdate()
    {
        //PlayerLookAround();
    }

    /*private void PlayerMove()
    {
        float left = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(forward, 0, left);
        if (direction.sqrMagnitude > 1)
        {
            direction = Vector3.Normalize(direction);
        }

        Vector3 newPos = transform.position +
           transform.forward * direction.x * playerSpeed * Time.deltaTime +
           transform.right * direction.z * playerSpeed * Time.deltaTime;


        playerRigidbody.MovePosition(newPos);
    }

    private void PlayerLookAround()
    {
        polar += Input.GetAxis("Mouse X") * mouseSensitivity;
        elevation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        elevation = Mathf.Max(-90f, Mathf.Min(90f, elevation));

        playerCamera.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.right);
        //playerCamera.transform.localRotation = Quaternion.AngleAxis(polar, Vector3.up) * r;
        //playerRigidbody.MoveRotation(Quaternion.AngleAxis(polar, Vector3.up));
        //transform.rotation = Quaternion.AngleAxis(polar, Vector3.up);
        playerRigidbody.MoveRotation(Quaternion.AngleAxis(polar, Vector3.up));
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
