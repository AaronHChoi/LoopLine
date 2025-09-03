using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour, IDependencyInjectable
{
    public DialogueSO Dialogue;
    public DialogueSO MainDialogue;
    public static event Action<DialogueSO> OnDialogueEndedById;

    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private GameObject questionContainer;
    [SerializeField] private FadeInOutController letterBoxFadeInOut;

    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private AudioClip typingAudioSource;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private RawImage dialogueBackground;
    
    public int localIndex = 1;

    [SerializeField] bool isTyping = false;
    [SerializeField] bool isQuestionActive = false;
    [SerializeField] bool isFirstDialogueSaved = false;
    [SerializeField] bool skipe = false;
    Coroutine activeCoroutine;

    PlayerStateController playerStateController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        InjectDependencies(DependencyContainer.Instance);
    }
    private void Start()
    {
        skipe = false;
        InitializeUI();
    }
    private void OnEnable()
    {
        if(playerStateController != null)
        {
            playerStateController.OnDialogueNext += AdvanceDialogue;
            //playerStateController.OnDialogueSkip += SkipTyping;
        }
        DialogueManager.OnDialogueStarted += OnDialogueStartedHandler;
        DialogueManager.OnDialogueEnded += OnDialogueEndedHandler;
    }
    private void OnDisable()
    {
        if(playerStateController != null)
        {
            playerStateController.OnDialogueNext -= AdvanceDialogue;
            //playerStateController.OnDialogueSkip -= SkipTyping;
        }
        if (letterBoxFadeInOut.isVisible)
        {
            ShowletterBox(false);
        }
        DialogueManager.OnDialogueStarted -= OnDialogueStartedHandler;
        DialogueManager.OnDialogueEnded -= OnDialogueEndedHandler;
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
    public void InjectDependencies(DependencyContainer provider)
    {
        playerStateController = provider.PlayerStateController;
    }
    private void InitializeUI()
    {
        dialogueContainer.SetActive(true);
        questionContainer.SetActive(false);
    }
    private void AdvanceDialogue()
    {
        if (!isTyping && !isQuestionActive)
        {
            TextUpdate(1);
        }
    }
    public void TextUpdate(int trigger)
    {
        dialogueContainer.SetActive(true);
        questionContainer.SetActive(false);
        isQuestionActive = false;

        switch (trigger)
        {
            case 0:

                //print("Dialogo actualizado");
                name.text = Dialogue.Dialogues[localIndex].character.name;
                StopAllCoroutines();
                activeCoroutine = StartCoroutine(WriteText());

                if (Dialogue.Dialogues[localIndex].sound != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(Dialogue.Dialogues[localIndex].sound);
                }
                    break;
            case 1:

                if (!isFirstDialogueSaved)
                {
                    MainDialogue = Dialogue;
                    isFirstDialogueSaved = true;
                }

                if(localIndex < Dialogue.Dialogues.Length - 1)
                {
                    //print("Dialogo Siguiente");
                    localIndex++;
                    name.text = Dialogue.Dialogues[localIndex].character.name;
                    StopAllCoroutines();
                    activeCoroutine = StartCoroutine(WriteText());

                    if (Dialogue.Dialogues[localIndex].sound != null)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(Dialogue.Dialogues[localIndex].sound);
                    }
                }
                else
                {
                    //print("Dialogo Terminado");
                    localIndex = 0;
                    DialogueManager.actualSpeaker.DialogueLocalIndex = 0;
                    Dialogue.Finished = true;
                    OnDialogueEndedById?.Invoke(Dialogue);

                    if (Dialogue.Questions != null)
                    {
                        dialogueContainer.SetActive(false);
                        questionContainer.SetActive(true);
                        var question = Dialogue.Questions;
                        name.text = question.CharacterName.name;

                        Options[] availableOptions = System.Array.FindAll(question.Options, o => !o.Choosen /*&& !o.Hide*/);

                        DialogueManager.Instance.QuestionManager.ActivateButtons(availableOptions.Length, question.Question, availableOptions);

                        isQuestionActive = true;
                        return;
                    }
                    DialogueManager.Instance.ShowUI(false, true);
                    DialogueManager.actualSpeaker.EndDialogue();
                    //MainDialogue.ReUse = true;
                    isFirstDialogueSaved = false;

                    return;
                }
                DialogueManager.actualSpeaker.DialogueLocalIndex = localIndex;

                break;
            default:
                Debug.LogWarning("Estas pasando un valor no valido");
                break;
        }
    }
    IEnumerator WriteText()
    {
        isTyping = true;
        skipe = true;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = Dialogue.Dialogues[localIndex].dialogue;
        dialogueText.richText = true;

        var dialogueContent = Dialogue.Dialogues[localIndex].dialogue.ToCharArray();
        
        int totalCharacters = dialogueContent.Length;
        int currentCharacter = 0;

        while(currentCharacter < totalCharacters)
        {
            if (Input.GetMouseButtonDown(0) && isTyping == true && skipe == true)
            {
                if (currentCharacter > 3)
                {
                    dialogueText.maxVisibleCharacters = totalCharacters;
                    skipe = false;
                    break;
                }

            }

            dialogueText.maxVisibleCharacters++;
            if (!char.IsWhiteSpace(dialogueContent[currentCharacter]))
            {
                audioSource.PlayOneShot(typingAudioSource);
            }
            currentCharacter++;
            yield return null;
        }
        dialogueText.maxVisibleCharacters = totalCharacters;

        isTyping = false;
        skipe = false;
    }
    //private void SkipTyping()
    //{
    //    if (isTyping && skipe)
    //    {
    //        dialogueText.maxVisibleCharacters = dialogueText.text.Length;
    //        skipe = false;
    //    }
    //}
}