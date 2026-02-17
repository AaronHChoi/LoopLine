using UnityEngine;

public class GenericLightController : MonoBehaviour
{
    [SerializeField] Light lightSource;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] Material onMaterial;
    Material offMaterial;

    bool startsEnabled = false;

    private void Awake()
    {
        offMaterial = meshRenderer.sharedMaterial;

        SetLight(startsEnabled);
    }
#if UNITY_EDITOR
    [ContextMenu("ToggleLight")]
    public void Toggle()
    {
        SetLight(!lightSource.enabled);
    }
#endif
    public void SetLight(bool state)
    {
        lightSource.enabled = state;

        meshRenderer.material = state ? offMaterial : onMaterial;
    }
}