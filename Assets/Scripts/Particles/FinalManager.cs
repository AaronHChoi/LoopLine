using DependencyInjection;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    [SerializeField] ParticleSystem twirlParticles;
    [SerializeField] Material distortion;

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

        distortion.SetFloat("_Distorsion_Strength", 0.01f);

        emissionModule.rateOverTime = 250f;
    }
}