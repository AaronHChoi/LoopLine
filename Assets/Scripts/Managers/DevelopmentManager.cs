using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrinciplal;
    [SerializeField] private GameObject UIDeveloperMode;
    [SerializeField] private GameObject bgm;

    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] TimeManager timeManager;
    [SerializeField] DialogueManager dialManager;
    bool isCursorVisible = false;
    private void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        timeManager = FindFirstObjectByType<TimeManager>();
        dialManager = FindFirstObjectByType<DialogueManager>();
    }
    void Start()
    {
        timeManager.changeLoopTime = false;
        UpdateCursorState();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIPrinciplal != null && !dialManager.isDialogueActive)
        {
            UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
            UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);

            UpdateCursorState();
            GameManager.Instance.TogglePause();
        }
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = UIDeveloperMode.activeInHierarchy;

        if(isCursorVisible != shouldShowCursor)
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
        bgm.SetActive(!bgm.activeInHierarchy);
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