using UnityEngine;
using UnityEngine.Rendering;

public class TransitionToMindPlace : MonoBehaviour
{
    private TimeManager timeManager;

    [SerializeField] private float transitionVolumeSpeed = 2f;
    [SerializeField] private float timeExplosionRemaining;
    private float volumeWeight = 1f;
    private Volume volumeExplosion;

    private bool volumeIsActive = false;

    void Start()
    {
        timeManager = FindFirstObjectByType<TimeManager>();

        GameObject volume = GameObject.Find("VolumeExplosion");
        if (volume != null)
        {
            volumeExplosion = volume.GetComponent<Volume>();
            if (volumeExplosion != null)
                volumeExplosion.weight = 0f;
        }
    }

    void Update()
    {
        if (timeManager != null && !volumeIsActive && timeManager.LoopTime <= timeExplosionRemaining)
        {
            volumeIsActive = true;
        }

        if (volumeIsActive && volumeExplosion != null)
        {
            volumeExplosion.weight = Mathf.Lerp(volumeExplosion.weight, volumeWeight, Time.deltaTime * transitionVolumeSpeed);

            if (Mathf.Abs(volumeExplosion.weight - volumeWeight) < 0.01f)
                volumeExplosion.weight = volumeWeight;
        }
    }
}
