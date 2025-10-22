using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialogueManager
{
    public static DialogueManager Instance { get; private set; }

    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    [SerializeField] private DialogueUI dialogueUI;
    DialogueSpeakerBase currentSpeaker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowDialogue(DialogueSO2 data, DialogueSpeakerBase speaker = null)
    {
        if (data == null)
        {
            Debug.LogWarning("DialogueData es null");
            return;
        }

        OnDialogueStarted?.Invoke();
        currentSpeaker = speaker;
        dialogueUI.DisplayDialogue(data, speaker);
    }
    public void HideDialogue()
    {
        OnDialogueEnded?.Invoke();
        dialogueUI.HideDialogue();
        currentSpeaker = null;
    }
    public DialogueSpeakerBase GetCurrentSpeaker()
    {
        return currentSpeaker;  
    }
}
public interface IDialogueManager
{
    event Action OnDialogueStarted;
    event Action OnDialogueEnded;
    void HideDialogue();
}