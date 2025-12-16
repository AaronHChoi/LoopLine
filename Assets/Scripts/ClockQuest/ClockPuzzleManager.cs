using DependencyInjection;
using Player;
using System;
using UnityEngine;

public class ClockPuzzleManager : MonoBehaviour, IClockPuzzleManager
{
    [SerializeField] Clock clock;

    [SerializeField] int targetHour;
    [SerializeField] int targetMinute;
    [SerializeField] SoundData complete;
    [SerializeField] SingleDoorInteract doorInteract;

    bool firstTime = false;

    [SerializeField] Events QuestCompleteEvent;
    [SerializeField] TutorialInteract Key;

    public event Action OnClockQuestFinished;
    IMonologueSpeaker monologueSpeaker;
    IFinalQuestManager finalQuestManager;
    IPlayerStateController playerStateController;
    IGearRotator gearRotator;
    IClock clockLock;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        finalQuestManager = InterfaceDependencyInjector.Instance.Resolve<IFinalQuestManager>();
        gearRotator = InterfaceDependencyInjector.Instance.Resolve<IGearRotator>();
        clockLock = InterfaceDependencyInjector.Instance.Resolve<IClock>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void OnEnable()
    {
        if(clock != null)
        {
            clock.OnCheckTime += CheckTime;
        }
    }
    private void OnDisable()
    {
        if (clock != null)
        {
            clock.OnCheckTime -= CheckTime;
        }
    }
    public void CheckTime()
    {
        string currentTime = clock.GetClockTime();
        string[] timeParts = currentTime.Split(':');

        int currenHour = int.Parse(timeParts[0]);
        int currentMinute = int.Parse(timeParts[1]);

        if (currenHour == targetHour && currentMinute == targetMinute && !firstTime)
        {
            DelayUtility.Instance.Delay(3f, () => monologueSpeaker.StartMonologue(QuestCompleteEvent));

            RevealObject();

            SoundManager.Instance.CreateSound()
                .WithSoundData(complete)
                .WithSoundPosition(transform.position)
                .Play();

            firstTime = true;

            playerStateController.StateMachine.TransitionTo(playerStateController.NormalState);

            gearRotator.StopGears();
            OnClockQuestFinished?.Invoke();
            clockLock.SetLockState(true);
        }
    }
    private void RevealObject()
    {
        DelayUtility.Instance.Delay(2f, () => Key.Interact());
        GameManager.Instance.SetCondition(GameCondition.WordGroup1, true);
        //finalQuestManager.UpdateWordsActivation();
        GameManager.Instance.SetCondition(GameCondition.IsClockQuestComplete, true);
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, false);
    }
    public void OpenDoorClockQuest()
    {
        GameManager.Instance.SetCondition(GameCondition.ClockDoorOpen, true);
    }
}
public interface IClockPuzzleManager
{
    event Action OnClockQuestFinished;
}