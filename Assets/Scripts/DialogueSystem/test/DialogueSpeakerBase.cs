using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Player;
using UnityEngine;

[Serializable]
public class DialogueGroup
{
    public Events eventType;
    public List<DialogueSO2> dialogues = new List<DialogueSO2>();
}
public abstract class DialogueSpeakerBase : MonoBehaviour
{
    [SerializeField] protected List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();
    [SerializeField] protected Events currentEvent = Events.Test;
    [SerializeField] protected string id;

    [SerializeField] protected bool autoAdvance = false;

    protected List<DialogueSO2> currentDialogues = new List<DialogueSO2>();
    protected int currentDialogueIndex = 0;
    protected bool isShowingDialogue = false;

    protected IPlayerStateController playerStateController;

    protected virtual void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();   
    }
    protected virtual void Start()
    {
        LoadDialoguesForCurrentEvent();
    }
    protected virtual void LoadDialoguesForCurrentEvent()
    {
        DialogueGroup group = dialogueGroups.FirstOrDefault(g => g.eventType == currentEvent);

        if (group != null && group.dialogues.Count > 0)
        {
            currentDialogues = new List<DialogueSO2>(group.dialogues);
            currentDialogueIndex = 0;
        }
        else
        {
            currentDialogues.Clear();
            Debug.LogWarning($"No hay diálogos para el evento: {currentEvent}");
        }
    }
    public void StartDialogueSequence()
    {
        if (currentDialogues.Count == 0)
        {
            Debug.LogWarning($"No hay diálogos disponibles para el evento: {currentEvent}");
            return;
        }

        isShowingDialogue = true;
        currentDialogueIndex = 0;
        ShowCurrentDialogue();
    }
    protected virtual void ShowCurrentDialogue()
    {
        if (currentDialogueIndex < currentDialogues.Count)
        {
            DialogueSO2 dialogue = currentDialogues[currentDialogueIndex];
            DialogueManager2.Instance.ShowDialogue(dialogue, this);
        }
        else
        {
            EndDialogueSequence();
        }
    }
    public virtual void ShowNextDialogue()
    {
        if (!isShowingDialogue) return;

        currentDialogueIndex++;

        if (currentDialogueIndex < currentDialogues.Count)
        {
            ShowCurrentDialogue();
        }
        else
        {
            EndDialogueSequence();
        }
    }
    protected virtual void EndDialogueSequence()
    {
        isShowingDialogue = false;
        currentDialogueIndex = 0;
        DialogueManager2.Instance.HideDialogue();
    }
    public virtual void SetCurrentEvent(Events newEvent)
    {
        currentEvent = newEvent;
        LoadDialoguesForCurrentEvent();
        Debug.Log($"Evento cambiado a: {currentEvent}");
    }
    public virtual void TriggerEventDialogue(Events eventType)
    {
        SetCurrentEvent(eventType);
        StartDialogueSequence();
    }
    public virtual void HandleEventChange(string targetNPC, Events newEvent)
    {
        if(targetNPC == id || targetNPC == "ALL")
        {
            SetCurrentEvent(newEvent);
        }
    }
}