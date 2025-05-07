using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private ScreenManager screenManager;

    public string nextScene;

    public int Loop;

    public ScreenManager ScreenManager => screenManager;

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
    }
    public void LoadNextScene(string sceneName)
    {
        if (sceneName == "MindPlace")
            Loop++;

        SceneManager.LoadScene(sceneName);
    }
}
