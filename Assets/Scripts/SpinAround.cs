using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    public float rotationSpeed = 0.3f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }
}
