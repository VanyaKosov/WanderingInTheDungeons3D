using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;   

    public Camera playerCamera;

    public float cameraOffset;

    public float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera.transform.position = new Vector3(this.transform.position.x, 
            this.transform.position.y + cameraOffset, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 offset = new Vector3(horizontal, 0, vertical);

        this.transform.position += Vector3.Normalize(offset) * playerSpeed * Time.deltaTime;

        playerCamera.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + cameraOffset, this.transform.position.z);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        playerCamera.transform.Rotate(playerCamera.transform.right * mouseY * cameraSpeed);
        playerCamera.transform.Rotate(-playerCamera.transform.up * mouseX * cameraSpeed);
    }
}
