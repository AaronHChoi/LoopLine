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
    [SerializeField] bool hasCamera;
    public bool HasCamera
    {
        get { return  hasCamera; }
        set { hasCamera = value; }
    }

    public IScreenManager screenManager;

    protected override void Awake()
    {
        base.Awake();

        screenManager = InterfaceDependencyInjector.Instance.Resolve<IScreenManager>();

        SetGameConditions();
    }
    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIManager.Instance.ShowPanel(UIPanelID.Default);
        }
    }
    public void SetGameConditions()
    {
        var keys = new List<GameCondition>(conditions.Keys);
        foreach (var key in keys)
        {
            conditions[key] = false;
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