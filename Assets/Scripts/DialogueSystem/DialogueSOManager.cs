using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueSOManager : MonoBehaviour
{
    [Serializable]
    public class DialogueStateChange
    {
        public DialogueSO dialogue;
        public bool unlock;
    }

    [Serializable]
    public class DialogueEvent
    {
        public Events EventType;
        public List<DialogueStateChange> changes;
    }
    public List<DialogueEvent> DialogueEvents;
    public DialogueSpeaker dialogueSpeaker;

    private void Awake()
    {
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
    }
    public void TriggerEventDialogue(Events triggeredEvent)
    {
        DialogueEvent config = DialogueEvents.Find(e => e.EventType == triggeredEvent);

        if (config == null)
        {
            Debug.LogWarning($"No DialogueEvent found for {triggeredEvent}");
            return;
        }

        foreach (var change in config.changes)
        {
            if (change.dialogue == null)
            {
                Debug.LogWarning("DialogueSO is null in changes");
                continue;
            }

            if (dialogueSpeaker.AvailableDialogs.Contains(change.dialogue))
            {
                change.dialogue.Unlocked = change.unlock;
                Debug.Log($"Dialogue {change.dialogue.name} set to {change.unlock}");
            }
            else
            {
                Debug.LogWarning($"DialogueSO {change.dialogue.name} not found in AvailableDialogs of {dialogueSpeaker.name}");
            }
        }
    }
}