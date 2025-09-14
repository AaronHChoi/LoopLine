using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using DependencyInjection;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrinciplal;
    [SerializeField] private GameObject UIDeveloperMode;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private FadeInOutController cinemaFade;
    private Dictionary<AudioSource, float> audiosVolumeDic;
    bool isCursorVisible = false;
    bool isUIActive = false;
    private bool isCinemaOn = false;

    IItemManager itemManager;
    IPlayerController playerController;
    IPlayerStateController playerStateController;
    IDialogueManager dialogueManager;
    ITimeProvider timeManager;
    private void Awake()
    {
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        itemManager = InterfaceDependencyInjector.Instance.Resolve<IItemManager>();
    }
    void Start()
    {
        timeManager.ChangeLoopTime = false;
        UpdateCursorState();
        InitAudios();
        Mute(false);
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnOpenDevelopment += OpenDevelopMode;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnOpenDevelopment -= OpenDevelopMode;
        }
    }
    public void OpenDevelopMode()
    {
        if (UIPrinciplal != null && !dialogueManager.IsDialogueActive)
        {
            ToggleUI();
        }
    }
    private void ToggleUI()
    {
        isUIActive = !isUIActive;

        UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
        UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);

        if (isUIActive)
        {
            playerStateController.ChangeState(playerStateController.DialogueState);
        }
        else
        {
            playerStateController.ChangeState(playerStateController.NormalState);
        }

        UpdateCursorState();
    }
    public void DeactivateUIIfActive()
    {
        if (UIDeveloperMode.activeInHierarchy)
        {
            UIPrinciplal.SetActive(true);
            UIDeveloperMode.SetActive(false);

            isUIActive = false;

            playerStateController.ChangeState(playerStateController.NormalState);

            timeManager.PauseTime(false);
            UpdateCursorState();
        }
    }
    private void InitAudios()
    {
        audiosVolumeDic = new Dictionary<AudioSource, float>();

        var auxAudios = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var audio in auxAudios)
        {
            audiosVolumeDic.Add(audio, audio.volume);
        }
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = UIDeveloperMode.activeInHierarchy;

        if (isCursorVisible != shouldShowCursor)
        {
            isCursorVisible = shouldShowCursor;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    public void ResetDialogues()
    {
        dialogueManager.ResetAllDialogues();
        dialogueManager.UnlockFirstDialogues();
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuLevel()
    {
        SceneManager.LoadScene("01. MainMenu");
    }
    public void Mute(bool changeMute)
    {
        if(changeMute) GameManager.Instance.isMuted = !GameManager.Instance.isMuted;

        foreach (var audio in audiosVolumeDic)
        {
            if (GameManager.Instance.isMuted)
            {
                audio.Key.volume = 0;
            }
            else
            {
                audio.Key.volume = audio.Value;
            }
        }
        if (GameManager.Instance.isMuted)
        {
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioMixer.SetFloat("Master", 0f);
        }
    }
    public void LoadMainLevel()
    {
        if ("05. MindPlace, 99. Showcase".Contains(SceneManager.GetActiveScene().name))
        {
            SceneManager.LoadScene("04. Train");
        }
    }
    public void LoadShowcase()
    {
        SceneManager.LoadScene("99. Showcase");
    }
    public void CutTime()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (timeManager.LoopTime > 5f)
            {
                timeManager.ChangeLoopTime = true;
            }
            else
            {
                timeManager.ChangeLoopTime = false;
            }
        }
    }
    public void ForceCinematic()
    {
        cinemaFade.ForceFade(!isCinemaOn);
        isCinemaOn =! isCinemaOn;
    }
    //public void CutTimeStopTrain()
    //{
    //    timeManager.SetLoopTimeToStopTrain();
    //}
    public void CutTimeBreakCrystal()
    {
        timeManager.SetLoopTimeToBreakCrystal();
    }
    public void AvilitateItemsCollection()
    {
        for (int i = 0; i < itemManager.items.Count; i++)
        {
            itemManager.items[i].canBePicked = (!itemManager.items[i].canBePicked);
        }
    }
}