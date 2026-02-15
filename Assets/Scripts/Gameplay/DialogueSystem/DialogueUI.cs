using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;
using Core.Audio;
using Core.DependencyInjection;
using Core.UI;

[System.Serializable]
public struct NPCAudioSettings
{
    public NPCType npcType;
    public float pitch;
    public float customTypingSpeed;
}

public class DialogueUI : MonoBehaviour, IDialogueUI
{
    public event Action OnTypingFinished;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] GameObject continueIndicator;
    [SerializeField] private Color dialogueColor;
    [SerializeField] private Color monologueColor;

    [SerializeField] private float typingSpeed = 0.05f;

    [SerializeField] SoundData typeSound;

    private bool isTyping = false;
    private string fullText;
    private DialogueSO2 currentDialogue;
    private int currentLineIndex = 0;

    [SerializeField] private List<NPCAudioSettings> npcAudioConfigs;
    private NPCAudioSettings currentAudioSettings;
    private float defaultPitch = 1f;

    DialogueSpeakerBase currentSpeaker;
    Coroutine typingCoroutine;

    IPlayerStateController playerStateController;
    IDialogueManager dialogueManager;
    IClock mindplaceClock;
    ISoundManager soundManager;
    IFadeInOutController letterBoxFadeInOut;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
        mindplaceClock = InterfaceDependencyInjector.Instance.Resolve<IClock>();
        letterBoxFadeInOut = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.CinematicCanvas);
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
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueStarted += OnDialogueStartedHandler;
            dialogueManager.OnDialogueEnded += OnDialogueEndedHandler;
        }
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
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueStarted -= OnDialogueStartedHandler;
            dialogueManager.OnDialogueEnded -= OnDialogueEndedHandler;
        }
        if (mindplaceClock != null)
        {
            mindplaceClock.OnEnterClock -= OnClockStartedHandler;
            mindplaceClock.OnExitClock -= OnClockEndedHandler;
        }
    }
    private void SetAudioSettingsForNPC(NPCType type)
    {
        currentAudioSettings = npcAudioConfigs.Find(x => x.npcType == type);
        
        if (currentAudioSettings.npcType == NPCType.None && type != NPCType.None)
        {
            currentAudioSettings.pitch = defaultPitch;
            currentAudioSettings.customTypingSpeed = typingSpeed;
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
         { NPCType.Player, "Claire" },
         { NPCType.StrangeVoice, "Strange Voice" },
         { NPCType.MusicTape, "Music Tape" }
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

        SetAudioSettingsForNPC(line.npcType);

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
        if (!GameManager.Instance.isCinematicMonologue)
        {
            playerStateController.ChangeState(playerStateController.NormalState);
        }
    }
    private void CompleteTyping()
    {
        StopAllCoroutines();
        dialogueText.text = fullText;
        isTyping = false;
        OnTypingFinished?.Invoke();
    }
    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

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

            soundManager.CreateSound()
                        .WithSoundData(typeSound)
                        .WithPitch(currentAudioSettings.pitch)
                        .Play();
            i++;

            float speed = currentAudioSettings.customTypingSpeed > 0 ? currentAudioSettings.customTypingSpeed : typingSpeed;

            yield return new WaitForSeconds(speed);
        }
        isTyping = false;
        typingCoroutine = null;

        OnTypingFinished?.Invoke();
    }
    private string ApplyItalicFormat(string text)
    {
        return $"<i>{text}</i>";
    }
}
public interface IDialogueUI
{
    event Action OnTypingFinished;
}