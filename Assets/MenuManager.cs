using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] FadeInOut fade;
    [SerializeField] float timePausedFadeOut;
    [SerializeField] float timeToFadeOut;
    [SerializeField] float timeToFadeIn;
    [SerializeField] float timeToChangeSceneAfterFadeIn;
    [SerializeField] string nextSceneName;

    [Header("Sound")]
    [SerializeField] SoundData clickSoundData;
    [SerializeField] AudioSource bgmAudio;

    [Header ("Flash")]
    [SerializeField] CanvasGroup flashCanvasGroup;
    [SerializeField] float flashTotalTime;

    [Header("Cursor")]
    [SerializeField] Texture2D cursorTexture;

    private bool buttonsAllowed = false;
    private bool isFlashing = false;
    private bool isGrowing = true;
    private bool isDecreasingVolume = false;
    private float bgmVolumeBase;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        if (fade != null)
        {
            StartCoroutine(AllowButtons(timePausedFadeOut + timeToFadeOut));
            fade.PauseFade(timePausedFadeOut);
            fade.FadeOut(timeToFadeOut);
        }
        bgmVolumeBase = bgmAudio.volume;
    }
    private void Update()
    {
        if (isFlashing)
        {
            if (isGrowing)
            {
                flashCanvasGroup.alpha += Time.deltaTime * (flashTotalTime / 2.0f);
                if (flashCanvasGroup.alpha > 1) flashCanvasGroup.alpha = 1;
            }
            else
            {
                flashCanvasGroup.alpha -= Time.deltaTime * (flashTotalTime / 2.0f);
                if (flashCanvasGroup.alpha < 0) flashCanvasGroup.alpha = 0;
            }
        }
        if (isDecreasingVolume)
        {
            bgmAudio.volume -= (bgmVolumeBase) * Time.deltaTime * (1/ (timeToFadeIn + timeToChangeSceneAfterFadeIn));
        }
    }


    public void ChangeToNextLevel()
    {
        if (!buttonsAllowed) return;
        ClikBehaviour();
        isDecreasingVolume = true;
        fade.FadeIn(timeToFadeIn);
        StartCoroutine(ChangeNextLevel(timeToFadeIn+timeToChangeSceneAfterFadeIn));
    }
    public void QuitGame()
    {
        if (!buttonsAllowed) return;
        ClikBehaviour();
        fade.FadeIn(timeToFadeIn);
        StartCoroutine(ExitGame(timeToFadeIn + timeToChangeSceneAfterFadeIn));
    }


    private void ClikBehaviour()
    {
        StartCoroutine(flashCanvasAlpha());

        SoundManager.Instance.CreateSound()
            .WithSoundData(clickSoundData)
            .WithRandomPitch()
            .Play();
    }
    private IEnumerator flashCanvasAlpha()
    {
        isFlashing = true;
        yield return new WaitForSeconds(flashTotalTime/2.0f);
        if (isGrowing)
        {
            isGrowing = false;
            StartCoroutine(flashCanvasAlpha());
        }
        else
        {
            isFlashing = false;
            isGrowing = false;
        }
    }
    private IEnumerator AllowButtons(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        buttonsAllowed = true;
    }
    private IEnumerator ExitGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }
    private IEnumerator ChangeNextLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(nextSceneName);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}