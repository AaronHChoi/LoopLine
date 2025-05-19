using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class DialogueSpeaker : MonoBehaviour, IInteract, IObserver
{
    [SerializeField] private string interactText = "Interact with me!";
    [Tooltip("Este valor solo para los NPC, para poder identificar los dialogos")]
    public string id = "L";
    public List<DialogueSO> AvailableDialogs = new List<DialogueSO>();
    public int dialogueIndex = 0;
    public int DialogueLocalIndex = 0;
    public bool isDialogueActive = false;

    DevelopmentManager developmentManager;
    UIManager uiManager;
    Subject eventManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer container)
    {
        uiManager = container.UIManager;
        developmentManager = container.DevelopmentManager;
        eventManager = container.SubjectEventManager;
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
        if (_event == Events.TriggerMonologue)
            TriggerPlayerDialogue();
    }
    private void OnEnable()
    {
        eventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        eventManager.RemoveObserver(this);
    }
    public void DialogueTrigger()
    {
        developmentManager.DeactivateUIIfActive();

        if (isDialogueActive) return;

        Debug.Log("Trigger");
        
        while (dialogueIndex < AvailableDialogs.Count && !AvailableDialogs[dialogueIndex].Unlocked)
        {
            dialogueIndex++;
        }

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
                //dialogueIndex++;
            }
            else
            {
                Debug.LogWarning("La conversacion esta bloqueada");
                EndDialogue();
                DialogueManager.Instance.ShowUI(false);
                return;
            }
        }
        else
        {
            print("Fin del dialogo");
            EndDialogue();
            DialogueManager.Instance.ShowUI(false);
        }
        //DialogueRefresh();
    }
    private int FindNextUnlockedDialogue(int startIndex)
    {
        for (int i = startIndex; i < AvailableDialogs.Count; i++)
        {
            if (AvailableDialogs[i].Unlocked)
            {
                return i;
            }
        }
        return -1;
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
        if(AvailableDialogs == null || AvailableDialogs.Count == 0)
        {
            uiManager.ShowUIText("No hay dialogos disponibles");
            StartCoroutine(ExecuteAfterDelay());
        }
        else
        {
            DialogueTrigger();
        }
    }
    private IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        uiManager.HideUIText();
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
}
