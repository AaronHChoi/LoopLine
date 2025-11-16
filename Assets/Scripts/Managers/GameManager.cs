using System;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManager")]
    public string nextScene;
    public bool isGamePaused;

    public int TrainLoop = 0;

    Dictionary<GameCondition, bool> conditions = new Dictionary<GameCondition, bool>();

    [Header("DeveloperTools")]
    public bool isMuted = false;

    [Header("ClockQuest")]
    bool clockQuestCompleted;
    public bool ClockQuest
    {
        get { return clockQuestCompleted; }
        set { clockQuestCompleted = value; }
    }

    [Header("Player")]
    [SerializeField] bool hasCamera = true;
    public bool HasCamera
    {
        get { return  hasCamera; }
        set { hasCamera = value; }
    }

    public IScreenManager screenManager;
    IGameSceneManager gameSceneManager;
    protected override void Awake()
    {
        base.Awake();

        screenManager = InterfaceDependencyInjector.Instance.Resolve<IScreenManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();

        SetGameConditions();
    }
    private void Update()
    {
        //TESTING
        if (Input.GetKeyDown(KeyCode.H))
        {
            gameSceneManager.SetInitialLoop(true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            gameSceneManager.SetInitialLoop(false);
        }
    }
    public void SetGameConditions()
    {
        conditions.Clear();

        foreach (GameCondition enumValue in Enum.GetValues(typeof(GameCondition)))
        {
            conditions[enumValue] = false;
        }
    }
    public bool GetCondition(GameCondition condition)
    {
        return conditions.ContainsKey(condition) && conditions[condition];
    }
    public void SetCondition(GameCondition condition, bool value)
    {
        conditions[condition] = value;
    }
}