using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControllerScript : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph;

    [Header("Dissolve Settings")]
    public Material dissolveMaterialTemplate;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    [Header("Runtime Options")] 
    [Tooltip("If true, original shared materials are restored after the dissolve finishes.")] public bool restoreOriginalAfterDissolve = false;

    private Material[] _runtimeMaterials; // instanced materials used during play
    private Material[] _originalSharedMaterials; // cached for restoration if desired
    private bool _isDissolving;

    // Cached shader property IDs
    private static readonly int AlbedoId = Shader.PropertyToID("_Albedo");
    private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");
    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (VFXGraph == null) VFXGraph = GetComponent<VisualEffect>();

        if (VFXGraph != null)
        {
            VFXGraph.Stop();
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null && !VFXGraph.HasMesh("MeshRenderer"))
            {
                VFXGraph.SetMesh("MeshRenderer", mf.mesh);
            }
        }
    }
    private void OnEnable()
    {
        EventBus.Subscribe<FinalQuestCompleteEvent>(ActivateDissolve2);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<FinalQuestCompleteEvent>(ActivateDissolve2);
    }
    // Public dissolve trigger
    public void ActivateDissolve()
    {
        // Prevent any material changes while in edit mode
        if (!Application.isPlaying)
        {
            Debug.LogWarning("DissolveController: ActivateDissolve called in edit mode. Enter Play Mode to run dissolve.");
            return;
        }

        if (!_isDissolving)
        {
            PrepareMaterials();
            StartCoroutine(DissolveEffect());
        }
    }
    void ActivateDissolve2(FinalQuestCompleteEvent ev)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("DissolveController: ActivateDissolve called in edit mode. Enter Play Mode to run dissolve.");
            return;
        }

        if (!_isDissolving)
        {
            PrepareMaterials();
            StartCoroutine(DissolveEffect());
        }
    }

    [ContextMenu("Test Dissolve")]
    private void TestDissolve() => ActivateDissolve();

    private void PrepareMaterials()
    {
        if (meshRenderer == null || dissolveMaterialTemplate == null)
        {
            Debug.LogError("DissolveController: Missing MeshRenderer or Dissolve Material Template!");
            return;
        }

        Material[] originalMaterials = meshRenderer.sharedMaterials;
        _runtimeMaterials = new Material[originalMaterials.Length];
        _originalSharedMaterials = originalMaterials;

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            Material newMat = Instantiate(dissolveMaterialTemplate);
            var src = originalMaterials[i];
            if (src != null)
            {
                if (src.HasProperty(BaseMapId))
                    newMat.SetTexture(AlbedoId, src.GetTexture(BaseMapId));
                else if (src.HasProperty(MainTexId))
                    newMat.SetTexture(AlbedoId, src.GetTexture(MainTexId));
            }
            newMat.SetFloat(DissolveAmountId, 0f);
            _runtimeMaterials[i] = newMat;
        }

        meshRenderer.materials = _runtimeMaterials;
    }

    private IEnumerator DissolveEffect()
    {
        _isDissolving = true;
        if (VFXGraph != null) VFXGraph.Play();

        if (_runtimeMaterials != null && _runtimeMaterials.Length > 0)
        {
            float counter = 0f;
            while (counter < 1f)
            {
                counter += dissolveRate;
                for (int i = 0; i < _runtimeMaterials.Length; i++)
                {
                    var m = _runtimeMaterials[i];
                    if (m != null) m.SetFloat(DissolveAmountId, counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }

        if (restoreOriginalAfterDissolve && _originalSharedMaterials != null)
        {
            meshRenderer.sharedMaterials = _originalSharedMaterials;
        }
        _isDissolving = false;
    }

    private void OnDestroy()
    {
        if (_runtimeMaterials != null && Application.isPlaying)
        {
            for (int i = 0; i < _runtimeMaterials.Length; i++)
            {
                if (_runtimeMaterials[i] != null)
                    Destroy(_runtimeMaterials[i]);
            }
        }
    }
}
