using UnityEngine;
using UnityEngine.Rendering;

public class FocusModeController : MonoBehaviour
{
    [SerializeField] private Material eagleVisionMaterial;

    private float transitionSpeed = 3;

    private Renderer[] targetRenderers;
    private Material[] defaultMaterials;
    private bool defaultMaterial = true;
    private float volumeWeight;
    private Volume volumeFocusMode;

    void Start()
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

        GameObject volumeObject = GameObject.Find("VolumeFocusMode");
        if (volumeObject != null)
        {
            volumeFocusMode = volumeObject.GetComponent<Volume>();
            if (volumeFocusMode != null)
                volumeWeight = volumeFocusMode.weight;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                if (targetRenderers[i] != null)
                {
                    targetRenderers[i].material = defaultMaterial ? eagleVisionMaterial : defaultMaterials[i];
                }
            }

            volumeWeight = defaultMaterial ? 1 : 0;
            defaultMaterial = !defaultMaterial;
        }

        if (volumeFocusMode != null)
        {
            volumeFocusMode.weight = Mathf.Lerp(volumeFocusMode.weight, volumeWeight, Time.deltaTime * transitionSpeed);

            // Fix lerp 0
            if (Mathf.Abs(volumeFocusMode.weight - volumeWeight) < 0.01f)
                volumeFocusMode.weight = volumeWeight;
        }
    }

}