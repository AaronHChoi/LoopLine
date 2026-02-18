using Core.Data;
using Core.DependencyInjection;
using Core.UI;
using Core.Utilities;
using UnityEngine;

public class MindplaceManager : MonoBehaviour
{
    [SerializeField] GenericLightController photoLightController;
    [SerializeField] GenericLightController musicLightController;

    IClockPuzzleManager clockPuzzleManager;
    IPhotoQuestManager photoQuestManager;
    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            photoLightController.SetLight(true);
        }
        if (GameManager.Instance.GetCondition(GameCondition.IsPhotoQuestComplete))
        {
            musicLightController.SetLight(true);
        }
        if (!GameManager.Instance.GetCondition(GameCondition.FirstTimeInMindPlace))
        {
            GameManager.Instance.SetCondition(GameCondition.FirstTimeInMindPlace, true);

            DelayUtility.Instance.Delay(1f, () => monologueSpeaker.StartMonologue(Events.Mindplace1));
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.Instance.SetCondition(GameCondition.PhotoDoorOpen, true);
            //GameManager.Instance.SetCondition(GameCondition.MusicSafeDoorOpen, true);
        }
    }
#endif
    private void OnEnable()
    {
        clockPuzzleManager.OnClockQuestFinished += ClockQuestCompleted;
        photoQuestManager.OnPhotoQuestFinished += PhotoQuestCompleted;
    }
    private void OnDisable()
    {
        clockPuzzleManager.OnClockQuestFinished -= ClockQuestCompleted;
        photoQuestManager.OnPhotoQuestFinished -= PhotoQuestCompleted;
    }
    private void ClockQuestCompleted()
    {
        photoLightController.SetLight(true);
    }
    void PhotoQuestCompleted()
    {
        musicLightController.SetLight(true);
    }
}