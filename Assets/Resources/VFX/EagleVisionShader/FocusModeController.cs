using Player;
using UnityEngine;
using UnityEngine.Rendering;

public class FocusModeController : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private Material eagleVisionMaterial;

    private float transitionSpeed = 3;

    private Renderer[] targetRenderers;
    private Material[] defaultMaterials;
    private bool defaultMaterial = true;
    private float volumeWeight;
    private Volume volumeFocusMode;

    PlayerStateController playerStateController;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    void Start()
    {
        InitializeTargets();
        InitializeVolume();
    }
    private void OnEnable()
    {
        playerStateController.OnFocusMode += HandleInput;
    }
    private void OnDisable()
    {
        playerStateController.OnFocusMode -= HandleInput;
    }
    void Update()
    {
        UpdateVolumeWeight();
    }
    private void InitializeTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("FocusModeTarget");
        targetRenderers = new Renderer[targets.Length];
        defaultMaterials = new Material[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            Renderer rend = targets[i].GetComponent<Renderer>();
            if (rend != null)
            {
                targetRenderers[i] = rend;
                defaultMaterials[i] = rend.material;
            }
        }
    }
    private void InitializeVolume()
    {
        GameObject volume = GameObject.Find("VolumeFocusMode");
        if (volume != null)
        {
            volumeFocusMode = volume.GetComponent<Volume>();
            if (volumeFocusMode != null)
                volumeWeight = volumeFocusMode.weight;
        }
    }
    private void HandleInput()
    {
        ToggleMaterials();
        ToggleVolumeWeight();
    }
    private void ToggleMaterials()
    {
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] != null)
            {
                targetRenderers[i].material = defaultMaterial ? eagleVisionMaterial : defaultMaterials[i];
            }
        }
    }
    private void ToggleVolumeWeight()
    {
        volumeWeight = defaultMaterial ? 1 : 0;
        defaultMaterial = !defaultMaterial;
    }
    private void UpdateVolumeWeight()
    {
        if (volumeFocusMode != null)
        {
            volumeFocusMode.weight = Mathf.Lerp(volumeFocusMode.weight, volumeWeight, Time.deltaTime * transitionSpeed);

            // Fix lerp 0
            if (Mathf.Abs(volumeFocusMode.weight - volumeWeight) < 0.01f)
                volumeFocusMode.weight = volumeWeight;
        }
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        playerStateController = provider.PlayerStateController;
    }
}