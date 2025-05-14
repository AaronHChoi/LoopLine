using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrinciplal;
    [SerializeField] private GameObject UIDeveloperMode;

    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] TimeManager timeManager;
    [SerializeField] DialogueManager dialManager;
    [SerializeField] PlayerController playerController;

    private Dictionary<AudioSource, float> audiosVolumeDic;
    bool isCursorVisible = false;
    bool isUIActive = false;
    bool isMuted = false;
    private void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        timeManager = FindFirstObjectByType<TimeManager>();
        dialManager = FindFirstObjectByType<DialogueManager>();
        playerController = FindFirstObjectByType<PlayerController>();
    }
    void Start()
    {
        timeManager.changeLoopTime = false;
        UpdateCursorState();
        InitAudios();
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
        }
    }
    private void ToggleUI()
    {
        isUIActive = !isUIActive;

        UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
        UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);

        playerController.SetControllerEnabled(!isUIActive);

        UpdateCursorState();
    }
    public void DeactivateUIIfActive()
    {
        if (UIDeveloperMode.activeInHierarchy)
        {
            UIPrinciplal.SetActive(true);
            UIDeveloperMode.SetActive(false);

            isUIActive = false;

            playerController.SetControllerEnabled(true);

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
    public void Mute()
    {
        isMuted = !isMuted;
        foreach (var audio in audiosVolumeDic)
        {
            if (isMuted)
            {
                audio.Key.volume = 0;
            }
            else
            {
                audio.Key.volume = audio.Value;
            }
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
                timeManager.changeLoopTime = true;
            }
            else
            {
                timeManager.changeLoopTime = false;
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