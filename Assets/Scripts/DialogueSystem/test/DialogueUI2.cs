using System.Collections;
using DependencyInjection;
using Player;
using TMPro;
using UnityEngine;

public class DialogueUI2 : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] GameObject continueIndicator;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool skipTypingOnClick = true;

    [SerializeField] SoundData typeSound;

    private bool isTyping = false;
    private string fullText;
    DialogueSpeakerBase currentSpeaker;
    Coroutine typingCoroutine;

    IPlayerStateController playerStateController;
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void Start()
    {
        HideDialogue();
    }
    private void Update()
    {
        if (continueIndicator != null)
        {
            continueIndicator.SetActive(dialoguePanel.activeSelf && !isTyping);
        }
    }
    private void OnEnable()
    {
        if(playerStateController != null)
        {
            playerStateController.OnDialogueNext += HandleInteraction;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnDialogueNext -= HandleInteraction;
        }
    }
    private void HandleInteraction()
    {
        if (isTyping)
        {
            CompleteTyping();
        }
        else
        {
            currentSpeaker?.ShowNextDialogue();
        }
    }
    public void DisplayDialogue(DialogueSO2 data, DialogueSpeakerBase speaker = null)
    {
        playerStateController.ChangeState(playerStateController.DialogueState);
        dialoguePanel.SetActive(true);
        currentSpeaker = speaker;

        fullText = data.dialogueText;
        typingCoroutine = StartCoroutine(TypeText());
    }
    public void HideDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialoguePanel.SetActive(false);
        isTyping = false;
        currentSpeaker = null;
        playerStateController.ChangeState(playerStateController.NormalState);
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

        var soundInstance = SoundManager.Instance.CreateSound()
            .WithSoundData(typeSound);

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            soundInstance.Play();
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
    }
    public bool IsTyping()
    {
        return isTyping;
    }
}