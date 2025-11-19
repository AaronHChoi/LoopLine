using System;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManager")]
    public string nextScene;
    public bool isGamePaused;

    [SerializeField] int trainLoop = 0;

    public int TrainLoop
    {
        get { return trainLoop; }
        set { trainLoop = value; }
    }

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

    public int currentPhotoIndex = 0;

    public IScreenManager screenManager;
    protected override void Awake()
    {
        base.Awake();

        screenManager = InterfaceDependencyInjector.Instance.Resolve<IScreenManager>();

        SetGameConditions();
    }
    private void OnValidate()
    {
        if (trainLoop > 2)
        {
            if (Application.isPlaying)
            {
                TrainLoop = trainLoop;
            }
        }
    }
    private void Update()
    {
        //TESTING
        if (Input.GetKeyDown(KeyCode.H))
        {

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