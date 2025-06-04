using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class Word : Subject, IWord /*IInteract*/
{
    [SerializeField] public bool isCorrectWord;
    [SerializeField] private TextMeshPro tmpro;
    public string word;
    public int numerofWord;
    bool incorrectWordSelected = false;
    public bool canReactivate = true;

    [Header("Dialogue Events")]
    [SerializeField] private string correctWordEvent;
    [SerializeField] private string incorrectWordEvent;
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueSOManager dialogueSOManager;
    [SerializeField] MindPlaceEventManagerMind eventManager;
    [SerializeField] private DialogueSpeaker dialogueSpeaker;


    [Header("LookAt")]
    private PlayerController playerController;
    [SerializeField] private int range = 2;
    Vector3 _direction;
    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();     
        tmpro = GetComponentInChildren<TextMeshPro>();
        
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        eventManager = FindFirstObjectByType<MindPlaceEventManagerMind>();
        dialogueSOManager = player.GetComponent<DialogueSOManager>();
        dialogueSpeaker = player.GetComponent<DialogueSpeaker>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        if (tmpro == null)
        {
            Debug.LogError("TextMeshPro component not found in children.");
        }
    }
    public void Interacted()
    {

        if (isCorrectWord)
        {
            tmpro.color = Color.green;
            GameManager.Instance.CorrectWord101 = true;
            if(correctWordEvent != null)
            {
                dialogueSOManager.TriggerEventDialogue(correctWordEvent);
                eventManager.EventTriggerMonologue();
            }                     
        }
        else
        {
            if (incorrectWordEvent != null)
            {
                dialogueSOManager.TriggerEventDialogue(incorrectWordEvent);
                eventManager.EventTriggerMonologue();
                tmpro.color = Color.red;
                incorrectWordSelected = true;
            }
            
        }
    }
    private void Update()
    {
        tmpro.text = word + " " + "[" + numerofWord.ToString()+ "]";
        if (Vector3.Distance(transform.position, playerController.transform.position) <= range)
        {
            _direction = playerController.transform.position - transform.position;
            _direction.y = 0;

            transform.rotation = Quaternion.LookRotation(_direction);
        }
        if (incorrectWordSelected)
        {
            if (!dialogueSpeaker.isDialogueActive) { SceneManager.LoadScene("Train"); }
        }
    }

}