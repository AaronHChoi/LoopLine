using DependencyInjection;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    [SerializeField] ParticleSystem twirlParticles;

    IClockPuzzleManager clockPuzzleManager;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
    }
    private void OnEnable()
    {
        clockPuzzleManager.OnPhotoQuestFinished += FirstQuestComplete;
    }
    private void OnDisable()
    {
        clockPuzzleManager.OnPhotoQuestFinished -= FirstQuestComplete;
    }
    private void FirstQuestComplete()
    {
        var emissionModule = twirlParticles.emission;

        emissionModule.rateOverTime = 250f;
    }
}