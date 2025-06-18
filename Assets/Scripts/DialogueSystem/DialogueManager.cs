using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDependencyInjectable, IDialogueManager
{
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    public static DialogueManager Instance { get; private set; }
    public static DialogueSpeaker actualSpeaker;
    
    public bool isDialogueActive = false;
    
    public List<DialogueSO> AllDialogues = new List<DialogueSO>();

    public QuestionManager QuestionManager
    {
        get { return questionManager; }
        private set { value = questionManager; }
    }

    DialogueUI dialogueUI;
    QuestionManager questionManager;

    IUIManager uiManager;
    IPlayerController playerController;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InjectDependencies(DependencyContainer.Instance);
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        questionManager = provider.QuestionManager;
        dialogueUI = provider.DialogueUI;
    }
    private void Start()
    {
        ShowUI(false, true);
    }
    public void ShowUI(bool _show, bool _event)
    {
        dialogueUI.gameObject.SetActive(_show);
        if (!_show)
        {
            dialogueUI.localIndex = 0;
            OnDialogueEnded?.Invoke();
            if(_event)
                playerController.SetCinemachineController(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isDialogueActive = false;
            uiManager.HideUIText();
        }
        else
        {
            OnDialogueStarted?.Invoke();
            playerController.SetCinemachineController(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isDialogueActive = true;
        }
    }
    public void SetDialogue(DialogueSO _dialogue, DialogueSpeaker speaker)
    {
        if(speaker != null)
        {
            actualSpeaker = speaker;
        }
        else
        {
            dialogueUI.Dialogue = _dialogue;
            dialogueUI.localIndex = 0;
            dialogueUI.TextUpdate(0);
        }
        if (_dialogue.Finished && !_dialogue.ReUse)
        {
            dialogueUI.Dialogue = _dialogue;
            dialogueUI.localIndex = _dialogue.Dialogues.Length;
            dialogueUI.TextUpdate(1);
        }
        else
        {
            dialogueUI.Dialogue = _dialogue;
            dialogueUI.localIndex = actualSpeaker.DialogueLocalIndex;
            dialogueUI.TextUpdate(0);
        }
    }
    //metodo para cambiar el estado de reuse
    public void ChangeTheReUseStatus(DialogueSO _dialogo, bool status)
    {
        _dialogo.ReUse = status;
    }
    //metodo para desbloquear x dialogo
    public void LockingAndUnlockinkUpdates(DialogueSO _dialogue, bool unlocking)
    {
        _dialogue.Unlocked = unlocking;
    }

    public void ResetAllDialogues()
    {
        foreach(DialogueSO dialogue in AllDialogues)
        {
            if(dialogue != null)
            {
                dialogue.ResetValues();
            }
        }
    }
    public void StopAndFinishDialogue() //Metodo para parar dialogos
    {
        if(actualSpeaker != null)
        {
            foreach (var dialogue in actualSpeaker.AvailableDialogs)
            {
                dialogue.Finished = true;
            }
            actualSpeaker.dialogueIndex = 0;
            actualSpeaker.DialogueLocalIndex = 0;
            actualSpeaker.isDialogueActive = false;
        }
        dialogueUI.gameObject.SetActive(false);
        ShowUI(false, false);
    }
}
public interface IDialogueManager
{
    void ResetAllDialogues();
    void StopAndFinishDialogue();
}