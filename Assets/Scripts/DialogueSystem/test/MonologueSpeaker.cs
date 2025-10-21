using System.Collections;
using UnityEngine;

public class MonologueSpeaker : DialogueSpeakerBase, IMonologueSpeaker
{
    [SerializeField] private Events defaultEvent = Events.MonologueTest3;
    [SerializeField] private float startDelay = 1.5f;

    private Events currentMonologueEvent;
    protected override void Start()
    {
        base.Start();
        currentMonologueEvent = defaultEvent;

        StartCoroutine(StartMonologueWithDelay());
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
        if (currentDialogueIndex < currentDialogues.Count)
        {
            DialogueSO2 dialogue = currentDialogues[currentDialogueIndex];
            DialogueManager.Instance.ShowDialogue(dialogue, this);

            if (autoAdvance)
            {
                Invoke(nameof(ShowNextDialogue), 3f);
            }
        }
        else
        {
            EndDialogueSequence();
        }
    }
    public Events CurrentMonologueEvent => currentMonologueEvent;
}
public interface IMonologueSpeaker
{
    public void StartMonologue(Events eventType);
}