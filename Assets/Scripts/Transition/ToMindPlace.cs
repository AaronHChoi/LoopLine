using UnityEngine;
using UnityEngine.Rendering;

public class ToMindPlace : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private float transitionVolumeSpeed = 2f;
    [SerializeField] private float timeExplosionRemaining;
    private float volumeWeight = 1f;
    private Volume volumeExplosion;

    private bool isVolumeActive = false;
    TimeManager timeManager;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        timeManager = provider.TimeManager;
    }
    void Start()
    {
        InitializeVolume();
    }
    void Update()
    {
        CheckActivationCondition();
        UpdateVolumeTransition();
    }
    private void InitializeVolume()
    {
        GameObject volume = GameObject.Find("VolumeExplosion");
        if (volume != null)
        {
            volumeExplosion = volume.GetComponent<Volume>();
            if (volumeExplosion != null)
                volumeExplosion.weight = 0f;
        }
    }
    private void CheckActivationCondition()
    {
        if (timeManager != null && !isVolumeActive && timeManager.LoopTime <= timeExplosionRemaining)
        {
            isVolumeActive = true;
        }
    }
    private void UpdateVolumeTransition()
    {
        if(!isVolumeActive || volumeExplosion == null)
        {
            return;
        }

        volumeExplosion.weight = Mathf.Lerp(volumeExplosion.weight, volumeWeight, Time.deltaTime * transitionVolumeSpeed);

        if (Mathf.Abs(volumeExplosion.weight - volumeWeight) < 0.01f)
            volumeExplosion.weight = volumeWeight;
    }
}