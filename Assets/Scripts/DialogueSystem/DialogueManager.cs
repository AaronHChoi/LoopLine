using System;
using System.Collections.Generic;
using UnityEngine;
using DependencyInjection;
using Player;

public class DialogueManager : MonoBehaviour, IDialogueManager
{
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;
    public static DialogueManager Instance { get; private set; }
    public DialogueSpeaker actualSpeaker {  get;  set;}
    
    private bool isDialogueActive = false;
    
    public List<DialogueSO> AllDialogues = new List<DialogueSO>();
    public List<DialogueSO> AllFirstDialogues = new List<DialogueSO>();
    public List<QuestionSO> AllQuestions = new List<QuestionSO>();
    public List<QuestionSO> SelectQuestions = new List<QuestionSO>();
    public bool IsDialogueActive { get { return isDialogueActive; } }
    public IQuestionManager QuestionManager
    {
        get { return questionManager; }
        private set { value = questionManager; }
    }

    IDialogueUI dialogueUI;
    IQuestionManager questionManager;
    IPlayerStateController playerState;

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
        dialogueUI = InterfaceDependencyInjector.Instance.Resolve<IDialogueUI>();
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        playerState = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        questionManager = InterfaceDependencyInjector.Instance.Resolve<IQuestionManager>();
    }

    private void Start()
    {
        ShowUI(false, true);
    }
    public void ShowUI(bool _show, bool _event)
    {
        dialogueUI.GetGameObject().SetActive(_show);
        if (!_show)
        {
            dialogueUI.localIndex = 0;
            OnDialogueEnded?.Invoke();
            if (_event)
                playerState.ChangeState(playerState.NormalState);
            isDialogueActive = false;
            uiManager.HideUIText();
            uiManager.ShowCrossHairFade(false);
        }
        else
        {
            OnDialogueStarted?.Invoke();
            playerState.ChangeState(playerState.DialogueState);
            isDialogueActive = true;
            uiManager.ShowCrossHairFade(true);
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
    public void UnlockFirstDialogues()
    {
        foreach (DialogueSO dialogue in AllFirstDialogues)
        {
            if (dialogue != null)
            {
                dialogue.ResetValues();
            }
        }
    }
    public void ResetAllDialogues()
    {
        foreach(DialogueSO dialogue in AllDialogues)
        {
            if(dialogue != null)
            {
                dialogue.ResetValuesToFalse();
            }
        }
    }
    public void ResetAllQuestions()
    {
        foreach (QuestionSO question in AllQuestions)
        {
            if(question != null)
            {
                question.ResetValues();
            }
        }
    }
    public void ResetSelectQuestions()
    {
        foreach (QuestionSO question in SelectQuestions)
        {
            if (question != null)
            {
                question.ResetValues();
            }
        }
    }
    public void StopAndFinishDialogue() //Metodo para parar dialogos
    {
        dialogueUI.GetGameObject().SetActive(false);
        if (actualSpeaker.isDialogueActive)
        {
            ShowUI(false, true);
        }
        else
        {
            ShowUI(false, false);
        }
        if (actualSpeaker != null)
        {
            foreach (var dialogue in actualSpeaker.AvailableDialogs)
            {
                dialogue.Finished = true;
            }
            actualSpeaker.dialogueIndex = 0;
            actualSpeaker.DialogueLocalIndex = 0;
            actualSpeaker.isDialogueActive = false;
        }
    }
}
public interface IDialogueManager
{
    event Action OnDialogueStarted;
    event Action OnDialogueEnded;
    DialogueSpeaker actualSpeaker {  get; set; }
    bool IsDialogueActive { get; }
    void ShowUI(bool _show, bool _event);
    void SetDialogue(DialogueSO _dialogue, DialogueSpeaker speaker);
    void ResetAllDialogues();
    void ResetAllQuestions();
    void ResetSelectQuestions();
    void StopAndFinishDialogue();
    void UnlockFirstDialogues();
}