using DependencyInjection;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    [SerializeField] ParticleSystem twirlParticles;
    [SerializeField] Material distortion;

    IClockPuzzleManager clockPuzzleManager;
    IPhotoQuestManager photoQuestManager;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
    }
    private void Start()
    {
        if (!GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            distortion.SetFloat("_Distorsion_Strength", 0.05f);

            var emissionModule = twirlParticles.emission;
            emissionModule.rateOverTime = 1000f;
        } 
        else if (GameManager.Instance.GetCondition(GameCondition.IsClockQuestComplete))
        {
            distortion.SetFloat("_Distorsion_Strength", 0.01f);

            var emissionModule = twirlParticles.emission;
            emissionModule.rateOverTime = 250f;
        }
    }
    private void OnEnable()
    {
        clockPuzzleManager.OnClockQuestFinished += FirstQuestComplete;
        photoQuestManager.OnPhotoQuestFinished += SecondQuestComplete;
    }
    private void OnDisable()
    {
        clockPuzzleManager.OnClockQuestFinished -= FirstQuestComplete;
        photoQuestManager.OnPhotoQuestFinished -= SecondQuestComplete;
    }
    private void FirstQuestComplete()
    {
        var emissionModule = twirlParticles.emission;

        distortion.SetFloat("_Distorsion_Strength", 0.01f);

        emissionModule.rateOverTime = 250f;
    }
    private void SecondQuestComplete()
    {
        var emissionModule = twirlParticles.emission;

        distortion.SetFloat("_Distorsion_Strength", 0f);

        emissionModule.rateOverTime = 0f;
    }
}