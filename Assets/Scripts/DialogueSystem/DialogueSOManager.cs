using System.Collections.Generic;
using UnityEngine;

public class DialogueSOManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEvent
    {
        public string EventName;
        public List<DialogueSO> DialoguesToAdd;
    }
    public List<DialogueEvent> DialogueEvents;
    public DialogueSpeaker dialogueSpeaker;
    private void Awake()
    {
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
    }
    public void TriggerEventDialogue(string _eventName)
    {
        DialogueEvent dialogueEvent = DialogueEvents.Find(e => e.EventName == _eventName);

        dialogueSpeaker.AvailableDialogs.Clear();

        foreach (DialogueSO dialogue in dialogueEvent.DialoguesToAdd)
        {
            if (!dialogueSpeaker.AvailableDialogs.Contains(dialogue))
                dialogueSpeaker.AvailableDialogs.Add(dialogue);
        }
        dialogueSpeaker.dialogueIndex = 0;
    }
}