using DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManager")]
    public string nextScene;
    public bool isGamePaused;

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
    public IScreenManager screenManager;

    protected override void Awake()
    {
        base.Awake();

        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        screenManager = InterfaceDependencyInjector.Instance.Resolve<IScreenManager>();
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F)) //TEST
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