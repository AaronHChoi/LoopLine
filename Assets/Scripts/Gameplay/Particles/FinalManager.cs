using DependencyInjection;
using Unity.VisualScripting;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    [SerializeField] ParticleSystem twirlParticles;
    [SerializeField] Material distortion;
    [SerializeField] GameObject finalEffectObject;

    IClockPuzzleManager clockPuzzleManager;
    IPhotoQuestManager photoQuestManager;
    ISafeQuestManager safeQuestManager;
    IFinalQuestManager finalQuestManager;

    private void Awake()
    {
        clockPuzzleManager = InterfaceDependencyInjector.Instance.Resolve<IClockPuzzleManager>();
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
        safeQuestManager = InterfaceDependencyInjector.Instance.Resolve<ISafeQuestManager>();
        finalQuestManager = InterfaceDependencyInjector.Instance.Resolve<IFinalQuestManager>();
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
        safeQuestManager.OnSafeQuestCompleted += ThirdQuestCompleted;
        finalQuestManager.OnQuestCompleted += LastQuestCompleted;
    }
    private void OnDisable()
    {
        clockPuzzleManager.OnClockQuestFinished -= FirstQuestComplete;
        photoQuestManager.OnPhotoQuestFinished -= SecondQuestComplete;
        safeQuestManager.OnSafeQuestCompleted -= ThirdQuestCompleted;
        finalQuestManager.OnQuestCompleted -= LastQuestCompleted;
    }
    private void FirstQuestComplete()
    {
        var emissionModule = twirlParticles.emission;

        distortion.SetFloat("_Distorsion_Strength", 0.02f);

        emissionModule.rateOverTime = 250f;
    }
    private void SecondQuestComplete()
    {
        var emissionModule = twirlParticles.emission;

        distortion.SetFloat("_Distorsion_Strength", 0.01f);

        emissionModule.rateOverTime = 150f;
    }

    private void ThirdQuestCompleted()
    {
        var emissionModule = twirlParticles.emission;
        distortion.SetFloat("_Distorsion_Strength", 0f);
        emissionModule.rateOverTime = 0f;
    }

    private void LastQuestCompleted()
    {
        finalEffectObject.gameObject.SetActive(false);
    }
}