using DependencyInjection;
using UnityEngine;

public class MindplaceManager : MonoBehaviour
{
    [SerializeField] GameObject doorLightPhotoQuest;

    IClockPuzzleManager clockPuzzleManager;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
    }
    private void Start()
    {
        doorLightPhotoQuest.SetActive(false);

        if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            doorLightPhotoQuest.SetActive(true);
        }
    }
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