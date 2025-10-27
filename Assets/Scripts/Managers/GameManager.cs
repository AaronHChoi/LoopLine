using DependencyInjection;
using UnityEngine;

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
}