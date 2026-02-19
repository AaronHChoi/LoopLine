using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using Core.UI;
using UnityEngine;

public class MonologueController : MonoBehaviour
{
    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
    }
    private void OnEnable()
    {
        EventBus.Subscribe<ClockSyncEvent>(StartMonologueStopTrain);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ClockSyncEvent>(StartMonologueStopTrain);
    }
    public void StartMonologueStopTrain(ClockSyncEvent ev)
    {
        if (GameManager.Instance.GetCondition(GameCondition.StopTrainButton))
        {
            monologueSpeaker.StartMonologue(Events.MonologueStopTrainButton);
            GameManager.Instance.SetCondition(GameCondition.StopTrainButton, true);
        }
    }
}