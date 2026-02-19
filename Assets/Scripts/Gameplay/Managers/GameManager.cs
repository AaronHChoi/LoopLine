using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Core.Utilities;
using Core.DependencyInjection;
using Core.Data;
using Core.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("GameManager")]
    public string nextScene;
    public bool isGamePaused;
    public bool isCinematicMonologue;

    [SerializeField] int trainLoop = 0;
    public int TrainLoop
    {
        get { return trainLoop; }
        set { trainLoop = value; }
    }

    Dictionary<GameCondition, bool> conditions = new Dictionary<GameCondition, bool>();

    public event Action<GameCondition, bool> OnConditionChanged;

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
    public int currentPhotoIndex;

    public IScreenManager screenManager;
    IMonologueSpeaker monologueSpeaker;

    protected override void Awake()
    {
        base.Awake();

        screenManager = InterfaceDependencyInjector.Instance.Resolve<IScreenManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);

        SetGameConditions();
    }
    private void Start()
    { 
        // ACTIVATE ON BUILD
        DelayUtility.Instance.Delay(80f, () => SetCondition(GameCondition.Chapter0, true));
        DelayUtility.Instance.Delay(80f, () => monologueSpeaker.StartMonologue(Events.InitialMonologue));
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
#if UNITY_EDITOR
    private void Update()
    {
        //TESTING
        if (Input.GetKeyDown(KeyCode.H))
        {
            //RectTransform rect = UICassette.GetComponent<RectTransform>();

            //rect.anchoredPosition = new Vector2(0, -45);
            //carretteControllerLeft.SetRotation(true);
            //carretteControllerRight.SetRotation(true);
        }
    }
#endif
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
        if (conditions.ContainsKey(condition) && conditions[condition] == value)
        {
            return;
        }

        conditions[condition] = value;

        OnConditionChanged?.Invoke(condition, value);
    }
    #region DEBUG_TOOLS
#if UNITY_EDITOR
    [ContextMenu("Debug All Conditions")]
    public void DebugAllConditions()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<size=14><b>--- GAME CONDITIONS STATES---</b></size>");

        foreach (var kvp in conditions)
        {
            string color = kvp.Value ? "green" : "red";
            string statusText = kvp.Value ? "ACTIVE" : "INACTIVE";

            sb.AppendLine($"<b>{kvp.Key}:</b> <color={color}>{statusText}</color>");
        }

        Debug.Log(sb.ToString());
    }
    [ContextMenu("Debug TP")]
    private void DebugTP()
    {
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
    }
#endif
#endregion
}