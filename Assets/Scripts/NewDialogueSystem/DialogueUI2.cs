using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI2 : MonoBehaviour
{
    public DialogueSO Dialogue;
    [SerializeField] private float textSpeed = 10;

    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private GameObject questionContainer;

    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private AudioClip typingAudioSource;
    [SerializeField] private AudioSource audioSource;
    
    public int localIndex = 1;

    bool isTyping = false;
    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.Q) && !isTyping) 
        {
            TextUpdate(1);
        }
        //
    }
    public void TextUpdate(int trigger)
    {
        dialogueContainer.SetActive(true);
        questionContainer.SetActive(false);
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
                    DialogueManager2.actualSpeaker.DialogueLocalIndex = 0;
                    Dialogue.Finished = true;

                    if(Dialogue.Questions != null)
                    {
                        dialogueContainer.SetActive(false);
                        questionContainer.SetActive(true);
                        var question = Dialogue.Questions;
                        DialogueManager2.Instance.QuestionManager.ActivateButtons(question.Options.Length, question.Question, question.Options);
                        return;
                    }
                    DialogueManager2.Instance.ShowUI(false);
                    return;
                }
                DialogueManager2.actualSpeaker.DialogueLocalIndex = localIndex;

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
        
        foreach (char letter in dialogueContent)
        {
            dialogueText.maxVisibleCharacters++;
            if (!char.IsWhiteSpace(letter)) audioSource.PlayOneShot(typingAudioSource);
            yield return new WaitForSeconds(1f / textSpeed);
        }
        isTyping = false;
    }
}
