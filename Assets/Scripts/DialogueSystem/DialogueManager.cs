using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static event Action OnDialogueEnded;
    public static DialogueManager Instance { get; private set; }
    public bool IsDialogueInProgress { get; private set; }

    private Queue<DialogueTurn> dialogueTurnsQueue;
    private PlayerController player;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private DialogueUI dialogueUI;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        dialogueUI.HideDialogueBox();
        player = FindFirstObjectByType<PlayerController>();
    }
    public void StartDialogue(DialogueRoundSO dialogue)
    {
        if (IsDialogueInProgress)
        {
            Debug.LogWarning("Dialogue already in progress");
            return;
        }
        player.SetControllerEnabled(false);

        IsDialogueInProgress = true;
        dialogueTurnsQueue = new Queue<DialogueTurn>(dialogue.DialogueTurnsList);
        StartCoroutine(DialogueCoroutine());
    }
    private IEnumerator DialogueCoroutine()
    {
        dialogueUI.ShowDialogueBox();
        while(dialogueTurnsQueue.Count > 0)
        {
            var CurrentTurn = dialogueTurnsQueue.Dequeue();
            dialogueUI.SetCharacterInfo(CurrentTurn.Character);
            dialogueUI.ClearDialogueArea();
            yield return StartCoroutine(TypeSentence(CurrentTurn));

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return));
            yield return null;
        }
        dialogueUI.HideDialogueBox();
        IsDialogueInProgress = false;

        player.SetControllerEnabled(true);
        OnDialogueEnded?.Invoke();
    }
    private IEnumerator TypeSentence(DialogueTurn dialogTurn)
    {
        var typingWaitSeconds = new WaitForSeconds(typingSpeed);
        foreach(char letter in dialogTurn.DialogueLine.ToCharArray())
        {
            dialogueUI.AppendToDialogueArea(letter);
            yield return typingWaitSeconds;
        }
    }
}
