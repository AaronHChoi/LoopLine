using UnityEngine;
using System;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour, IDependencyInjectable
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
    PlayerController player;
    QuestionManager questionManager;
    UIManager uiManager;
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
            //if(Instance = this)
            //{
            //    Instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}
            //else
            //{
            //    Destroy(gameObject);
            //}
            //dialogueUI = FindFirstObjectByType<DialogueUI>();
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        uiManager = provider.UIManager;
        player = provider.PlayerController;
        questionManager = provider.QuestionManager;
        dialogueUI = provider.DialogueUI;
    }
    private void Start()
    {
        ShowUI(false);
    }
    public void ShowUI(bool show)
    {
        dialogueUI.gameObject.SetActive(show);
        if (!show)
        {
            dialogueUI.localIndex = 0;
            OnDialogueEnded?.Invoke();
            player.SetControllerEnabled(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isDialogueActive = false;
            uiManager.HideUIText();
        }
        else
        {
            OnDialogueStarted?.Invoke();
            player.SetControllerEnabled(false);
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
    public void StopAndFinishDialogue() //Metodo para para dialogos
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
        ShowUI(false);
    }
}
