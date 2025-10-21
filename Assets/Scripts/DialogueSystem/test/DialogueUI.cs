using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using Player;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] GameObject continueIndicator;
    [SerializeField] private FadeInOutController letterBoxFadeInOut;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool skipTypingOnClick = true;

    [SerializeField] SoundData typeSound;

    private bool isTyping = false;
    private string fullText;
    DialogueSpeakerBase currentSpeaker;
    Coroutine typingCoroutine;

    IPlayerStateController playerStateController;
    IDialogueManager dialogueManager;

    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
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
        dialogueManager.OnDialogueStarted += OnDialogueStartedHandler;
        dialogueManager.OnDialogueEnded += OnDialogueEndedHandler;
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnDialogueNext -= HandleInteraction;
        }
        dialogueManager.OnDialogueStarted -= OnDialogueStartedHandler;
        dialogueManager.OnDialogueEnded -= OnDialogueEndedHandler;
    }
    private void OnDialogueStartedHandler()
    {
        ShowletterBox(true);
    }
    private void OnDialogueEndedHandler()
    {
        ShowletterBox(false);
    }
    private void ShowletterBox(bool showLetterBox)
    {
        letterBoxFadeInOut.ForceFade(showLetterBox);
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
    private Dictionary<NPCType, string> npcDisplayNames = new Dictionary<NPCType, string>()
    {
         { NPCType.None, "None" },
         { NPCType.Test, "Tester" },
         { NPCType.CameraGirl, "Camera Girl" },
         { NPCType.MysteryBoy, "Mystery Boy" },
         { NPCType.WorkingMan, "Working Man" },
         { NPCType.BassGirl, "Bass Girl" }
    };
    public void DisplayDialogue(DialogueSO2 data, DialogueSpeakerBase speaker = null)
    {
        if (data.IsAMonologue)
        {
            playerStateController.ChangeState(playerStateController.MonologueState);
        }
        else
        {
            playerStateController.ChangeState(playerStateController.DialogueState);
        }

        dialoguePanel.SetActive(true);
        currentSpeaker = speaker;

        string npcName = GetNPCName(speaker, data.IsAMonologue);

        string baseText = data.IsAMonologue ? ApplyItalicFormat(data.dialogueText) : data.dialogueText;
        fullText = $"{npcName}{baseText}";

        typingCoroutine = StartCoroutine(TypeText());
    }
    private string GetNPCName(DialogueSpeakerBase speaker, bool isMonologue = false)
    {
        if (speaker == null) return "";

        string displayName;
        if(npcDisplayNames.TryGetValue(speaker.NPCType, out displayName))
        {
            return isMonologue ? $"{ApplyItalicFormat(displayName)}: " : $"{displayName}: ";
        }

        string enumName = speaker.NPCType.ToString();
        string formattedName = System.Text.RegularExpressions.Regex
            .Replace(enumName, "([a-z])([A-Z])", "$1 $2");

        return isMonologue ? $"{ApplyItalicFormat(formattedName)}: " : $"{formattedName}: ";
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
    private string ApplyItalicFormat(string text)
    {
        return $"<i>{text}</i>";
    }
    public bool IsTyping()
    {
        return isTyping;
    }
}