using DependencyInjection;
using UnityEngine;

public class MindplaceManager : MonoBehaviour
{
    [SerializeField] GameObject doorLightPhotoQuest;

    IClockPuzzleManager clockPuzzleManager;
    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void Start()
    {
        doorLightPhotoQuest.SetActive(false);

        if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            doorLightPhotoQuest.SetActive(true);
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
    }
    private void OnDisable()
    {
        clockPuzzleManager.OnClockQuestFinished -= ClockQuestCompleted;
    }
    private void ClockQuestCompleted()
    {
        doorLightPhotoQuest.SetActive(true);
    }
}