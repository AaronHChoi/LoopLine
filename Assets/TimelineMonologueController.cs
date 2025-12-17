using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineMonologueController : MonoBehaviour
{
    [SerializeField] List<Events> TimeLineEvents;
    MonologueSpeaker monologueSpeaker;
    PlayableDirector playableDirector;
    int eventIndex = 0;

    private void Start()
    {
        monologueSpeaker = GetComponent<MonologueSpeaker>();
        playableDirector = GetComponent<PlayableDirector>();

        if(GameManager.Instance.TrainLoop == 0) playableDirector.Play();
    }
    public void StartMonologueIncrement()
    {
        if (TimeLineEvents != null && TimeLineEvents.Count > 1)
        {
            monologueSpeaker.StartMonologue(TimeLineEvents[eventIndex]);
            eventIndex = TimeLineEvents.Count <= eventIndex + 1 ? 0 : eventIndex + 1;
        }
    }
}
