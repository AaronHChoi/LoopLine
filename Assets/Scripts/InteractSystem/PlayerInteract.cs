using Player;
using UnityEngine;
using DependencyInjection;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private RaycastController rayController;
    [SerializeField] List<SoundData> grabSoundData;

    IPlayerStateController playerStateController;
    IGameSceneManager gameSceneManager;

    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
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
        if (gameSceneManager.IsCurrentScene("05. MindPlace"))
        {
            TryInteract();
        }
        else
        {
            TryInteract();
        }
    }
    private void GrabItem()
    {
        IItemGrabInteractable intemGrabObject = GetItemGrabIteractableObject();
        if (intemGrabObject != null)
        {
            if (intemGrabObject.Interact())
            {
                EventBus.Publish(new PlayerGrabItemEvent());
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
        if (rayController.FoundInteract)
        {
            var target = rayController.Target;
            if (target == null) return null;

            // Search the hierarchy to support interactions on child colliders
            IInteract interactable = target.GetComponent<IInteract>();
            if (interactable == null)
                interactable = target.GetComponentInParent<IInteract>();
            if (interactable == null)
                interactable = target.GetComponentInChildren<IInteract>();

            return interactable;
        }
        return null;
    }
    public IItemGrabInteractable GetItemGrabIteractableObject()
    {
        if (rayController.FoundInteract)
        {
            var target = rayController.Target;
            if (target == null) return null;

            // Search the hierarchy to support item grab on child colliders
            IItemGrabInteractable itemGrabInteractable = target.GetComponent<IItemGrabInteractable>();
            if (itemGrabInteractable == null)
                itemGrabInteractable = target.GetComponentInParent<IItemGrabInteractable>();
            if (itemGrabInteractable == null)
                itemGrabInteractable = target.GetComponentInChildren<IItemGrabInteractable>();

            return itemGrabInteractable;
        }
        return null;
    }
    public GameObject GetRaycastTarget()
    {
        if (rayController.FoundInteract)
        {
            return rayController.Target;
        }
        return null;
    }
}
public interface IPlayerInteract
{
    IInteract GetInteractableObject();
    GameObject GetRaycastTarget();
}