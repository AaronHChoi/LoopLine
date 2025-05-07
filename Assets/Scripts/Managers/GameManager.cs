using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] DialogueManager dialogueManager;

    public string nextScene;

    public int Loop;

    public ScreenManager ScreenManager => screenManager;
    public bool CorrectWord101 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    public void LoadNextScene(string sceneName)
    {
        if (sceneName == "MindPlace")
        {
            Loop++;
            dialogueManager.ResetAllDialogues();
        }
            

        SceneManager.LoadScene(sceneName);
    }
}
