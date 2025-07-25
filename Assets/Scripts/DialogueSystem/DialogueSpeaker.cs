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


    DevelopmentManager developmentManager;
    Subject eventManager;
    DialogueSOManager dialogueSOManager;
    ItemInteract itemInteract;
    

    IUIManager uiManager;
    #region MAGIC_METHODS
    private void Awake()
    {
        dialogueSOManager = GetComponent<DialogueSOManager>();
        itemInteract = GetComponent<ItemInteract>();
        InjectDependencies(DependencyContainer.Instance);
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    private void Start()
    {
        dialogueIndex = 0;
        DialogueLocalIndex = 0;

        DialogueRefresh();

        if (itemInteract != null)
        {
            interactText = itemInteract.ItemData.itemName;
        }
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
    #endregion
    public void InjectDependencies(DependencyContainer provider)
    {
        developmentManager = provider.DevelopmentManager;
        eventManager = provider.SubjectEventManager;
        
    }
    public void DialogueTrigger()
    {
        //if (AvailableDialogs == null || AvailableDialogs.Count == 0 || (dialogueIndex == AvailableDialogs.Count - 1 && AvailableDialogs[dialogueIndex].Finished))
        //{
        //    //uiManager.ShowUIText("No hay dialogos disponibles.");
        //    StartCoroutine(ExecuteAfterDelay());
        //    return;
        //}

        developmentManager.DeactivateUIIfActive();

        if (isDialogueActive) return;

        Debug.Log("Trigger");

        GameManager.Instance.SetBool(id, true);
        
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
        itemInteract.Interact();
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
