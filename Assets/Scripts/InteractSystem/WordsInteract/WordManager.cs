using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
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
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) <= range)
        {
            SpawnWords();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                CheckNumber(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                CheckNumber(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                CheckNumber(3);
        }
        else
        {
            UnSpawnWords();
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
        foreach (Word word in words)
        {
           word.gameObject.SetActive(true);                               
        }
    }

    private void UnSpawnWords()
    {
        foreach (Word word in words)
        {
            word.gameObject.SetActive(false);
        }
    }

}
