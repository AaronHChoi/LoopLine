using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using DependencyInjection;
public class PlayerInteract : MonoBehaviour, IDependencyInjectable
{
    private CinemachineCamera rayCastPoint;
    private InventoryUI inventoryUI;
    [SerializeField] public float raycastDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    PlayerStateController playerStateController;
    
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnInteract += HandleInteraction;
            playerStateController.OnGrab += GrabItem;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnInteract -= HandleInteraction;
            playerStateController.OnGrab -= GrabItem;
        }
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        rayCastPoint = provider.CinemachineContainer.CinemachineCamera;
        inventoryUI = provider.UIContainer.InventoryUI;
        playerStateController = provider.PlayerContainer.PlayerStateController;
    }
    private void HandleInteraction()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (inventoryUI.IsInventoryOpen == false /*&& playerInventorySystem.ItemInUse == inventoryUI.HandItemUI*/)
            {
                TryInteract();
            }
        }
        else
        {
            TryInteract();
        }
    }
    void Update()
    {
        Debug.DrawRay(rayCastPoint.transform.position, rayCastPoint.transform.forward * raycastDistance, Color.red);
    }
    private void GrabItem()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (inventoryUI.IsInventoryOpen == false && inventoryUI.ItemInUse == inventoryUI.HandItemUI)
            {
                IItemGrabInteract intemGrabObject = GetItemGrabIteractableObject();
                if (intemGrabObject != null)
                {
                    intemGrabObject.Interact();
                }
            }
        }
    }
    private void TryInteract()
    {
        IInteract interactableObject = GetInteractableObject();
        if (interactableObject != null)
        {
            interactableObject.Interact();
        }
    }
    public IInteract GetInteractableObject()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (inventoryUI.IsInventoryOpen == false) 
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
            }
        }

        else
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

    public IItemGrabInteract GetItemGrabIteractableObject()
    {
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (inventoryUI.IsInventoryOpen == false && inventoryUI.ItemInUse == inventoryUI.HandItemUI)
            {
                Ray ray = new Ray(rayCastPoint.transform.position, rayCastPoint.transform.forward);

                if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
                {
                    if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
                    {

                        if (hit.collider.TryGetComponent(out IItemGrabInteract itemGrabInteractable))
                        {
                            return itemGrabInteractable;
                        }
                    }
                }
            }
        }
        return null;
    }
}