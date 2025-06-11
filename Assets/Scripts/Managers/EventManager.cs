using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Subject, IDependencyInjectable, IEventManager
{
    [SerializeField] private AudioSource audioSource2D;
    [SerializeField] private AudioSource audioSource3D;

    [Header("Train Event 1")]
    [SerializeField] private AudioClip trainStopSound_1;
    [SerializeField] private AudioClip trainStopSound_2;

    [Header("Train Event 2")]
    [SerializeField] private AudioClip crystalBreakSound;

    [SerializeField] float delay = 1f;
    private bool isWindowBroken = false;
    bool trainEvent1 = true;

    [SerializeField] bool start = true;
    [SerializeField] bool stopTrain = false;
    [SerializeField] bool brokenWindow = false;

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
        StartCoroutine(StartSceneMonologue(delay));
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
            if (!audioSource2D.isPlaying)
            {
                audioSource2D.clip = trainStopSound_1;
                audioSource2D.Play();

                dialogueManager.StopAndFinishDialogue();

                start = false;
                stopTrain = true;

                EventStopTrain();
            }
        }
        else
        {
            //audioSource.Stop();
        }
        if (timeManager.LoopTime <= 180 && timeManager.LoopTime >= 175)
        {
            NotifyObservers(Events.ResumeTrain);
            if (!audioSource2D.isPlaying)
            {
                audioSource2D.clip = trainStopSound_2;
                audioSource2D.Play();
            }
        }
        else
        {
            //audioSource.Stop();
        }
    }
    private void TrainEvent2()
    {
        if (!isWindowBroken && timeManager.LoopTime <= 60 && timeManager.LoopTime >= 55)
        {
            if (!audioSource3D.isPlaying)
            {
                audioSource3D.clip = crystalBreakSound;
                audioSource3D.Play();

                dialogueManager.StopAndFinishDialogue();

                stopTrain = false;
                brokenWindow = true;

                EventBrokenWindow();
            }
            NotifyObservers(Events.BreakCrystal);
            isWindowBroken = true;
        }
        else
        {
            //audioSource.Stop();
        }
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
    public void AfterFirstInteraction(string _name)
    {
        foreach (DialogueSOManager dialogueManager in dialogueManagers)
        {
            if(dialogueManager.NPCname == _name)
            {
                if (start && !stopTrain && !brokenWindow)
                {
                    dialogueManager.TriggerEventDialogue("E1-Start");
                } else if (!start && stopTrain && !brokenWindow)
                {
                    dialogueManager.TriggerEventDialogue("E1-StopTrain");
                } else
                {
                    dialogueManager.TriggerEventDialogue("E1-BrokenWindow");
                }
            }
        }
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
        //uiManager.ShowUIText("Aprete F para saltear");
    }
}
public interface IEventManager
{
    void AfterFirstInteraction(string _name);
}