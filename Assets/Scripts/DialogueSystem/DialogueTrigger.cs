using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteract
{
    public static event Action OnDialogueStarted;

    [SerializeField] private DialogueRoundSO dialogue;
    [SerializeField] private string interactText = "Interact with me!";
    private DialogueManager dialogueManager;
    private void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue);
        Debug.Log("Disparando evento OnDialogueStarted");
        OnDialogueStarted?.Invoke();
    }
}
