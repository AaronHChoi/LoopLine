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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < incorrectWordsObject.Count; i++)
        {
            incorrectWordsObject[i].word = incorrectWords[Random.Range(0, incorrectWords.Count -1)];
        }

        for (int i = 0; i < correctWordsObject.Count; i++)
        {
            correctWordsObject[i].word = correctWords[Random.Range(0, correctWords.Count - 1)];
        }
    }
}
