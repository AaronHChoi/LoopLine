using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FadeInOutController : MonoBehaviour
    {
        [SerializeField] private FadeInOutSO settings;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        private void OnEnable()
        {
            int loopNum = 1;
            while (loopNum < settings.AmmountFadeLoops)
            {
                switch (settings.StartsWithFadeState)
                {
                    case FadeState.FadeIn:
                        StartCoroutine(InitFadeSecuence(settings.FadeIn, FadeState.FadeIn));
                        StartCoroutine(InitFadeSecuence(settings.FadeOut, FadeState.FadeOut));
                        break;
                    case FadeState.FadeOut:
                        StartCoroutine(InitFadeSecuence(settings.FadeOut, FadeState.FadeOut));
                        StartCoroutine(InitFadeSecuence(settings.FadeIn, FadeState.FadeIn));
                        break;
                    default:
                        break;
                }
                loopNum++;
            }
            if (settings.DisableOnFinish) gameObject.SetActive(false);
        }

        private IEnumerator InitFadeSecuence(FadeTiming fadeTiming, FadeState fadeState)
        {
            yield return StartCoroutine(WaitTime(fadeTiming.TimeBeforeFade));

            yield return StartCoroutine(Fade(fadeTiming.FadeTime, fadeState));
        }
        private IEnumerator WaitTime(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        private IEnumerator Fade(float fadeTime, FadeState fadeState)
        {
            float timer = 0;
            float startAlpha = fadeState == FadeState.FadeIn ? 0f : 1f;
            float endAlpha = fadeState == FadeState.FadeIn ? 1f : 0f;

            canvasGroup.alpha = startAlpha;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeTime);
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }
        }
    }
}
