using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DialogueSOManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEvent
    {
        public string EventName;
        public List<DialogueSO> DialoguesToUnlock;
        public List<DialogueSO> DialoguesToLock;
    }
    public List<DialogueEvent> DialogueEvents;

    public void TriggerEventDialogue(string eventName)
    {
        DialogueEvent dialogueEvent = DialogueEvents.Find(e => e.EventName == eventName);

        foreach (DialogueSO dialogue in dialogueEvent.DialoguesToUnlock)
        {
            dialogue.Unlocked = true;
            dialogue.Finished = false;
        }
        foreach (DialogueSO dialogue in dialogueEvent.DialoguesToLock)
        {
            dialogue.Unlocked = false;
            dialogue.Finished = false;
        }
    }
}
