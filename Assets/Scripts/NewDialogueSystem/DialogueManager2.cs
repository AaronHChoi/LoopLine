using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 Instance { get; private set; }
    public static DialogueSpeaker actualSpeaker;
    [SerializeField] private DialogueUI2 dialogueUI;
    [SerializeField] private GameObject player;

    public QuestionManager QuestionManager;
    private void Awake()
    {
        if(Instance = this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueUI = FindFirstObjectByType<DialogueUI2>();
        QuestionManager = FindFirstObjectByType<QuestionManager>();
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
            MouseController.UnlockCursor();
        }
        else
        {
            MouseController.LockCursor();
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
    public void ChangeTheReUseStatus(DialogueSO _dialogo, bool status)
    {
        _dialogo.ReUse = status;
    }
    public void LockingAndUnlockinkUpdates(DialogueSO _dialogue, bool unlocking)
    {
        _dialogue.Unlocked = unlocking;
    }
}
