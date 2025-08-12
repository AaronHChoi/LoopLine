using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour, IDependencyInjectable
{
    private CinemachineCamera _rayCastPoint;
    private PlayerInventorySystem _playerInventorySystem;
    private InventoryUI _inventoryUI;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        _rayCastPoint = provider.CinemachineCamera;
        _inventoryUI = provider.InventoryUI;
        _playerInventorySystem = provider.PlayerInventorySystem;
    }
    void Update()
    {
        if (PhotoCapture.isCameraActiveGlobal)
            return;

        Debug.DrawRay(_rayCastPoint.transform.position, _rayCastPoint.transform.forward * raycastDistance, Color.red);

        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _inventoryUI.gameObject.activeInHierarchy == false && _playerInventorySystem.ItemInUse == null)
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
        if (SceneManager.GetActiveScene().name == "04. Train")
        {
            if (_inventoryUI.gameObject.activeInHierarchy == false && _playerInventorySystem.ItemInUse == null)
            {
                Ray ray = new Ray(_rayCastPoint.transform.position, _rayCastPoint.transform.forward);

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
            Ray ray = new Ray(_rayCastPoint.transform.position, _rayCastPoint.transform.forward);

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
    }
}