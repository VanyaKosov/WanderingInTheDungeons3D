using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingController : MonoBehaviour
{
    private const float waitBeforeOptionsAppear = 11.0f;

    public Image whiteFade;
    public Fade fade;
    public GameObject victoryOverlay;

    void Start()
    {
        fade.FadeOut(whiteFade, 1.5f);
        StartCoroutine(ShowOverlay());
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private IEnumerator ShowOverlay()
    {
        yield return new WaitForSeconds(waitBeforeOptionsAppear);
        victoryOverlay.SetActive(true);

        yield return null;
    }
}
