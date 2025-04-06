using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteract
{
    [SerializeField] private DialogueRoundSO dialogue;
    [SerializeField] private string interactText = "Interact with me!";

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
    }
}
