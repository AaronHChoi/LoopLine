using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText = "Interact with me!";
    public List<DialogueSO> AvailableDialogs = new List<DialogueSO>();
    [SerializeField] private int dialogueIndex = 0;
    public int DialogueLocalIndex = 0;
    private void Start()
    {
        dialogueIndex = 0;
        DialogueLocalIndex = 0;

        //Solo para el editor COMENTAR CUANDO SE HACE BUILD
        foreach(var dial in AvailableDialogs)
        {
            dial.Finished = false;
            var question = dial.Questions;
            if(question != null)
            {
                foreach (var option in question.Options)
                {
                    option.dialogue.Finished = false;
                }
            }
        }
        ///////////////////////////////////////////////////
    }
    public void DialogueTrigger()
    {
        Debug.Log("Trigger");
        if(dialogueIndex <= AvailableDialogs.Count - 1)
        {
            if (AvailableDialogs[dialogueIndex].Unlocked)
            {
                if (AvailableDialogs[dialogueIndex].Finished)
                {
                    if (DialogueUpdate())
                    {
                        DialogueManager2.Instance.ShowUI(true);
                        DialogueManager2.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    }
                    DialogueManager2.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    return;
                }
                DialogueManager2.Instance.ShowUI(true);
                DialogueManager2.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
            }
            else
            {
                Debug.LogWarning("La conversacion esta bloqueada");
                DialogueManager2.Instance.ShowUI(false);
            }
        }
        else
        {
            print("Fin del dialogo");
            DialogueManager2.Instance.ShowUI(false);
        }
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
}
