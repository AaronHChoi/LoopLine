using UnityEngine;
using UnityEngine.Rendering;

public class InsideMindPlace : MonoBehaviour
{
    [SerializeField] private float transitionVolumeSpeed;

    private float volumeWeight = 0f;
    private Volume volumeInsideMindPlace;

    private bool volumeIsActive = true;

    void Start()
    {
        InitializeVolume();
    }
    void Update()
    {
        UpdateVolumeWeight();
    }
    private void InitializeVolume()
    {
        GameObject volume = GameObject.Find("VolumeInsideMindPlace");
        if (volume != null)
        {
            volumeInsideMindPlace = volume.GetComponent<Volume>();
            if (volumeInsideMindPlace != null)
                volumeInsideMindPlace.weight = 1f;
        }
    }
    private void UpdateVolumeWeight()
    {
        if (volumeIsActive && volumeInsideMindPlace != null)
        {
            volumeInsideMindPlace.weight = Mathf.MoveTowards(volumeInsideMindPlace.weight, volumeWeight, Time.deltaTime * transitionVolumeSpeed);

            if (Mathf.Approximately(volumeInsideMindPlace.weight, volumeWeight))
            {
                volumeIsActive = false;
            }
        }
    }
}
