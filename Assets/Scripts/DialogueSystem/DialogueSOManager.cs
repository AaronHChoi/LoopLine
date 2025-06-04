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

    [SerializeField] private List<DialogueSO> dialoguesToCheck;

    IEventManager eventManager;
    private void Awake()
    {
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
        eventManager = InterfaceDependencyInjector.Instance.Resolve<IEventManager>();
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
    public void CheckFirstInteraction()
    {
        foreach (DialogueSO dialogue in dialoguesToCheck)
        {
            if (dialogue.Finished)
            {
                eventManager.AfterFirstInteraction(NPCname);
                break;
            }
        }
    }
}