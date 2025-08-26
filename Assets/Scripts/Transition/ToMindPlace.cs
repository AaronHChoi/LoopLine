using UnityEngine;
using UnityEngine.Rendering;

public class ToMindPlace : MonoBehaviour
{
    [SerializeField] private float transitionVolumeSpeed = 2f;
    [SerializeField] private float timeExplosionRemaining;
    private float volumeWeight = 1f;
    private Volume volumeExplosion;

    private bool isVolumeActive = false;

    ITimeProvider timeProvider;

    private void Awake()
    {
        timeProvider = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
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
        if (timeProvider != null && !isVolumeActive && timeProvider.LoopTime <= timeExplosionRemaining)
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