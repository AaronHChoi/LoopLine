using System.Collections;
using UnityEngine;

public class IntroLevelManager : MonoBehaviour
{
    [SerializeField] FadeInOut fade;
    [SerializeField] float timePausedFadeOut;
    [SerializeField] float timeToFadeOut;
    [SerializeField] float timeLevelToFadeIn;
    [SerializeField] float timeToFadeIn;
    void Start()
    {
        fade.PauseFade(timePausedFadeOut);
        fade.FadeOut(timeToFadeOut);
        StartCoroutine(FadeOutOnTime(timeLevelToFadeIn));
    }
    private IEnumerator FadeOutOnTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fade.FadeIn(timeToFadeIn);
    }
}
