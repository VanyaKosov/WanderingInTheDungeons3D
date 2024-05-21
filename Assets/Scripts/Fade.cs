using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Fade : MonoBehaviour
    {
        public void FadeIn(Image fadePanel, float fadeDuration)
        {
            StartCoroutine(DoFadeIn(fadePanel, fadeDuration));
        }

        private IEnumerator DoFadeIn(Image fadePanel, float fadeDuration)
        {
            while (fadePanel.color.a < 1)
            {
                Color fadeColor = fadePanel.color;
                fadeColor.a += 1 / fadeDuration * Time.deltaTime;
                fadePanel.color = fadeColor;

                yield return null;
            }
        }

        public void FadeOut(Image fadePanel, float fadeDuration)
        {
            StartCoroutine(DoFadeOut(fadePanel, fadeDuration));
        }

        private IEnumerator DoFadeOut(Image fadePanel, float fadeDuration)
        {
            while (fadePanel.color.a > 0)
            {
                Color fadeColor = fadePanel.color;
                fadeColor.a -= 1 / fadeDuration * Time.deltaTime;
                fadePanel.color = fadeColor;

                yield return null;
            }
        }
    }
}
