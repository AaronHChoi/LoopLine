using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSpeaker : MonoBehaviour, IInteract, IObserver, IDependencyInjectable
{
    [SerializeField] private string interactText = "Interact with me!";
    [Tooltip("Este valor solo para los NPC, para poder identificar los dialogos")]
    public string id = "L";
    public List<DialogueSO> AvailableDialogs = new List<DialogueSO>();
    public int dialogueIndex = 0;
    public int DialogueLocalIndex = 0;
    public bool isDialogueActive = false;
    public bool NPCInteracted = false;
    [SerializeField] Transform headTarget;

    Subject eventManager;

    IUIManager uiManager;

    [Serializable]
    public class DialogueStateChange
    {
        public DialogueSO dialogue;
        public bool unlock;
    }
    [Serializable]
    public class DialogueEvent
    {
        public Events EventType;
        public List<DialogueStateChange> changes;
    }
    public List<DialogueEvent> DialogueEvents;

    #region MAGIC_METHODS
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    private void Start()
    {
        dialogueIndex = 0;
        DialogueLocalIndex = 0;

        DialogueRefresh();
    }
    public void OnNotify(Events _event, string _id = null)
    {
        if (_event == Events.TriggerMonologue)
            TriggerPlayerDialogue();
    }
    private void OnEnable()
    {
        if(eventManager != null)
            eventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        if (eventManager != null)
            eventManager.RemoveObserver(this);
    }
    #endregion
    public void TriggerEventDialogue(Events triggeredEvent)
    {
        DialogueEvent config = DialogueEvents.Find(e => e.EventType == triggeredEvent);

        if (config == null)
        {
            Debug.LogWarning($"No DialogueEvent found for {triggeredEvent}");
            return;
        }

        foreach (var change in config.changes)
        {
            if (change.dialogue == null)
            {
                Debug.LogWarning("DialogueSO is null in changes");
                continue;
            }

            if (AvailableDialogs.Contains(change.dialogue))
            {
                change.dialogue.Unlocked = change.unlock;
                Debug.Log($"Dialogue {change.dialogue.name} set to {change.unlock}");
            }
            else
            {
                Debug.LogWarning($"DialogueSO {change.dialogue.name} not found in AvailableDialogs of {name}");
            }
        }
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        eventManager = provider.SubjectEventManager;
    }
    public void DialogueTrigger()
    {
        //if (lookAtNPC != null && headTarget != null)
        //{
        //    lookAtNPC.SetTarget(headTarget);
        //}

        //developmentManager.DeactivateUIIfActive();

        if (isDialogueActive) return;

        //GameManager.Instance.SetBool(id, true);
        
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
                        DialogueManager.Instance.ShowUI(true, true);
                        DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    }
                    StartDialogue();
                    DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                    return;
                }
                StartDialogue();
                DialogueManager.Instance.ShowUI(true, true);
                DialogueManager.Instance.SetDialogue(AvailableDialogs[dialogueIndex], this);
                //dialogueIndex++;
            }
            else
            {
                Debug.LogWarning("La conversacion esta bloqueada");
                EndDialogue();
                DialogueManager.Instance.ShowUI(false, true);
                return;
            }
        }
        else
        {
            print("Fin del dialogo");
            EndDialogue();
            DialogueManager.Instance.ShowUI(false, true);
        }
        //DialogueRefresh();
    }
    void StartDialogue()
    {
        isDialogueActive = true;
    }
    public void EndDialogue()
    {
        isDialogueActive = false;
        //dialogueSOManager.CheckFirstInteraction();

        //if (lookAtNPC != null)
        //{
        //    lookAtNPC.ClearTarget();
        //}
    }
    bool DialogueUpdate()
    {
        if (!AvailableDialogs[dialogueIndex].ReUse)
        {
            int nextIndex = dialogueIndex + 1;

            while(nextIndex < AvailableDialogs.Count)
            {
                if (AvailableDialogs[nextIndex].Unlocked)
                {
                    dialogueIndex = nextIndex;
                    return true;

                }
                nextIndex++;
            }
            return false;
            //if(dialogueIndex < AvailableDialogs.Count - 1)
            //{
            //    dialogueIndex++;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        else
        {
            return true;
        }
    }
    public void Interact()
    {
        if(SceneManager.GetActiveScene().name == "05. MindPlace" && GameManager.Instance.test)
        {
            var clue = GetComponent<Clue>();
            clue.Interact();
        }
        else
        {
            if (AvailableDialogs == null || AvailableDialogs.Count == 0)
            {
                //uiManager.ShowUIText("No hay dialogos disponibles");
                StartCoroutine(ExecuteAfterDelay());
            }
            else
            {
                DialogueTrigger();
            }
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
        dialogueIndex = 0;
        var playerSpeaker = FindFirstObjectByType<PlayerController>().GetComponent<DialogueSpeaker>();

        if (playerSpeaker != null)
            playerSpeaker.DialogueTrigger();
    }
}