using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour, IDependencyInjectable
{
    public DialogueSO Dialogue;
    public DialogueSO MainDialogue;
    [SerializeField] private float textSpeed = 10;

    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private GameObject questionContainer;

    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private AudioClip typingAudioSource;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private RawImage dialogueBackground;
    
    public int localIndex = 1;

    [SerializeField] bool isTyping = false;
    [SerializeField] bool isQuestionActive = false;
    [SerializeField] bool isFirstDialogueSaved = false;
    Coroutine activeCoroutine;

    TimeManager timeManager;
    private void Awake()
    {
        InjectDependencies(
            DependencyContainer.Instance);
        audioSource = GetComponent<AudioSource>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        timeManager = provider.TimeManager;
    }
    private void Start()
    {
        InitializeUI();
    }
    private void Update()
    {
        HandleInput();
    }
    private void InitializeUI()
    {
        dialogueContainer.SetActive(true);
        questionContainer.SetActive(false);
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isTyping && !isQuestionActive)
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

                print("Dialogo actualizado");
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
                    print("Dialogo Siguiente");
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
                    print("Dialogo Terminado");
                    localIndex = 0;
                    DialogueManager.actualSpeaker.DialogueLocalIndex = 0;
                    Dialogue.Finished = true;

                    if(Dialogue.Questions != null)
                    {
                        //dialogueContainer.SetActive(false);
                        questionContainer.SetActive(true);
                        var question = Dialogue.Questions;
                        name.text = question.CharacterName.name;
                        DialogueManager.Instance.QuestionManager.ActivateButtons(question.Options.Length, question.Question, question.Options);

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
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = Dialogue.Dialogues[localIndex].dialogue;
        dialogueText.richText = true;

        var dialogueContent = Dialogue.Dialogues[localIndex].dialogue.ToCharArray();
        
        int totalCharacters = dialogueContent.Length;
        int currentCharacter = 0;

        while(currentCharacter < totalCharacters)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                dialogueText.maxVisibleCharacters = totalCharacters;
                break;
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
        timeManager.SkipDialogue();
    }
}