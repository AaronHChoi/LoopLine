using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrinciplal;
    [SerializeField] private GameObject UIDeveloperMode;

    [SerializeField] TimeManager timeManager;
    [SerializeField] DialogueManager dialManager;

    [SerializeField] private AudioMixer audioMixer;
    private Dictionary<AudioSource, float> audiosVolumeDic;
    bool isCursorVisible = false;
    bool isUIActive = false;

    IPlayerController playerController;
    IDialogueManager dialogueManager;
    private void Awake()
    {
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        timeManager = FindFirstObjectByType<TimeManager>();
        dialManager = FindFirstObjectByType<DialogueManager>();
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
    }
    void Start()
    {
        timeManager.ChangeLoopTime = false;
        UpdateCursorState();
        InitAudios();
        Mute(false);
    }
    void Update()
    {
        OpenDevelopMode();
    }
    public void OpenDevelopMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIPrinciplal != null && !dialManager.isDialogueActive)
        {
            ToggleUI();
            timeManager.PauseTime();
        }
    }
    private void ToggleUI()
    {
        isUIActive = !isUIActive;

        UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
        UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);

        playerController.SetCinemachineController(!isUIActive);

        UpdateCursorState();
    }
    public void DeactivateUIIfActive()
    {
        if (UIDeveloperMode.activeInHierarchy)
        {
            UIPrinciplal.SetActive(true);
            UIDeveloperMode.SetActive(false);

            isUIActive = false;

            playerController.SetCinemachineController(true);

            timeManager.PauseTime();
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
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MenuLevel()
    {
        SceneManager.LoadScene("MainMenu");
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
        if (SceneManager.GetActiveScene().name == "MindPlace")
        {
            SceneManager.LoadScene("Train");
        }
    }
    public void CutTime()
    {
        if (SceneManager.GetActiveScene().name == "Train")
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
    public void CutTimeStopTrain()
    {
        timeManager.SetLoopTimeToStopTrain();
    }

    public void CutTimeBreakCrystal()
    {
        timeManager.SetLoopTimeToBreakCrystal();
    }
}