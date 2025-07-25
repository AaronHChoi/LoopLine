using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour, IDependencyInjectable
{
    private CinemachineCamera rayCastPoint;
    private InventoryUI inventoryUI;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        rayCastPoint = provider.CinemachineCamera;
        inventoryUI = provider.InventoryUI;
    }
    void Update()
    {
        Debug.DrawRay(rayCastPoint.transform.position, rayCastPoint.transform.forward * raycastDistance, Color.red);

        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && inventoryUI.gameObject.activeInHierarchy == false)
            {
                IInteract interactableObject = GetInteractableObject();
                if (interactableObject != null)
                {
                    interactableObject.Interact();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
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
        Ray ray = new Ray(rayCastPoint.transform.position, rayCastPoint.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
            {
                if (hit.collider.TryGetComponent(out IInteract interactable))
                {
                    return interactable;
                }
            }
        }

        return null;
    
      /*  
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
        */
        }
}
