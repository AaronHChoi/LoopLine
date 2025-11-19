using DependencyInjection;
using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    BoxCollider collider;

    IPolaraidItem item;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
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
        collider.enabled = false;
    }
}