using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    CanvasGroup canvasGroup;
    float timeToFade;
    float pauseTime;
    bool fadeIn;
    bool fadeOut;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (pauseTime > 0)
        {
            pauseTime -= Time.deltaTime;
        }
        else
        {
            if (fadeIn)
            {
                if (canvasGroup.alpha < 1)
                {
                    canvasGroup.alpha += (1 / timeToFade) * Time.deltaTime;
                }
                else
                {
                    canvasGroup.alpha = 1;
                    fadeIn = false;
                }
            }
            else if (fadeOut)
            {
                if (canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha -= (1 / timeToFade) * Time.deltaTime;
                }
                else
                {
                    canvasGroup.alpha = 0;
                    fadeOut = false;
                }
            }
        }
    }
    public void FadeIn(float timeSeconds)
    {
        if (!fadeIn)
        {
            canvasGroup.alpha = 0;
            timeToFade = timeSeconds;
            fadeIn = true;
        }
    }
    public void FadeOut(float timeSeconds)
    {
        if (!fadeOut)
        {
            canvasGroup.alpha = 1;
            timeToFade = timeSeconds;
            fadeOut = true;
        }
    }
    public void PauseFade(float timeSeconds)
    {
        pauseTime = timeSeconds;
    }
}
