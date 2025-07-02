using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MindPlaceEventManagerMind : Subject
{
    [SerializeField] DialogueSOManager player;

    [SerializeField] float delay = 1f;

    public bool CorrectWordActive = false;

    [SerializeField] QuestionSO questionList;
    [SerializeField] List<GameObject> clues;
    [SerializeField] List<GameObject> textMeshPros;

    private void Start()
    {
        CheckCluesOptions(questionList, clues, textMeshPros);
        if (GameManager.Instance.TrainLoop == 1)
        {
            player.TriggerEventDialogue("MindPlace1A");
        }
        StartCoroutine(StartSceneMonologue(delay));
    }

    public void EventTriggerMonologue()
    {
        NotifyObservers(Events.TriggerMonologue);
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventTriggerMonologue();
    }
    public void CheckCluesOptions(QuestionSO question, List<GameObject> whiteboardObjects, List<GameObject> textMeshPros)
    { // cheqeo de las opciones para habilitar la pista en el mindplace
        for (int i = 0; i < question.Options.Length && i < whiteboardObjects.Count; i++)
        {
            bool shouldShow = question.Options[i].AddToWhiteboard;
            whiteboardObjects[i].SetActive(shouldShow);
            textMeshPros[i].SetActive(shouldShow);
        }
    }
}
