using DependencyInjection;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineMonologueController : MonoBehaviour
{
    [SerializeField] List<Events> TimeLineEvents;
    [SerializeField] UIPanelID panelID;

    MonologueSpeaker monologueSpeaker;
    PlayableDirector playableDirector;
    int eventIndex = 0;
    IPlayerStateController playerStateController;
    IUIManager uiManager;

    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    private void Start()
    {
        monologueSpeaker = GetComponent<MonologueSpeaker>();
        playableDirector = GetComponent<PlayableDirector>();

        if (GameManager.Instance.TrainLoop == 0)
        {
            playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);
            playableDirector.Play();
        }
    }
    public void StartMonologueIncrement()
    {
        if (TimeLineEvents != null && TimeLineEvents.Count > 1)
        {
            monologueSpeaker.StartMonologue(TimeLineEvents[eventIndex]);
            eventIndex = TimeLineEvents.Count <= eventIndex + 1 ? 0 : eventIndex + 1;
        }
    }
    public void ResumePlayerActions()
    {
        playerStateController.StateMachine.TransitionTo(playerStateController.NormalState);
    }
    public void ChangeCinematicMonologueState(bool isCinematic)
    {
        GameManager.Instance.isCinematicMonologue = isCinematic;
    }
    public void ShowTooltipPanel()
    {
        uiManager.ShowPanel(panelID);
    }
}
