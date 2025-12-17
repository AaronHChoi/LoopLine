using System;
using System.Collections;
using UnityEngine;

public class MonologueSpeaker : DialogueSpeakerBase, IMonologueSpeaker
{
    public event Action<Events> OnMonologueEnded;

    [SerializeField] private Events defaultEvent = Events.MonologueTest3;
    [SerializeField] private float startDelay = 1.5f;
    [SerializeField] private float autoAdvanceDelay = 3f;

    private Events currentMonologueEvent;

    private Coroutine autoAdvanceCoroutine;

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
            DialogueManager.Instance.ShowDialogue(dialogue, this);

            if (autoAdvance)
            {
                //Invoke(nameof(ShowNextDialogue), 3f);
                autoAdvanceCoroutine = StartCoroutine(WaitAndAdvance());
            }
        }
        else
        {
            EndDialogueSequence();
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