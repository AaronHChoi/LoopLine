using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeakerTest : MonoBehaviour
{
    [System.Serializable]
    public class DialogueCollection
    {
        public Events eventType;
        public DialogueSOTest dialogue;
    }

    [SerializeField] private List<DialogueCollection> dialogues = new List<DialogueCollection>();
    [SerializeField] private Events currentEvent = Events.Without_Camera;

    private Dictionary<Events, DialogueSOTest> dialogueDictionary = new Dictionary<Events, DialogueSOTest>();

    private void Awake()
    {
        foreach (var dialogueCollection in dialogues)
        {
            if (dialogueCollection.dialogue != null)
            {
                dialogueDictionary[dialogueCollection.eventType] = dialogueCollection.dialogue;
            }
        }
    }

    public void Interact()
    {
        if (DialogueManagerTest.Instance.IsDialogueActive()) return;

        if (dialogueDictionary.ContainsKey(currentEvent))
        {
            DialogueManagerTest.Instance.StartDialogue(dialogueDictionary[currentEvent]);
        }
    }

    public void SetCurrentEvent(Events newEvent)
    {
        currentEvent = newEvent;
    }

    public void TriggerDialogueByEvent(Events eventType)
    {
        if (dialogueDictionary.ContainsKey(eventType))
        {
            DialogueManagerTest.Instance.StartDialogue(dialogueDictionary[eventType]);
        }
        else
        {
            Debug.LogWarning($"No dialogue found for event: {eventType} on speaker: {gameObject.name}");
        }
    }

    public void TriggerDialogueById(string dialogueId)
    {
        foreach (var dialogueCollection in dialogues)
        {
            if (dialogueCollection.dialogue != null &&
                dialogueCollection.dialogue.dialogueId == dialogueId)
            {
                DialogueManagerTest.Instance.StartDialogue(dialogueCollection.dialogue);
                return;
            }
        }

        Debug.LogWarning($"No dialogue found with ID: {dialogueId} on speaker: {gameObject.name}");
    }
}