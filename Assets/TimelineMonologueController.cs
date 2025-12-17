using System.Collections.Generic;
using UnityEngine;

public class TimelineMonologueController : MonoBehaviour
{
    [SerializeField] List<Events> TimeLineEvents;
    MonologueSpeaker monologueSpeaker;
    int eventIndex = 0;

    private void Start()
    {
        monologueSpeaker = GetComponent<MonologueSpeaker>();
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
