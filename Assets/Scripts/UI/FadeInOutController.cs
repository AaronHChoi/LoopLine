using System.Collections;
using UnityEngine;

public class FadeInOutController : MonoBehaviour
{
    [SerializeField] private FadeInOutSO settings;

    private CanvasGroup canvasGroup;
    private Coroutine forcedCoroutine;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        if (settings.StartsWithFadeState != FadeState.Force)
        {
            StartCoroutine(InitFadeSecuence());
        }
    }

    private IEnumerator InitFadeSecuence()
    {
        int loopNum = 0;
        while (loopNum < settings.AmmountFadeLoops)
        {
            switch (settings.StartsWithFadeState)
            {
                case FadeState.FadeIn:
                    yield return FadeProcess(settings.FadeIn, FadeState.FadeIn);
                    yield return FadeProcess(settings.FadeOut, FadeState.FadeOut);
                    break;
                case FadeState.FadeOut:
                    yield return FadeProcess(settings.FadeOut, FadeState.FadeOut);
                    yield return FadeProcess(settings.FadeIn, FadeState.FadeIn);
                    break;
                default:
                    break;
            }
            loopNum++;
        }

        if (settings.DisableOnFinish) gameObject.SetActive(false);
    }
    private IEnumerator FadeProcess(FadeTiming fadeTiming, FadeState fadeState)
    {
        canvasGroup.alpha = fadeState == FadeState.FadeIn ? 0f : 1f;

        if (fadeTiming.TimeBeforeFade > 0)
        {
            yield return new WaitForSeconds(fadeTiming.TimeBeforeFade);
        }

        yield return Fade(fadeTiming.FadeTime, fadeState);

        if (settings.StartsWithFadeState == FadeState.Force) forcedCoroutine = null;
    }

    private IEnumerator Fade(float fadeTime, FadeState fadeState)
    {
        float timer = 0;
        float startAlpha = fadeState == FadeState.FadeIn ? 0f : 1f;
        float endAlpha = fadeState == FadeState.FadeIn ? 1f : 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeTime);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
    public void ForceFade(bool isFadeIn)
    {
        if (forcedCoroutine == null && settings.StartsWithFadeState == FadeState.Force)
        {
            if (isFadeIn)
            {
                forcedCoroutine = StartCoroutine(FadeProcess(settings.FadeIn, FadeState.FadeIn));
            }
            else
            {
                forcedCoroutine = StartCoroutine(FadeProcess(settings.FadeOut, FadeState.FadeOut));
            }
        }
    }
}