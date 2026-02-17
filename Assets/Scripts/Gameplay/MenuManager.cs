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
    [SerializeField] MenuPanel activePanel;
    [SerializeField] GameObject UIActivePanel;
    [SerializeField] MenuPanel panel_1;
    [SerializeField] MenuPanel panel_2;
    [SerializeField] MenuPanel panel_3;
    [SerializeField] TextMeshPro PanelTitleText;
    [SerializeField] Animator doorAnimator;
    [SerializeField] DoorInteract door;
    private bool isTransitioning;
    private Coroutine transitionCoroutine;

    private bool buttonsAllowed = false;
    private bool isFlashing = false;
    private bool isGrowing = true;
    private bool isDecreasingVolume = false;
    private bool isDoorOpening= false;
    private float bgmVolumeBase;

    ISoundManager soundManager;
    IFadeInOutController fade;
    IFadeInOutController fadePanel1;
    IFadeInOutController fadePanel2;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();   
        fade = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.MenuFade);
        fadePanel1 = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(panel_1.fadeID);
        fadePanel2 = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(panel_2.fadeID);
    }
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        if (fade != null)
        {
           
            fade.ForceFade(false);
        }
        activePanel = panel_1;
        //panel_2.Fade(false);
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
            StartCoroutine(ChangeCamera());
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

    public IEnumerator ChangeCamera()
    {
        cinemachineAnimator.SetBool("IsCamera1", !cinemachineAnimator.GetBool("IsCamera1"));
        ClikBehaviour();
        yield return new WaitForSeconds(2f);
        door.OpenDoors();

        yield return new WaitUntil(() => door.isOpen);
        panel_1.UIPanel.SetActive(true);
        //panel_1.Fade(true);
        UIActivePanel = panel_1.UIPanel;

        StartCoroutine(AllowButtons(true));

        
    }

    public void ChangeActivePanel (MenuPanel panel)
    {
        if (isTransitioning) return;
     

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionRoutine(panel));
    }
    private IEnumerator TransitionRoutine(MenuPanel panel)
    {
        isTransitioning = true;
        buttonsAllowed = false;

        UIActivePanel.SetActive(false);

        door.CloseDoors();

        yield return new WaitUntil(() => !door.isOpen);

        activePanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f); // total 3 segundos

        activePanel = panel;
        UIActivePanel = panel.UIPanel;

        PanelTitleText.text = panel.panelTitle;

        activePanel.gameObject.SetActive(true);
        UIActivePanel.SetActive(true);

        door.OpenDoors();
        Debug.Log("Door opened");
        yield return new WaitUntil(() => door.isOpen);

        buttonsAllowed = true;
        isTransitioning = false;
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
        //if (!buttonsAllowed) return;
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
        yield return new WaitUntil(() => door.isOpen);
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