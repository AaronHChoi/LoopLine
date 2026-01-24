using DependencyInjection;
using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    BoxCollider evidenceCollider;

    IPolaraidItem item;

    private void Awake()
    {
        evidenceCollider = GetComponent<BoxCollider>();
        item = InterfaceDependencyInjector.Instance.Resolve<IPolaraidItem>();
    }
    private void OnEnable()
    {
        if (item != null)
        {
            item.OnPolaroidTaken += OnPolaroidTaken;
        }
    }
    private void OnDisable()
    {
        if (item != null)
        {
            item.OnPolaroidTaken -= OnPolaroidTaken;
        }
    }
    private void OnPolaroidTaken()
    {
        evidenceCollider.enabled = false;
    }
}