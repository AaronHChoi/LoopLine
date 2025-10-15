using UnityEngine;
using UnityEngine.Rendering;
using Unity.Cinemachine;

public class InsideMindPlace : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float transitionVolumeSpeed = 2f;
    [SerializeField] private float fovSpeed = 60f;
    [SerializeField] private float startFOV = 120f;
    [SerializeField] private float endFOV = 60f;

    [Header("Scene References")]
    [SerializeField] private Volume volumeInsideMindPlace;
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private bool isActive = false;
    private bool isReverse = false;
    private float targetWeight = 0f;
    private float currentFOV;

    private void Start()
    {
        if (volumeInsideMindPlace)
            volumeInsideMindPlace.weight = 1f;

        if (mainCamera)
        {
            currentFOV = startFOV;
            mainCamera.Lens.FieldOfView = currentFOV;
        }

        // fade in from black
        StartReverseTransition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Activate();

        UpdateTransition();
    }

    public void Activate()
    {
        if (isActive) return;
        isActive = true;
        isReverse = false;
        targetWeight = 1f;

        if (impulseSource)
            impulseSource.GenerateImpulse();
    }

    public void StartReverseTransition()
    {
        isActive = true;
        isReverse = true;
        targetWeight = 0f;
    }

    private void UpdateTransition()
    {
        if (!isActive || volumeInsideMindPlace == null || mainCamera == null)
            return;

        volumeInsideMindPlace.weight = Mathf.MoveTowards(
            volumeInsideMindPlace.weight, targetWeight, Time.deltaTime * transitionVolumeSpeed);

        float targetFov = isReverse ? endFOV : startFOV;
        currentFOV = Mathf.MoveTowards(currentFOV, targetFov, Time.deltaTime * fovSpeed);
        mainCamera.Lens.FieldOfView = currentFOV;

        if (Mathf.Approximately(volumeInsideMindPlace.weight, targetWeight))
            isActive = false;
    }
}
