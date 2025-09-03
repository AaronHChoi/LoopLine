using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDialogueManager : Subject
{
    [SerializeField] float delayMonologue;
    public static System.Action<string> OnItemPicked;
    HashSet<string> receivedClueId = new HashSet<string>();

    private void Start()
    {
        StartCoroutine(StartSceneMonologue(delayMonologue));
    }
    private void OnEnable()
    {
        DialogueUI.OnDialogueEndedById += HandleDialgoueFinished;
        OnItemPicked += HandleItemPicked;
        PhotoCapture.OnPhotoClueCaptured += HandlePhotoClueCaptured;
    }
    private void OnDisable()
    {
        DialogueUI.OnDialogueEndedById -= HandleDialgoueFinished;
        OnItemPicked -= HandleItemPicked;
        PhotoCapture.OnPhotoClueCaptured += HandlePhotoClueCaptured;
    }
    void HandlePhotoClueCaptured(string clueId)
    {
        bool isFirstTimeForThisClue = false;

        if(!string.IsNullOrEmpty(clueId) && !receivedClueId.Contains(clueId))
        {
            receivedClueId.Add(clueId);
            isFirstTimeForThisClue = true;
        }

        if (clueId == null)
        {
            SendOnlyEvent(Events.With_Any_Good_Photos);
        } else if (clueId == "Event")
        {
            SendOnlyEvent(Events.With_Event_Photo);
        } else if (clueId == "Person")
        {
            SendOnlyEvent(Events.With_Some_Good_Photos);
        }
    }
    void HandleItemPicked(string itemId)
    {
        if(itemId == "Camera")
        {
            SendOnlyEvent(Events.With_Camera);
        }
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
    void SendOnlyEvent(Events _event)
    {
        NotifyObservers(_event);
    }
}