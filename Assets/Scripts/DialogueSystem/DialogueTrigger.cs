using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteract
{
    public static event Action OnDialogueStarted;

    [SerializeField] private DialogueRoundSO dialogue;
    [SerializeField] private string interactText = "Interact with me!";
    [SerializeField] private InteractUI interactUI;
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
        DialogueManager.Instance.StartDialogue(dialogue);
        Debug.Log("Disparando evento OnDialogueStarted");
        OnDialogueStarted?.Invoke();
    }
}
