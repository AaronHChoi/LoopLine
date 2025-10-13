using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerTest : MonoBehaviour
{
    public static DialogueManagerTest Instance { get; private set; }

    [SerializeField] private DialogueUITest dialogueUI;

    private Queue<DialogueSOTest.DialogueLine> currentDialogueLines = new Queue<DialogueSOTest.DialogueLine>();
    private bool isDialogueActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(DialogueSOTest dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Length == 0) return;

        currentDialogueLines.Clear();

        foreach (var line in dialogue.dialogueLines)
        {
            currentDialogueLines.Enqueue(line);
        }

        isDialogueActive = true;
        dialogueUI.ShowDialogueUI();
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentDialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        var nextLine = currentDialogueLines.Dequeue();
        dialogueUI.DisplayDialogueLine(nextLine.text);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.HideDialogueUI();
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}