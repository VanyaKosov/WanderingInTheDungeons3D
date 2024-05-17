using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Run");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Run()
    {
        print("start");
        CharacterController controller = GetComponent<CharacterController>();
        var speed = new Vector3(0, 0, -1);

        for (var i = 0; i < 400; i++)
        {
            controller.SimpleMove(speed);
            yield return null;
        }

        print("stop");
    }
}
