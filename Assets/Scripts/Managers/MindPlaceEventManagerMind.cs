using System.Collections;
using UnityEngine;

public class MindPlaceEventManagerMind : Subject
{
    [SerializeField] DialogueSOManager player;

    [SerializeField] float delay = 1f;

    public bool CorrectWordActive = false;

    private void Start()
    {
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
}
