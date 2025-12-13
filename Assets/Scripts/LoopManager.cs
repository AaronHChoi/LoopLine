using DependencyInjection;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    IGameSceneManager gameSceneManager;
    IPhotoCapture polaroid;
    IUIManager uiManager;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        polaroid = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
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
        GameManager.Instance.TrainLoop += 1;
        polaroid.ResetPhotoCounter();

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
            case 0:
            case 1:
                {
                    gameSceneManager.LoadSceneAsync2("AS_NPC");
                }
                break;
            case 2:
                {
                    gameSceneManager.LoadSceneAsync2("AS_NoNPC-NoItems");
                }
                break;
            case 3:
                {
                    gameSceneManager.LoadSceneAsync2("AS_Clocks");
                }
                break;
            case 4:
                {
                    CompleteFirstLoopSequence();
                }
                break;
            default:
                {
                    gameSceneManager.LoadRandomScene();
                }
                break;
        }
    }
    void CompleteFirstLoopSequence()
    {
        uiManager.ShowPanel(UIPanelID.TeleportTutorial);
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
        GameManager.Instance.SetCondition(GameCondition.IsFirstLoopsCompleted, true);
        gameSceneManager.LoadRandomScene();
    }
    void LoadRandomNextScene()
    {
        gameSceneManager.UnloadLastScene();
        gameSceneManager.LoadRandomScene();
    }
}