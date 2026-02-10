using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using Core.Utilities;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MenuManager : MonoBehaviour, IMenuManager
{
    [Header("Fade")]
    [SerializeField] float timeToChangeSceneAfterCommand;
    [SerializeField] string nextSceneName;
    [SerializeField] float timeToEnableButtons;

    [Header("Sound")]
    [SerializeField] SoundData clickSoundData;
    [SerializeField] SoundData hoverSoundData;
    [SerializeField] AudioSource bgmAudio;


    [Header ("Flash")]
    [SerializeField] CanvasGroup flashCanvasGroup;
    [SerializeField] float flashTotalTime;

    [Header("Cursor")]
    [SerializeField] Texture2D cursorTexture;

    [Header("Buttons")]
    [SerializeField] Animator animator1;
    [SerializeField] Animator animator2;

    [Header("CinemaMachineSttings")]
    [SerializeField] Animator cinemachineAnimator;
    private bool isCamera2Active = false;

    [Header("Panel Settings")]
    [SerializeField] GameObject activePanel;
    [SerializeField] GameObject UIActivePanel;
    [SerializeField] MenuPanel panel_1;
    [SerializeField] MenuPanel panel_2;
    [SerializeField] MenuPanel panel_3;
    [SerializeField] TextMeshPro PanelTitleText;
    [SerializeField] Animator doorAnimator;


    private bool buttonsAllowed = false;
    private bool isFlashing = false;
    private bool isGrowing = true;
    private bool isDecreasingVolume = false;
    private float bgmVolumeBase;

    ISoundManager soundManager;
    IFadeInOutController fade;
    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();   
        fade = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.MenuFade);
    }
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        if (fade != null)
        {
            StartCoroutine(AllowButtons(true, timeToEnableButtons));
            fade.ForceFade(false);
        }
        activePanel = panel_1.gameObject;
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

        if (Input.anyKeyDown && !isCamera2Active)
        {
            ChangeCamera();
            isCamera2Active = true;
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

    public void ChangeCamera()
    {
        cinemachineAnimator.SetBool("IsCamera1", !cinemachineAnimator.GetBool("IsCamera1"));
        ClikBehaviour();
        DelayUtility.Instance.Delay(2f, () => 
        {
            doorAnimator.SetTrigger("DoorOpened");
            panel_1.UIPanel.SetActive(true);
            UIActivePanel = panel_1.UIPanel;
            doorAnimator.SetTrigger("DoorIdleOpen");
        });
        
    }

    public void ChangeActivePanel (MenuPanel Panel)
    {

        doorAnimator.SetTrigger("DoorClosed");
        doorAnimator.SetTrigger("DoorIdleClosed");
        DelayUtility.Instance.Delay(2f, () =>
        {
            activePanel.SetActive(false);            
            UIActivePanel.SetActive(false);
        });

        StartCoroutine(ChangePanelAfterClose(Panel));
    }
    private IEnumerator ChangePanelAfterClose(MenuPanel panel)
    {
        yield return new WaitForSeconds(3); 

        activePanel = panel.gameObject;
        UIActivePanel = panel.UIPanel;

        activePanel.SetActive(true);
        UIActivePanel.SetActive(true);

        PanelTitleText.text = panel.panelTitle;

        doorAnimator.SetTrigger("DoorOpened");
        doorAnimator.SetTrigger("DoorIdleOpen");
    }

    private void ClikBehaviour()
    {
        StartCoroutine(flashCanvasAlpha());

        soundManager.CreateSound()
            .WithSoundData(clickSoundData)
            .WithRandomPitch()
            .Play();
        buttonsAllowed = false;
    }

    public void HoverBehaviour()
    {
        if (!buttonsAllowed) return;
        soundManager.CreateSound()
            .WithSoundData(hoverSoundData)
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

public interface IMenuManager
{
    void HoverBehaviour();
}