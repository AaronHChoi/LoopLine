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
    [SerializeField] public string NPCname;

    #region MAGIC_METHODS
    private void Awake()
    {
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
    }
    #endregion

    public void TriggerEventDialogue(string _eventName)
    {
        if (DialogueEvents == null)
        {
            Debug.LogWarning("DialogueEvents list is null.");
            return;
        }

        DialogueEvent dialogueEvent = DialogueEvents.Find(e => e.EventName == _eventName);
        if (dialogueEvent == null)
        {
            Debug.LogWarning($"Dialogue event '{_eventName}' not found.");
            return;
        }

        if (dialogueSpeaker == null)
        {
            Debug.LogWarning("DialogueSpeaker component is missing.");
            return;
        }

        dialogueSpeaker.AvailableDialogs.Clear();

        if (dialogueEvent.DialoguesToAdd == null)
        {
            Debug.LogWarning($"DialoguesToAdd is null for event '{_eventName}'.");
            return;
        }

        foreach (DialogueSO dialogue in dialogueEvent.DialoguesToAdd)
        {
            if (dialogue != null && !dialogueSpeaker.AvailableDialogs.Contains(dialogue))
                dialogueSpeaker.AvailableDialogs.Add(dialogue);
        }

        dialogueSpeaker.dialogueIndex = 0;
    }
}