using System.Collections;
using UnityEngine;

public class EventDialogueManager : Subject
{
    [SerializeField] float delayMonologue;
    private void Start()
    {
        StartCoroutine(StartSceneMonologue(delayMonologue));
    }
    private void OnEnable()
    {
        DialogueUI.OnDialogueEndedById += HandleDialgoueFinished;
    }
    private void OnDisable()
    {
        DialogueUI.OnDialogueEndedById -= HandleDialgoueFinished;
    }
    void HandleDialgoueFinished(DialogueSO dialogue)
    {
        if (dialogue.events != Events.None)
        {
            StartCoroutine(StartSceneMonologue(delayMonologue, dialogue.events));
        }
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
    }
    private IEnumerator StartSceneMonologue(float delay, Events _event)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(_event);
        NotifyObservers(Events.TriggerMonologue);
    }
}