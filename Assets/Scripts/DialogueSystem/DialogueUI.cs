using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
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
    [SerializeField] TimeManager timeManager;

    public int localIndex = 1;

    [SerializeField] bool isTyping = false;
    [SerializeField] bool isQuestionActive = false;
    [SerializeField] bool isFirstDialogueSaved = false;

    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        dialogueContainer.SetActive(true);
        questionContainer.SetActive(false);
    }
    private void Update()
    {
        //
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isTyping && !isQuestionActive) 
        {
            TextUpdate(1);
        }
        //
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
                StartCoroutine(WriteText());

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
                    StartCoroutine(WriteText());

                    if (Dialogue.Dialogues[localIndex].sound != null)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(Dialogue.Dialogues[localIndex].sound);
                    }
                }
                else
                {
                    print("Dialogo Termiando");
                    localIndex = 0;
                    DialogueManager.actualSpeaker.DialogueLocalIndex = 0;
                    //Dialogue.Finished = true;

                    if(Dialogue.Questions != null)
                    {
                        dialogueContainer.SetActive(false);
                        questionContainer.SetActive(true);
                        var question = Dialogue.Questions;
                        name.text = question.CharacterName.name;
                        DialogueManager.Instance.QuestionManager.ActivateButtons(question.Options.Length, question.Question, question.Options);

                        isQuestionActive = true;
                        return;
                    }
                    DialogueManager.Instance.ShowUI(false);
                    DialogueManager.actualSpeaker.EndDialogue();
                    MainDialogue.ReUse = true;
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
        timeManager.AllowFastForwardMethod(true);
        isTyping = true;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = Dialogue.Dialogues[localIndex].dialogue;
        dialogueText.richText = true;

        var dialogueContent = Dialogue.Dialogues[localIndex].dialogue.ToCharArray();
        
        foreach (char letter in dialogueContent)
        {
            dialogueText.maxVisibleCharacters++;
            if (!char.IsWhiteSpace(letter)) audioSource.PlayOneShot(typingAudioSource);
            yield return new WaitForSeconds(1f / textSpeed);
        }
        isTyping = false;
        timeManager.AllowFastForwardMethod(false);
    }
}
