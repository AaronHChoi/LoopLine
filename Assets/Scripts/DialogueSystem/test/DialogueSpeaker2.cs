using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DialogueGroup
{
    public Events eventType;
    public List<DialogueSO2> dialogues = new List<DialogueSO2>();
}
public class DialogueSpeaker2 : MonoBehaviour
{
    [SerializeField] private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();

    [SerializeField] private Events currentEvent = Events.BreakCrystal;

    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private bool autoAdvance = false;

    private Transform player;
    private bool playerInRange = false;
    private List<DialogueSO2> currentDialogues = new List<DialogueSO2>();
    private int currentDialogueIndex = 0;
    private bool isShowingDialogue = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        LoadDialoguesForCurrentEvent();
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (!isShowingDialogue)
            {
                StartDialogueSequence();
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    private void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        playerInRange = distance <= interactionDistance;

        if (!playerInRange && isShowingDialogue)
        {
            EndDialogueSequence();
        }
    }
    private void LoadDialoguesForCurrentEvent()
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
    private void ShowCurrentDialogue()
    {
        if (currentDialogueIndex < currentDialogues.Count)
        {
            DialogueSO2 dialogue = currentDialogues[currentDialogueIndex];
            DialogueManager2.Instance.ShowDialogue(dialogue);

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
    public void ShowNextDialogue()
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
    private void EndDialogueSequence()
    {
        isShowingDialogue = false;
        currentDialogueIndex = 0;
        DialogueManager2.Instance.HideDialogue();
    }
    public void SetCurrentEvent(Events newEvent)
    {
        currentEvent = newEvent;
        LoadDialoguesForCurrentEvent();
        Debug.Log($"Evento cambiado a: {currentEvent}");
    }
    public void TriggerEventDialogue(Events eventType)
    {
        SetCurrentEvent(eventType);
        StartDialogueSequence();
    }
    public DialogueSO2 GetDialogueById(string id)
    {
        foreach (var group in dialogueGroups)
        {
            DialogueSO2 found = group.dialogues.FirstOrDefault(d => d.dialogueId == id);
            if (found != null) return found;
        }
        return null;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}