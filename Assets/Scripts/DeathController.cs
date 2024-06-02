using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathController : MonoBehaviour
{
    private const float waitBeforeOptionsAppear = 4.0f;

    public Image blackFade;
    public Fade fade;
    public GameObject deathOverlay;

    void Start()
    {
        fade.FadeOut(blackFade, 1.5f);
        StartCoroutine(ShowOverlay());
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Utils.CloseApplication();
    }

    private IEnumerator ShowOverlay()
    {
        yield return new WaitForSeconds(waitBeforeOptionsAppear);
        deathOverlay.SetActive(true);

        yield return null;
    }
}
