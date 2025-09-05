using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DependencyInjection;
public class EventManager : Subject
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
    [SerializeField] public float StopedTimeForTrain = 30f;
    [SerializeField] private float AddTime = 30f;
    [SerializeField] StopButtonInteract stopButtonInteract;
    private Coroutine coroutineDelay;

    [SerializeField] private QuestionSO stopTrainQuestion;

    [Header("Dialogues Managers")] //Referencias manuales
    [SerializeField] List<DialogueSOManager> dialogueManagers = new List<DialogueSOManager>();

    [SerializeField] DialogueSOManager player;

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
                timeManager.AddTime(AddTime);
                SoundManager.Instance.CreateSound()
                    .WithSoundData(trainStopSoundData)
                    .Play();

                dialogueManager.StopAndFinishDialogue();

                stopTrain = true;

                stopTrainQuestion.Options[3].Choosen = false;
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

            dialogueManager.StopAndFinishDialogue();

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
}