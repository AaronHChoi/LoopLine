using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI2 : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] GameObject continueIndicator;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool skipTypingOnClick = true;

    private bool isTyping = false;
    private string fullText;
    private DialogueSpeaker2 currentSpeaker;

    private void Start()
    {
        HideDialogue();
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping && skipTypingOnClick)
            {
                CompleteTyping();
            }
        }

        if (continueIndicator != null)
        {
            continueIndicator.SetActive(dialoguePanel.activeSelf && !isTyping);
        }
    }

    public void DisplayDialogue(DialogueSO2 data, DialogueSpeaker2 speaker = null)
    {
        dialoguePanel.SetActive(true);
        currentSpeaker = speaker;

        fullText = data.dialogueText;
        StartCoroutine(TypeText());
    }

    public void HideDialogue()
    {
        StopAllCoroutines();
        dialoguePanel.SetActive(false);
        isTyping = false;
        currentSpeaker = null;
    }
    private void CompleteTyping()
    {
        StopAllCoroutines();
        dialogueText.text = fullText;
        isTyping = false;
    }
    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    public bool IsTyping()
    {
        return isTyping;
    }
}