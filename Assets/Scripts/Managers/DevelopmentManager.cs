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

    DialogueManager2 dialogueManager;
    bool isCursorVisible = false;
    private void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager2>();
    }
    void Start()
    {
        GameManager.Instance.changeLoopTime = false;
        UpdateCursorState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIPrinciplal != null)
        {
            UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
            UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);

            UpdateCursorState();
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
            if (GameManager.Instance.LoopTime > 5f)
            {
                GameManager.Instance.changeLoopTime = true;
            }
            else
            {
                GameManager.Instance.changeLoopTime = false;
            }
        }    
    }
}
