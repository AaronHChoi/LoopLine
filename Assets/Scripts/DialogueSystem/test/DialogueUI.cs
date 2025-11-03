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
    [SerializeField] private Color dialogueColor;
    [SerializeField] private Color monologueColor;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool skipTypingOnClick = true;

    [SerializeField] SoundData typeSound;

    private bool isTyping = false;
    private string fullText;
    private DialogueSO2 currentDialogue;
    private int currentLineIndex = 0;

    DialogueSpeakerBase currentSpeaker;
    Coroutine typingCoroutine;

    IPlayerStateController playerStateController;
    IDialogueManager dialogueManager;
    IClock mindplaceClock;
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        mindplaceClock = InterfaceDependencyInjector.Instance.Resolve<IClock>();
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
        if(mindplaceClock != null)
        {
            mindplaceClock.OnEnterClock += OnClockStartedHandler;
            mindplaceClock.OnExitClock += OnClockEndedHandler;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnDialogueNext -= HandleInteraction;
        }
        dialogueManager.OnDialogueStarted -= OnDialogueStartedHandler;
        dialogueManager.OnDialogueEnded -= OnDialogueEndedHandler;
        if (mindplaceClock != null)
        {
            mindplaceClock.OnEnterClock -= OnClockStartedHandler;
            mindplaceClock.OnExitClock -= OnClockEndedHandler;
        }
    }
    private void OnDialogueStartedHandler()
    {
        ShowletterBox(true);
    }
    private void OnDialogueEndedHandler()
    {
        ShowletterBox(false);
    }
    private void OnClockStartedHandler()
    {
        ShowletterBox(true);
    }
    private void OnClockEndedHandler()
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
            return;
        }

        if (currentDialogue == null)
            return;

        if (currentLineIndex < currentDialogue.lines.Length - 1)
        {
            ShowNextLine();
        }
        else
        {
            if (currentSpeaker != null)
            {
                currentSpeaker.ShowNextDialogue();
            }
            else
            {
                dialogueManager.HideDialogue();
            }
        }
    }
    private Dictionary<NPCType, string> npcDisplayNames = new Dictionary<NPCType, string>()
    {
         { NPCType.None, "None" },
         { NPCType.Test, "Tester" },
         { NPCType.CameraGirl, "Camera Girl" },
         { NPCType.MysteryBoy, "Mystery Boy" },
         { NPCType.WorkingMan, "Working Man" },
         { NPCType.BassGirl, "Bass Girl" },
         { NPCType.Player, "Claire" }
    };
    public void DisplayDialogue(DialogueSO2 data, DialogueSpeakerBase speaker = null)
    {
        if (currentDialogue == data && dialoguePanel.activeSelf)
        {
            ShowNextLine();
            return;
        }

        currentDialogue = data;
        currentLineIndex = 0;
        currentSpeaker = speaker;

        ChangeDialogeColor(data.IsAMonologue);

        playerStateController.ChangeState(playerStateController.DialogueState);
        
        dialoguePanel.SetActive(true);
        ShowCurrentLine();
    }
    private void ChangeDialogeColor(bool isMonologue)
    {
        dialogueText.color = isMonologue ? monologueColor : dialogueColor;
    }
    private void ShowCurrentLine()
    {
        var line = currentDialogue.lines[currentLineIndex];
        NPCType npcTypeToUse = line.npcType;
        string npcName = GetNPCName(npcTypeToUse, currentDialogue.IsAMonologue);

        string baseText = currentDialogue.IsAMonologue ? ApplyItalicFormat(line.dialogueText) : line.dialogueText;
        fullText = $"{npcName}{baseText}";

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }
    public void ShowNextLine()
    {
        if (currentDialogue == null) return;

        currentLineIndex++;

        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Length)
        {
            dialogueManager.HideDialogue();
            return;
        }

        ShowCurrentLine();
    }
    private string GetNPCName(NPCType npcType, bool isMonologue = false)
    {
        if (npcType == NPCType.None)
        {
            return "";
        }

        if (npcDisplayNames.TryGetValue(npcType, out string displayName))
        {
            return isMonologue ? $"{ApplyItalicFormat(displayName)}: " : $"{displayName}: ";
        }

        string enumName = npcType.ToString();

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

        int i = 0;
        while (i < fullText.Length)
        {
            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    dialogueText.text += fullText.Substring(i, tagEnd - i + 1);
                    i = tagEnd + 1;
                    continue;
                }
            }
            dialogueText.text += fullText[i];

            i++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
    }
    private string ApplyItalicFormat(string text)
    {
        return $"<i>{text}</i>";
    }
}