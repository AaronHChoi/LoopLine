using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using DependencyInjection;

public class PlayerInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private RaycastController rayController;
    [SerializeField, Range(0f,1f)] private float minScoreAllowed;

    IPlayerStateController playerStateController;
    IInventoryUI inventoryUI;
    
    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
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
    private void GrabItem()
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
        if (/*inventoryUI.IsInventoryOpen == false && */rayController.FoundInteract && rayController.BestScore > minScoreAllowed) 
        {
            if (rayController.Target.TryGetComponent(out IInteract interactable))
            {
                return interactable;
            }
        }

        return null;
    }

    public IItemGrabInteract GetItemGrabIteractableObject()
    {
        
        
            if (inventoryUI.IsInventoryOpen == false && inventoryUI.ItemInUse == inventoryUI.HandItemUI && rayController.FoundInteract && rayController.BestScore > minScoreAllowed)
            {
                if (rayController.Target.TryGetComponent(out IItemGrabInteract itemGrabInteractable))
                {
                    return itemGrabInteractable;
                }
            }
        

        return null;
    }
}

public interface IPlayerInteract
{
    IInteract GetInteractableObject();
}