using System.Collections;
using UnityEngine;

public class EventDialogueManager : Subject
{
    [SerializeField] float delayMonologue = 0.1f;
    private void Start()
    {
        StartCoroutine(StartSceneMonologue(delayMonologue));
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue);
    }
}
