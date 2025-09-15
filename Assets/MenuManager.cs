using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] FadeInOutController fade;
    [SerializeField] float timeToChangeSceneAfterCommand;
    [SerializeField] string nextSceneName;
    [SerializeField] float timeToEnableButtons;

    [Header("Sound")]
    [SerializeField] SoundData clickSoundData;
    [SerializeField] AudioSource bgmAudio;

    [Header ("Flash")]
    [SerializeField] CanvasGroup flashCanvasGroup;
    [SerializeField] float flashTotalTime;

    [Header("Cursor")]
    [SerializeField] Texture2D cursorTexture;

    [Header("Buttons")]
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;

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
            StartCoroutine(AllowButtons(true, timeToEnableButtons));
            fade.ForceFade(false);
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
            bgmAudio.volume -= (bgmVolumeBase) * Time.deltaTime * (1/ (timeToChangeSceneAfterCommand));
        }
    }


    public void ChangeToNextLevel()
    {
        if (!buttonsAllowed) return;
        AllowButtons(false);
        ClikBehaviour();
        isDecreasingVolume = true;
        fade.ForceFade(true);
        StartCoroutine(ChangeNextLevel(timeToChangeSceneAfterCommand));
    }
    public void QuitGame()
    {
        if (!buttonsAllowed) return;
        AllowButtons(false);
        ClikBehaviour();
        fade.ForceFade(true);
        StartCoroutine(ExitGame(timeToChangeSceneAfterCommand));
    }


    private void ClikBehaviour()
    {
        StartCoroutine(flashCanvasAlpha());

        SoundManager.Instance.CreateSound()
            .WithSoundData(clickSoundData)
            .WithRandomPitch()
            .Play();
        buttonsAllowed = false;
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
    private IEnumerator AllowButtons(bool isAllowed, float seconds = 0f)
    {
        yield return new WaitForSeconds(seconds);
        buttonsAllowed = true;
        animator1.SetBool("IsAllowed", isAllowed);
        animator2.SetBool("IsAllowed", isAllowed);
    }
    private IEnumerator ExitGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }
    private IEnumerator ChangeNextLevel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetDefaultCursor();
        SceneManager.LoadScene(nextSceneName);
    }
    private void SetDefaultCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}