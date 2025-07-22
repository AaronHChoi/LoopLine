using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLevelManager : MonoBehaviour
{
    [SerializeField] FadeInOut fade;
    [SerializeField] float timePausedFadeOut;
    [SerializeField] float timeToFadeOut;
    [SerializeField] float timeLevelToFadeIn;
    [SerializeField] float timeToFadeIn;
    [SerializeField] float timeToChangeLevel;
    [SerializeField] string nextSceneName;
    void Start()
    {
        fade.PauseFade(timePausedFadeOut);
        fade.FadeOut(timeToFadeOut);
        StartCoroutine(FadeOutOnTime(timeLevelToFadeIn));
        StartCoroutine(ChangeNextLevel(timeToChangeLevel));
    }
    private IEnumerator FadeOutOnTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fade.FadeIn(timeToFadeIn);
    }
    private IEnumerator ChangeNextLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(nextSceneName);
    }
}
