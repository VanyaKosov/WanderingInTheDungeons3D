using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    public Image whiteFade;
    public Fade fade;

    void Start()
    {
        fade.FadeOut(whiteFade, 1.5f);
    }

    void Update()
    {

    }
}
