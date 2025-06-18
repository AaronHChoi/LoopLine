using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Subject, IDependencyInjectable
{
    [Header("Sound System Event 1")]
    [SerializeField] private SoundData trainStopSoundData;
    [SerializeField] private SoundData trainStopSoundData2;
    [Header("Sound System Event 2")]
    [SerializeField] private SoundData crystalBreakSoundData;
    [SerializeField] private Transform crystalBreakTransform;
    [SerializeField] float delayBrokenWindow;

    [SerializeField] float delayMonologue = 0.1f;
    private bool isWindowBroken = false;

    [SerializeField] bool start = true;
    bool brokenWindow = false;
    private bool stopTrain = false;
    private bool stopTrain2 = false;
    private Coroutine coroutineDelay;

    [Header("Dialogues Managers")] //Referencias manuales
    [SerializeField] List<DialogueSOManager> dialogueManagers = new List<DialogueSOManager>();
    [SerializeField] DialogueSOManager workingMan;
    [SerializeField] DialogueSOManager player;
    [SerializeField] DialogueSOManager peek;

    TimeManager timeManager;

    IUIManager uiManager;
    IDialogueManager dialogueManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        timeManager = provider.TimeManager;
    }
    private void Start()
    {
        if(GameManager.Instance.TrainLoop == 1)
        {
            player.TriggerEventDialogue("Train2");
        }
        StartCoroutine(StartSceneMonologue(delayMonologue));
    }
    void Update()
    {
        TrainEvent1();
        TrainEvent2();
    }
    #region TrainEvents
    private void TrainEvent1()
    {
        if (timeManager.LoopTime <= 240 && timeManager.LoopTime >= 235)
        {
            NotifyObservers(Events.StopTrain);

            if (!stopTrain)
            {
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData)
                    .Play();

                dialogueManager.StopAndFinishDialogue();

                start = false;
                stopTrain = true;

                EventStopTrain();
            }
        }
        if (timeManager.LoopTime <= 180 && timeManager.LoopTime >= 175)
        {
            NotifyObservers(Events.ResumeTrain);
            if (!stopTrain2)
            {
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData2)
                    .Play();

                stopTrain2 = true;
            }
        }
    }
    private void TrainEvent2()
    {
        if (!brokenWindow && timeManager.LoopTime <= 60 && timeManager.LoopTime >= 55)
        {

            SoundManager.Instance.CreateSound()
                .WithSoundData(crystalBreakSoundData)
                .WithSoundPosition(crystalBreakTransform.position)
                .PlayWithDelay(crystalBreakSoundData);

            dialogueManager.StopAndFinishDialogue();

            stopTrain = false;
            brokenWindow = true;
            
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
        EventBrokenWindow();
        NotifyObservers(Events.BreakCrystal);
    }
    #endregion

    private void EventStopTrain()
    {
        foreach (DialogueSOManager dialogueManager in dialogueManagers)
        {
            if(dialogueManager.firstInteractionAfterCheck)
                dialogueManager.TriggerEventDialogue("E1-StopTrain");
        }
    }
    private void EventBrokenWindow()
    {
        foreach (DialogueSOManager dialogueManager in dialogueManagers)
        {
            if (dialogueManager.firstInteractionAfterCheck)
                dialogueManager.TriggerEventDialogue("E1-BrokenWindow");
        }
    }
    //public void AfterFirstInteraction(string _name)
    //{
    //    foreach (DialogueSOManager dialogueManager in dialogueManagers)
    //    {
    //        if(dialogueManager.NPCname == _name)
    //        {
    //            if (start && !stopTrain && !brokenWindow)
    //            {
    //                dialogueManager.TriggerEventDialogue("E1-Start");
    //            } else if (!start && stopTrain && !brokenWindow)
    //            {
    //                dialogueManager.TriggerEventDialogue("E1-StopTrain");
    //            } else
    //            {
    //                dialogueManager.TriggerEventDialogue("E1-BrokenWindow");
    //            }
    //        }
    //    }
    //}
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
        //uiManager.ShowUIText("Aprete F para saltear");
    }
}