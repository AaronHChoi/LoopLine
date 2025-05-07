using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WordManager : MonoBehaviour
{
    [Header("Lista de string de las palabras")]
    [SerializeField] private List<string> correctWords = new List<string>();
    [SerializeField] private List<string> incorrectWords = new List<string>();

    [Header("Lista de objetos de las palabras")]
    [SerializeField] private List<Word> correctWordsObject = new List<Word>();
    [SerializeField] private List<Word> incorrectWordsObject = new List<Word>();

    private List<Word> words = new List<Word>();
    private float timer = 0f;

    [Header("LookAt")]
    private PlayerController playerController;
    [SerializeField] private int range = 6;
    Vector3 _direction;

    private Dictionary<int, Word> numberToObject = new Dictionary<int, Word>();

    [SerializeField] private Animator animator;
    [SerializeField] private bool isPlayerInRange = false;
    [SerializeField] private float distance;
    [SerializeField] private float delay = 1.6f;

    //Test
    MindPlaceEventManagerMind eventManager;
    bool onlyOneTime;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
        eventManager = FindFirstObjectByType<MindPlaceEventManagerMind>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onlyOneTime = false;
        for (int i = 0; i < incorrectWordsObject.Count; i++)
        {
            int randomIndex = Random.Range(0, incorrectWords.Count - 1);
            incorrectWordsObject[i].word = incorrectWords[randomIndex];
            words.Add(incorrectWordsObject[i]);
            incorrectWords.RemoveAt(randomIndex);
        }

        for (int i = 0; i < correctWordsObject.Count; i++)
        {
            int randomIndex = Random.Range(0, incorrectWords.Count - 1);
            correctWordsObject[i].word = correctWords[randomIndex];
            words.Add(correctWordsObject[i]);
            correctWords.RemoveAt(randomIndex);
        }
        AssignNumbers();
        ActiveAndDeactivateWords(false);
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, playerController.transform.position);

        if (distance <= range)
        {
            if(GameManager.Instance.Loop == 1 && !onlyOneTime)
            {
                eventManager.EventTriggerMonologue();
                onlyOneTime = true;
            }
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                SpawnWords();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
                CheckNumber(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                CheckNumber(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                CheckNumber(3);
        }
        else
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                UnSpawnWords();
            }
        }
    }
    private void AssignNumbers()
    {
        List<int> numbers = new List<int> { 1, 2, 3 };
        Shuffle(numbers);

        for (int i = 0; i < words.Count; i++)
        {
            numberToObject[numbers[i]] = words[i];
            words[i].numerofWord = numbers[i];
        }
    }
    private void CheckNumber(int numberPressed)
    {
        if (numberToObject.ContainsKey(numberPressed))
        {
            Word word = numberToObject[numberPressed];
            word.Interacted();
        }
    }
    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void SpawnWords()
    {
        ActiveAndDeactivateWords(true);
        animator.SetTrigger("EnterAnim");
    }
    private void UnSpawnWords()
    {
        animator.SetTrigger("ExitAnim");
        StartCoroutine(DisableWordsWithDelay(delay));
    }
    private IEnumerator DisableWordsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActiveAndDeactivateWords(false);
    }
    private void ActiveAndDeactivateWords(bool state)
    {
        foreach (Word word in words)
        {
            word.gameObject.SetActive(state);
        }
    }
}