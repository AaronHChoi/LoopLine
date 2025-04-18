using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText = "Interact with me!";
    public List<DialogueSO> AvailableDialogs = new List<DialogueSO>();
    [SerializeField] private int dialogueIndex = 0;
    public int DialogueLocalIndex = 0;

    public bool isDialogueActive = false;
    private void Start()
    {
        dialogueIndex = 0;
        DialogueLocalIndex = 0;

        //
        DialogueRefresh();
        //
    }
    public void DialogueTrigger()
    {
        if (isDialogueActive) return;

        Debug.Log("Trigger");
        if (dialogueIndex <= AvailableDialogs.Count - 1)
        {
            if (AvailableDialogs[dialogueIndex].Unlocked)
            {
                if (AvailableDialogs[dialogueIndex].Finished)
                {
                    if (DialogueUpdate())
                    {
                        StartDialogue();
                        DialogueManager.Instance.ShowUI(true);
                        DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    }
                    StartDialogue();
                    DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    return;
                }
                StartDialogue();
                DialogueManager.Instance.ShowUI(true);
                DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
            }
            else
            {
                Debug.LogWarning("La conversacion esta bloqueada");
                EndDialogue();
                DialogueManager.Instance.ShowUI(false);
            }
        }
        else
        {
            print("Fin del dialogo");
            EndDialogue();
            DialogueManager.Instance.ShowUI(false);
        }
        DialogueRefresh();
    }
    void StartDialogue()
    {
        isDialogueActive = true;
    }
    public void EndDialogue()
    {
        isDialogueActive = false;
    }
    bool DialogueUpdate()
    {
        if (!AvailableDialogs[dialogueIndex].ReUse)
        {
            if(dialogueIndex < AvailableDialogs.Count - 1)
            {
                dialogueIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public void Interact()
    {
        DialogueTrigger();
    }

    public string GetInteractText()
    {
        return interactText;
    }
    void DialogueRefresh()
    {
        foreach (var dial in AvailableDialogs)
        {
            dial.Finished = false;
            var question = dial.Questions;
            if (question != null)
            {
                foreach (var option in question.Options)
                {
                    option.dialogue.Finished = false;
                }
            }
        }
    }
}
