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
            if (rayController.Target.TryGetComponent(out IInteract interactable))
            {
                return interactable;
            }
        }
        return null;
    }
    public IItemGrabInteractable GetItemGrabIteractableObject()
    {
        if (rayController.FoundInteract)
        {
            if (rayController.Target.TryGetComponent(out IItemGrabInteractable itemGrabInteractable))
            {
                return itemGrabInteractable;
            }
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