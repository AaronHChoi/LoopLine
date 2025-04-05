using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameObject rayCastPoint;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(rayCastPoint.transform.position, rayCastPoint.transform.forward * raycastDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IInteract interactableObject = GetInteractableObject();
            if (interactableObject != null)
            {
                interactableObject.Interact();
            }
        }
    }

    public IInteract GetInteractableObject()
    {
        //List<IInteract> InteractableList = new List<IInteract>();

        Ray ray = new Ray(rayCastPoint.transform.position, rayCastPoint.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, interactableLayer);

        IInteract interactableObject = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out IInteract interactable))
            {
                interactableObject = interactable;
                //interactableLayer = hit.collider.gameObject.layer;
            }
        }

        return interactableObject;
    }
}
