using UnityEngine;
using Core.EventBus;
using Core.Utilities;
using Core.DependencyInjection;
using Core.Data;

public class LoopManager : MonoBehaviour
{
    IGameSceneManager gameSceneManager;
    IUIManager uiManager;
    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<LoopTeleportEvent>(HandleLoopTransition);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<LoopTeleportEvent>(HandleLoopTransition);
    }
    void HandleLoopTransition(LoopTeleportEvent ev)
    {
        if (!GameManager.Instance.GetCondition(GameCondition.AllNpcsLockedSpoken))
        {
            gameSceneManager.UnloadLastScene();
            gameSceneManager.LoadSceneAsync2("AS_NPC_LOCKED");
            return;
        }

        GameManager.Instance.TrainLoop += 1;

        bool isInitialLoop = gameSceneManager.GetIsInInitialLoop();
        bool firstLoopsCompleted = GameManager.Instance.GetCondition(GameCondition.IsFirstLoopsCompleted);

        if (!isInitialLoop && firstLoopsCompleted)
        {
            LoadRandomNextScene();
        } 
        else if(!isInitialLoop && !firstLoopsCompleted)
        {
            HandleStorySequence();
        }
    }
    void HandleStorySequence()
    {
        gameSceneManager.UnloadLastScene();

        switch (GameManager.Instance.TrainLoop)
        {
            case 1:
                {
                    gameSceneManager.LoadSceneAsync2("AS_NPC_PROLOGUE");
                }
                break;
            case 2:
                {
                    gameSceneManager.LoadSceneAsync2("AS_Clocks");
                    DelayUtility.Instance.Delay(1.5f, () => GameManager.Instance.SetCondition(GameCondition.Chapter1, true));
                }
                break;
            case 3:
                {
                    gameSceneManager.LoadSceneAsync2("AS_NPC");
                    CompleteFirstLoopSequence();
                }
                break;
            //case 4:
            //    {
            //        CompleteFirstLoopSequence();
            //    }
            //    break;
            default:
                {
                    gameSceneManager.LoadRandomScene();
                }
                break;
        }
    }
    void CompleteFirstLoopSequence()
    {
        monologueSpeaker.OnMonologueEnded += OnTutorialMonologueEnded;
        DelayUtility.Instance.Delay(1f, () => monologueSpeaker.StartMonologue(Events.TutorialTeleport));
       
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
        GameManager.Instance.SetCondition(GameCondition.IsFirstLoopsCompleted, true);
        //gameSceneManager.LoadRandomScene();
    }
    void OnTutorialMonologueEnded(Events finishedEvent)
    {
        if (finishedEvent == Events.TutorialTeleport)
        {
            monologueSpeaker.OnMonologueEnded -= OnTutorialMonologueEnded;
            uiManager.ShowPanel(UIPanelID.TeleportTutorial);
        }
    }
    void LoadRandomNextScene()
    {
        gameSceneManager.UnloadLastScene();
        gameSceneManager.LoadRandomScene();
    }
}