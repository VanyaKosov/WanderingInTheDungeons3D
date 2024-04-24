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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        //float left = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");
        this.transform.position += transform.forward * forward * playerSpeed * Time.deltaTime;

        //Vector3 offset = new Vector3(forward, 0, left);
        //this.transform.position += Vector3.Normalize(offset) * playerSpeed * Time.deltaTime;

        polar += Input.GetAxis("Mouse X") * mouseSensitivity;
        elevation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        elevation = Mathf.Max(-90f, Mathf.Min(90f, elevation));

        playerCamera.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(polar, Vector3.up);
    }
}
