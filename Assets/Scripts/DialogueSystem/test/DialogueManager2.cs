using UnityEngine;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 Instance { get; private set; }

    [SerializeField] private DialogueUI2 dialogueUI;
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

        currentSpeaker = speaker;
        dialogueUI.DisplayDialogue(data, speaker);
    }
    public void HideDialogue()
    {
        dialogueUI.HideDialogue();
        currentSpeaker = null;
    }
    public DialogueSpeakerBase GetCurrentSpeaker()
    {
        return currentSpeaker;  
    }
}