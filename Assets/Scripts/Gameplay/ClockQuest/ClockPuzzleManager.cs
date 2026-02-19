using System;
using UnityEngine;
using UnityEngine.Video;
using Player;
using Core.Utilities;
using Core.Audio;
using Core.DependencyInjection;
using Core.Data;
using Core.UI;

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

    [SerializeField] VideoClip successCinematic;

    [SerializeField] ParticleCinematicController particleController;

    public event Action OnClockQuestFinished;

    IMonologueSpeaker monologueSpeaker;
    IFinalQuestManager finalQuestManager;
    IPlayerStateController playerStateController;
    IGearRotator gearRotator;
    IClock clockLock;
    ICinematicManager cinematicManager;
    IUIManager uiManager;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
        finalQuestManager = InterfaceDependencyInjector.Instance.Resolve<IFinalQuestManager>();
        gearRotator = InterfaceDependencyInjector.Instance.Resolve<IGearRotator>();
        clockLock = InterfaceDependencyInjector.Instance.Resolve<IClock>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        cinematicManager = InterfaceDependencyInjector.Instance.Resolve<ICinematicManager>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            clockLock.SetLockState(true);
            gearRotator.StopGears();
        }
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
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameManager.Instance.SetCondition(GameCondition.WordGroup1, true);
        }
    }
#endif
    public void CheckTime()
    {
        string currentTime = clock.GetClockTime();
        string[] timeParts = currentTime.Split(':');

        int currenHour = int.Parse(timeParts[0]);
        int currentMinute = int.Parse(timeParts[1]);

        if (currenHour == targetHour && currentMinute == targetMinute && !firstTime)
        {
            firstTime = true;

            clockLock.SetLockState(true);
            gearRotator.StopGears();

            monologueSpeaker.OnMonologueEnded += StartCinematicAfterMonologue;

            monologueSpeaker.StartMonologue(Events.BeforeCompleteClockQuest);
        }
    } 
    void StartCinematicAfterMonologue(Events finishedEvent)
    {
        monologueSpeaker.OnMonologueEnded -= StartCinematicAfterMonologue;

        playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);

        cinematicManager.PlayCinematic(successCinematic, () =>
        {
            RevealObject();

            soundManager.CreateSound()
                .WithSoundData(complete)
                .WithSoundPosition(transform.position)
                .Play();

            playerStateController.StateMachine.TransitionTo(playerStateController.NormalState);

            OnClockQuestFinished?.Invoke();

            uiManager.ShowPanel(UIPanelID.InventoryTutorial);

            monologueSpeaker.StartMonologue(Events.ClockQuestComplete);
        });
    }
    private void RevealObject()
    {
        DelayUtility.Instance.Delay(2f, () => Key.Interact());

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