using System.Collections;
using UnityEngine;
using DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MonologuePanelMapping
{
    public Events monologueEvent;
    public UIPanelID panelID;
}

public class EventManager : Subject, IEventManager
{
    [Header("Sound System Event 1")]
    [SerializeField] private SoundData trainStopSoundData;
    [SerializeField] private SoundData trainStopSoundData2;
    [Header("Sound System Event 2")]
    [SerializeField] private SoundData crystalBreakSoundData;
    [SerializeField] private Transform crystalBreakTransform;
    [SerializeField] float delayBrokenWindow;

    bool brokenWindow = false;
    public bool stopTrain { get; private set; } = false;
    private bool stopTrain2 = false;
    [SerializeField] public float StopedTimeForTrain { get; set; } = 30f;
    [SerializeField] private float AddTime = 30f;
    [SerializeField] StopButtonInteract stopButtonInteract;
    private Coroutine coroutineDelay;

    [SerializeField] List<MonologuePanelMapping> monologuePanelMap = new List<MonologuePanelMapping>();

    ITimeProvider timeManager;
    IUIManager uiManager;
    IDialogueManager dialogueManager;
    private void Awake()
    {
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
    }
    private void Start()
    {
        stopButtonInteract = FindAnyObjectByType<StopButtonInteract>();

        MonologueSpeaker[] allSpeakers = FindObjectsByType<MonologueSpeaker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach(MonologueSpeaker speaker in allSpeakers)
        {
            speaker.OnMonologueEnded += HandleMonologueEnded;
        }
    }
    void Update()
    {
        TrainEventResumeTrain();
        TrainEvent2();
    }
    private void OnDisable()
    {
        MonologueSpeaker[] allSpeakers = FindObjectsByType<MonologueSpeaker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (MonologueSpeaker speaker in allSpeakers)
        {
            speaker.OnMonologueEnded -= HandleMonologueEnded;
        }
    }
    private void HandleMonologueEnded(Events monologueEvent)
    {
        if (GameManager.Instance.TrainLoop > 0) return;

        MonologuePanelMapping mapping = monologuePanelMap.FirstOrDefault(m => m.monologueEvent == monologueEvent);

        if (mapping != null)
        {
            uiManager.ShowPanel(mapping.panelID);
        }
    }
    #region TrainEvents
    public void TrainEventStopTrain()
    {
        if (!stopTrain)
        {
            NotifyObservers(Events.StopTrain);

            if (!stopTrain)
            {
                timeManager.AddTime(AddTime);
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData)
                    .Play();

                stopTrain = true;

            }
        }
    }
    public void TrainEventResumeTrain()
    {
        if (stopTrain && StopedTimeForTrain <= 0f)
        {
            NotifyObservers(Events.ResumeTrain);
            
            if (!stopTrain2)
            {
                stopButtonInteract.TriggerRock.gameObject.SetActive(false);
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData2)
                    .Play();

                stopTrain2 = true;
            }
        }
    }
    private void TrainEvent2()
    {
        if (timeManager == null) return;

        if (!brokenWindow && timeManager.LoopTime <= 60 && timeManager.LoopTime >= 55)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(crystalBreakSoundData)
                .WithSoundPosition(crystalBreakTransform.position)
                .PlayWithDelay(crystalBreakSoundData);

            stopTrain = false;
            brokenWindow = true;
            stopButtonInteract.Rock.gameObject.SetActive(true);
            stopButtonInteract.TriggerRock.gameObject.SetActive(true);
            if (coroutineDelay != null)
            {
                StopCoroutine(coroutineDelay);
            }
            coroutineDelay = StartCoroutine(DelayTimer(delayBrokenWindow));
        }
    }
    private IEnumerator DelayTimer(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        NotifyObservers(Events.BreakCrystal);
    }
    #endregion

    public void AddNewObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveOldObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
}

public interface IEventManager
{
    bool stopTrain {  get; }
    float StopedTimeForTrain { get; set; }
    void TrainEventStopTrain();
    void AddNewObserver(IObserver observer);
    void RemoveOldObserver(IObserver observer);
}