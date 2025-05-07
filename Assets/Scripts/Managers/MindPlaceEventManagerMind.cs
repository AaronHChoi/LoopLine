using System.Collections;
using UnityEngine;

public class MindPlaceEventManagerMind : Subject
{
    [SerializeField] DialogueSOManager player;

    [SerializeField] float delay = 1f;
    private void Start()
    {
        if (GameManager.Instance.Loop == 1)
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
        NotifyObservers(Events.TriggerMonologue);
    }
}
