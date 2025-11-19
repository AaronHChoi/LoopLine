using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControllerScript : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph; // Fixed typo from 'VFXGrapth'

    [Header("Settings")]
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    [SerializeField] private string vfxMeshPropertyName = "MeshRenderer"; // Matches your VFX Graph property name

    private Material[] meshMaterials;
    private bool isDissolving = false;

    void Start()
    {
        // 1. Auto-find components if not assigned manually
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (VFXGraph == null) VFXGraph = GetComponent<VisualEffect>();

        // 2. Setup Materials
        if (meshRenderer != null)
        {
            meshMaterials = meshRenderer.materials;
        }

        // 3. Stop VFX immediately so it doesn't play when the scene starts
        if (VFXGraph != null)
        {
            VFXGraph.Stop();

            // CRITICAL: Assign the current object's mesh to the VFX Graph
            // This ensures the particles take the shape of THIS specific object
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null)
            {
                VFXGraph.SetMesh(vfxMeshPropertyName, mf.mesh);
            }
        }
    }

    // 4. EXPOSED METHOD: Call this when you pick up the object
    public void ActivateDissolve()
    {
        if (!isDissolving)
        {
            StartCoroutine(DissolveEffect());
        }
    }

    // Context menu allows you to test inside Unity Editor by right-clicking the script component
    [ContextMenu("Test Dissolve")]
    public void TestDissolve()
    {
        ActivateDissolve();
    }

    IEnumerator DissolveEffect()
    {
        isDissolving = true;

        // Play the VFX only now
        if (VFXGraph != null)
        {
            VFXGraph.Play();
        }

        if (meshMaterials.Length > 0)
        {
            float counter = 0f;

            // Use the first material's current value as a starting point in case it's not 0
            if (meshMaterials[0].HasProperty("_DissolveAmount"))
            {
                counter = meshMaterials[0].GetFloat("_DissolveAmount");
            }

            while (counter < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < meshMaterials.Length; i++)
                {
                    meshMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }

        // Optional: Destroy the game object after the effect is done
        // Destroy(gameObject, 2.0f); 
    }
}