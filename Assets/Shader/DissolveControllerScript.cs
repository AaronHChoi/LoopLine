using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControllerScript : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph;

    [Header("Dissolve Settings")]
    // Drag your "Shader Graphs_DisolveShader" material here in the Inspector
    public Material dissolveMaterialTemplate;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    // Ensure this string matches the property name in your VFX Graph exactly
    [SerializeField] private string vfxMeshPropertyName = "MeshRenderer";

    private Material[] runtimeMaterials;
    private bool isDissolving = false;

    void Start()
    {
        // Auto-assign references if missing
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (VFXGraph == null) VFXGraph = GetComponent<VisualEffect>();

        // Stop VFX immediately on start so it doesn't play automatically
        if (VFXGraph != null)
        {
            VFXGraph.Stop();

            // Pass the generic mesh to the VFX Graph so particles spawn in the correct shape
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null)
            {
                if (VFXGraph.HasFloat(vfxMeshPropertyName) == false) // Simple check to avoid errors if property missing
                {
                    VFXGraph.SetMesh(vfxMeshPropertyName, mf.mesh);
                }
            }
        }
    }

    // --- PUBLIC METHODS ---

    // Call this method from your Interaction/PickUp script
    public void ActivateDissolve()
    {
        if (!isDissolving)
        {
            // 1. Swap materials BEFORE starting the effect
            PrepareMaterials();

            // 2. Start the dissolve loop
            StartCoroutine(DissolveEffect());
        }
    }

    // This allows you to right-click the script in the Inspector and choose "Test Dissolve"
    [ContextMenu("Test Dissolve")]
    public void TestDissolve()
    {
        ActivateDissolve();
    }

    // --- INTERNAL LOGIC ---

    void PrepareMaterials()
    {
        if (meshRenderer == null || dissolveMaterialTemplate == null)
        {
            Debug.LogError("DissolveController: Missing MeshRenderer or Dissolve Material Template!");
            return;
        }

        Material[] originalMaterials = meshRenderer.materials;
        runtimeMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            // Create a new material copy based on your blue dissolve shader
            Material newMat = new Material(dissolveMaterialTemplate);

            // STEAL THE TEXTURE from the old material
            // This makes the wood box look like wood, and the metal box look like metal
            // before they dissolve.
            if (originalMaterials[i].HasProperty("_BaseMap")) // HDRP/URP standard
            {
                newMat.SetTexture("_Albedo", originalMaterials[i].GetTexture("_BaseMap"));
            }
            else if (originalMaterials[i].HasProperty("_MainTex")) // Built-in standard
            {
                newMat.SetTexture("_Albedo", originalMaterials[i].GetTexture("_MainTex"));
            }

            // Initialize dissolve amount to 0
            newMat.SetFloat("_DissolveAmount", 0);

            runtimeMaterials[i] = newMat;
        }

        // Swap the materials on the object
        meshRenderer.materials = runtimeMaterials;
    }

    IEnumerator DissolveEffect()
    {
        isDissolving = true;

        // Play the VFX now that the dissolve is starting
        if (VFXGraph != null)
        {
            VFXGraph.Play();
        }

        // Animate the dissolve value from 0 to 1
        if (runtimeMaterials != null && runtimeMaterials.Length > 0)
        {
            float counter = 0f;

            while (counter < 1f)
            {
                counter += dissolveRate;
                for (int i = 0; i < runtimeMaterials.Length; i++)
                {
                    runtimeMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }

        // Optional: Destroy object after dissolve completes
        // Destroy(gameObject, 1.0f);
    }
}