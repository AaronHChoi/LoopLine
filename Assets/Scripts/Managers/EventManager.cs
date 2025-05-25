using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Subject, IDependencyInjectable
{
    [SerializeField] private AudioSource audioSource;
   
    [Header("Train Event 1")]
    [SerializeField] private AudioClip trainStopSound_1;
    [SerializeField] private AudioClip trainStopSound_2;

    [Header("Train Event 2")]
    [SerializeField] private AudioClip crystalBreakSound;

    [SerializeField] float delay = 1f;
    private bool isWindowBroken = false;
    bool trainEvent1 = true;

    [Header("Dialogues Managers")] //Referencias manuales
    [SerializeField] List<DialogueSOManager> dialogueManagers = new List<DialogueSOManager>();
    [SerializeField] DialogueSOManager workingMan;
    [SerializeField] DialogueSOManager player;
    [SerializeField] DialogueSOManager peek;

    DialogueUI dial;
    DialogueManager dialogueManager;
    TimeManager timeManager;
    UIManager uiManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        audioSource = GetComponent<AudioSource>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        dial = provider.DialogueUI;
        timeManager = provider.TimeManager;
        uiManager = provider.UIManager;
        dialogueManager = provider.DialogueManager;
    }
    private void Start()
    {
        if(GameManager.Instance.TrainLoop == 1)
        {
            player.TriggerEventDialogue("Train2");
        }
        if (GameManager.Instance.CorrectWord101)
        {
            workingMan.TriggerEventDialogue("CorrectWord");
        }
        StartCoroutine(StartSceneMonologue(delay));
    }
    void Update()
    {
        TrainEvent1();
        TrainEvent2();

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    NotifyObservers(Events.TriggerMonologue);
        //    dial.StopDialogue();
        //}
    }
    #region TrainEvents
    private void TrainEvent1()
    {
        if (timeManager.LoopTime <= 240 && timeManager.LoopTime >= 235)
        {
            NotifyObservers(Events.StopTrain);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = trainStopSound_1;
                audioSource.Play();
                //workingMan.TriggerEventDialogue("TrainStop");

                dialogueManager.StopAndFinishDialogue();

                foreach (DialogueSOManager dialogueManager in dialogueManagers)
                {
                    dialogueManager.TriggerEventDialogue("TrainStop");
                }
                peek.TriggerEventDialogue("TrainStop");
            }
        }
        else
        {
            //audioSource.Stop();
        }
        if (timeManager.LoopTime <= 180 && timeManager.LoopTime >= 175)
        {
            NotifyObservers(Events.ResumeTrain);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = trainStopSound_2;
                audioSource.Play();
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
            if (!audioSource.isPlaying)
            {
                audioSource.clip = crystalBreakSound;
                audioSource.Play();
                //workingMan.TriggerEventDialogue("BreakWindow");

                dialogueManager.StopAndFinishDialogue();

                foreach (DialogueSOManager dialogueManager in dialogueManagers)
                {
                    dialogueManager.TriggerEventDialogue("BreakWindow");
                }
                peek.TriggerEventDialogue("BreakWindow");
                player.TriggerEventDialogue("Train2");
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
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
        uiManager.ShowUIText("Aprete F para saltear");
    }
}
