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

    bool brokenWindow = false;
    public bool stopTrain { get; private set; } = false;
    private bool stopTrain2 = false;
    [SerializeField] public float StopedTimeForTrain = 30f;
    [SerializeField] private float AddTime = 30f;
    [SerializeField] StopButtonInteract stopButtonInteract;
    private Coroutine coroutineDelay;

    [SerializeField] private QuestionSO stopTrainQuestion;

    [Header("Dialogues Managers")] //Referencias manuales
    [SerializeField] List<DialogueSOManager> dialogueManagers = new List<DialogueSOManager>();

    [SerializeField] DialogueSOManager player;

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
        if (GameManager.Instance.TrainLoop == 1)
        {
            player.TriggerEventDialogue("Train2");

        }
        StartCoroutine(StartSceneMonologue(delayMonologue));
        stopButtonInteract = FindAnyObjectByType<StopButtonInteract>();
    }
    public void InitializeDialogues()
    {
        stopTrainQuestion.Options[0].Choosen = false;
        stopTrainQuestion.Options[1].Choosen = false;
        stopTrainQuestion.Options[2].Choosen = false;
        stopTrainQuestion.Options[3].Choosen = true;
        stopTrainQuestion.Options[4].Choosen = true;
        stopTrainQuestion.Options[5].Choosen = true;
    }
    public void KeepClues()
    {
        stopTrainQuestion.Options[0].AddToWhiteboard = false;
        stopTrainQuestion.Options[1].AddToWhiteboard = false;
        stopTrainQuestion.Options[2].AddToWhiteboard = false;
        stopTrainQuestion.Options[3].AddToWhiteboard = false;
        stopTrainQuestion.Options[4].AddToWhiteboard = false;
        stopTrainQuestion.Options[5].AddToWhiteboard = false;
    }
    void Update()
    {
        TrainEventResumeTrain();
        TrainEvent2();
    }
    #region TrainEvents
    public void TrainEventStopTrain()
    {
        if (!stopTrain)
        {
            NotifyObservers(Events.StopTrain);

            if (!stopTrain)
            {
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData)
                    .Play();

                dialogueManager.StopAndFinishDialogue();

                stopTrain = true;

                stopTrainQuestion.Options[3].Choosen = false;

                EventStopTrain();
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
                timeManager.AddTime(AddTime);
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
            stopButtonInteract.Rock.gameObject.SetActive(true);
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
        stopTrainQuestion.Options[4].Choosen = false;
        EventBrokenWindow();
        NotifyObservers(Events.BreakCrystal);
    }
    #endregion

    private void EventStopTrain()
    {
        foreach (DialogueSOManager dialogueManager in dialogueManagers)
        {
            dialogueManager.TriggerEventDialogue("StopTrain");
        }
    }
    private void EventBrokenWindow()
    {
        foreach (DialogueSOManager dialogueManager in dialogueManagers)
        {
            dialogueManager.TriggerEventDialogue("BreakWindow");
        }
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
        //uiManager.ShowUIText("Aprete F para saltear");
    }
}