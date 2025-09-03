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

    [SerializeField] private List<Word> words = new List<Word>();

    [Header("LookAt")]
    private PlayerController playerController;
    [SerializeField] private float range = 3f;
    Vector3 _direction;

    private Dictionary<int, Word> numberToObject = new Dictionary<int, Word>();

    [SerializeField] private Animator animator;
    [SerializeField] private bool isPlayerInRange = false;
    [SerializeField] private float distance;
    [SerializeField] private float delay = 1.6f;
    private int randomWordToCorrectIndex;
    //Test
    bool onlyOneTime;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    void Start()
    {
        randomWordToCorrectIndex = Random.Range(0, words.Count - 1);
        for (int i = 0; i < words.Count; i++)
        {       
            if (i == randomWordToCorrectIndex)
            {
                correctWordsObject.Add(words[i]);
                words[i].isCorrectWord = true;
            }
            else
            {
                incorrectWordsObject.Add(words[i]);
            }
        }
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
        bool nowInRange = distance <= range;

        if(nowInRange != isPlayerInRange)
        {
            isPlayerInRange = nowInRange;

            if (isPlayerInRange)
            {
                if (GameManager.Instance.TrainLoop == 1 && !onlyOneTime)
                {
                    onlyOneTime = true;
                }
                StartEnterSequence();
            }
            else
            {
                StartExitSequence();
            }
        }

        if(isPlayerInRange && words.Count > 0 && words[0].gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                CheckNumber(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                CheckNumber(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                CheckNumber(3);
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
            numberToObject.Remove(numberPressed);
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].numerofWord == numberPressed)
                {
                    words.RemoveAt(i);
                }
                else
                {
                    words[i].gameObject.SetActive(false);
                    words[i].canReactivate = false;
                }
            }
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
    private void StartEnterSequence()
    {
        StopAllCoroutines();

        ActiveAndDeactivateWords(true);
        animator.SetTrigger("EnterAnim");
    }
    private void StartExitSequence()
    {
        StopAllCoroutines();

        animator.SetTrigger("ExitAnim");
        StartCoroutine(DisableWordsWithDelay(delay));
    }
    private IEnumerator DisableWordsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isPlayerInRange)
            ActiveAndDeactivateWords(false);
    }
    private void ActiveAndDeactivateWords(bool state)
    {
        foreach (Word word in words)
        {
            if (word.canReactivate)
            {
                word.gameObject.SetActive(state);
            }
            else
            {
                word.gameObject.SetActive(false);
            }
        }
    }
}