using DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string nextScene;

    public int TrainLoop = 0;
    public int MindPlaceLoop = 0;

    [Header("DeveloperTools")]
    public bool isMuted = false;

    bool clockQuest;
    public bool ClockQuest
    {
        get { return clockQuest; }
        set { clockQuest = value; }
    }

    IGameSceneManager gameSceneManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            gameSceneManager.UnloadLastScene();
            if (gameSceneManager.IsCurrentScene("04. Train"))
            {
                SceneManager.LoadScene("05. MindPlace");
            }
            else if (gameSceneManager.IsCurrentScene("05. MindPlace"))
            {
                SceneManager.LoadScene("04. Train");
            }
        }
    }
}