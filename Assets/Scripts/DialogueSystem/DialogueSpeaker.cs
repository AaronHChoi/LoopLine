using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour, IInteract, IObserver
{
    [SerializeField] private string interactText = "Interact with me!";
    public string id = "";
    public List<DialogueSO> AvailableDialogs = new List<DialogueSO>();
    [SerializeField] private int dialogueIndex = 0;
    public int DialogueLocalIndex = 0;
    public Subject EventManager;

    public bool isDialogueActive = false;
    private void Awake()
    {
        EventManager = FindFirstObjectByType<Subject>();
    }
    private void Start()
    {
        dialogueIndex = 0;
        DialogueLocalIndex = 0;

        //
        DialogueRefresh();
        //
    }
    public void OnNotify(Events _event)
    {
        if (_event == Events.TriggerDialogue)
            TriggerPlayerDialogue();
    }
    private void OnEnable()
    {
        EventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        EventManager.RemoveObserver(this);
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
        if (interactText == null) return interactText = "";
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
    public void TriggerPlayerDialogue()
    {
        var playerSpeaker = FindFirstObjectByType<PlayerController>().GetComponent<DialogueSpeaker>();

        if (playerSpeaker != null)
            playerSpeaker.DialogueTrigger();
    }
    public void TriggerNPCDialogue(string _id)
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject npc in npcs)
        {
            DialogueSpeaker npcDialogueSpeaker = npc.GetComponent<DialogueSpeaker>();
            if(npcDialogueSpeaker == null)
                continue;

            if(npcDialogueSpeaker.id == _id)
            {
                npcDialogueSpeaker.DialogueTrigger();
                break;
            }
        }
    }
}
