using UnityEngine;
using UnityEngine.Rendering;
using Unity.Cinemachine;
using DependencyInjection;

public class ToMindPlace : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float transitionVolumeSpeed = 2f;
    [SerializeField] private float fovSpeed = 60f;
    [SerializeField] private float startFOV = 60f;
    [SerializeField] private float endFOV = 120f;

    [Header("Scene References")]
    [SerializeField] private Volume volumeExplosion;
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private ITimeProvider timeProvider;
    private bool isActive = false;
    private bool isReverse = false;
    private float targetWeight = 1f;
    private float currentFOV;

    private void Awake()
    {
        timeProvider = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
    }

    private void Start()
    {
        if (volumeExplosion)
            volumeExplosion.weight = 1f;

        if (mainCamera)
        {
            currentFOV = startFOV;
            mainCamera.Lens.FieldOfView = currentFOV;
        }

        // reverse entry fade
        ReverseTransition();
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

    public void ReverseTransition()
    {
        isActive = true;
        isReverse = true;
        targetWeight = 0f;
    }

    private void UpdateTransition()
    {
        if (!isActive || volumeExplosion == null || mainCamera == null)
            return;

        volumeExplosion.weight = Mathf.MoveTowards(
            volumeExplosion.weight, targetWeight, Time.deltaTime * transitionVolumeSpeed);

        float targetFov = isReverse ? startFOV : endFOV;
        currentFOV = Mathf.MoveTowards(currentFOV, targetFov, Time.deltaTime * fovSpeed);
        mainCamera.Lens.FieldOfView = currentFOV;

        if (Mathf.Approximately(volumeExplosion.weight, targetWeight))
            isActive = false;
    }
}
