using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
        {
            Image image = canvasGroup.GetComponent<Image>();
            image.color = backgroundColor;
            float time = 0;

            while (time <= fadeSeconds)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
                yield return null;
            }
            canvasGroup.alpha = targetFadeAlpha;
        }
    }
}
