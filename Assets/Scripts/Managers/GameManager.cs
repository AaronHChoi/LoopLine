using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;

    public string nextScene;

    public ScreenManager ScreenManager => screenManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TriggerPlayerDialogue();
        }
    }
    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void TriggerPlayerDialogue()
    {
        var playerSpeaker = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueSpeaker>();
        if (playerSpeaker != null)
        {
            playerSpeaker.DialogueTrigger();
        }
    }
}
