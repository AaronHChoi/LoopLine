using Core.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueSpeaker : DialogueSpeakerBase, IMonologueSpeaker
{
    public event Action<Events> OnMonologueEnded;

    [SerializeField] private Events defaultEvent = Events.MonologueTest3;
    [SerializeField] private float startDelay = 1.5f;
    [SerializeField] private float autoAdvanceDelay = 3f;
    [SerializeField] private float delayBetweenQueuedMonologues = 1.0f;

    private Events currentMonologueEvent;
    private Coroutine autoAdvanceCoroutine;
    private Coroutine queueProcessCoroutine;
    private Queue<Events> monologueQueue = new Queue<Events>();
    private bool isWaitingForNext = false;

    IDialogueUI dialogueUI;

    protected override void Awake()
    {
        dialogueUI = InterfaceDependencyInjector.Instance.Resolve<IDialogueUI>();
    }
    protected override void Start()
    {
        base.Start();
        currentMonologueEvent = defaultEvent;

        if (GameManager.Instance.HasCamera) //Patch
        {
            StartCoroutine(StartMonologueWithDelay());
            GameManager.Instance.HasCamera = false;
        }
    }
    public void StartMonologue(Events eventType)
    {
        if (isShowingDialogue || isWaitingForNext)
        {
            monologueQueue.Enqueue(eventType);
            return;
        }

        ExecuteMonologue(eventType);
    }
    private void ExecuteMonologue(Events eventType)
    {
        currentMonologueEvent = eventType;
        SetCurrentEvent(eventType);
        StartDialogueSequence();
    }
    public void StartMonologue()
    {
        StartMonologue(defaultEvent);
    }
    private IEnumerator StartMonologueWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartMonologue();
    }
    protected override void ShowCurrentDialogue()
    {
        StopAutoAdvance();

        if (currentDialogueIndex < currentDialogues.Count)
        {
            DialogueSO2 dialogue = currentDialogues[currentDialogueIndex];

            dialogueUI.OnTypingFinished += HandleTypingFinished;

            DialogueManager.Instance.ShowDialogue(dialogue, this);
        }
        else
        {
            EndDialogueSequence();
        }
    }
    private void HandleTypingFinished()
    {
        dialogueUI.OnTypingFinished -= HandleTypingFinished;

        if (autoAdvance)
        {
            //Invoke(nameof(ShowNextDialogue), 3f);
            autoAdvanceCoroutine = StartCoroutine(WaitAndAdvance());
        }
    }
    private IEnumerator WaitAndAdvance()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);

        ShowNextDialogue();
    }
    public override void ShowNextDialogue()
    {
        //CancelInvoke(nameof(ShowNextDialogue));
        StopAutoAdvance();
        base.ShowNextDialogue();
    }
    protected override void EndDialogueSequence()
    {
        //CancelInvoke(nameof(ShowNextDialogue));
        StopAutoAdvance();
        isShowingDialogue = false;
        currentDialogueIndex = 0;

        DialogueManager.Instance.HideDialogue();
        //base.EndDialogueSequence();

        OnMonologueEnded?.Invoke(currentMonologueEvent);

        if (monologueQueue.Count > 0)
        {
            if (queueProcessCoroutine != null)
            {
                StopCoroutine(queueProcessCoroutine);
            }
            queueProcessCoroutine = StartCoroutine(ProcessQueueRoutine());
        }
    }
    private IEnumerator ProcessQueueRoutine()
    {
        isWaitingForNext = true;

        yield return new WaitForSeconds(delayBetweenQueuedMonologues);

        isWaitingForNext = false;

        if(monologueQueue.Count > 0)
        {
            Events nextEvent = monologueQueue.Dequeue();
            ExecuteMonologue(nextEvent);
        }
    }
    void StopAutoAdvance()
    {
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }
    }
    public Events CurrentMonologueEvent => currentMonologueEvent;
}
public interface IMonologueSpeaker
{
    public void StartMonologue(Events eventType);
    event Action<Events> OnMonologueEnded;
}