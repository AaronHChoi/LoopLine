using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using DependencyInjection;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private RaycastController rayController;
    [SerializeField] List<SoundData> grabSoundData;

    IPlayerStateController playerStateController;
    IInventoryUI inventoryUI;
    IGameSceneManager gameSceneManager;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
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
            if (inventoryUI.IsInventoryOpen == false)
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
        if (gameSceneManager.IsCurrentScene("05. MindPlace"))
        {
            if (inventoryUI.IsInventoryOpen == false && inventoryUI.ItemInUse == inventoryUI.HandItemUI)
            {
                IItemGrabInteract intemGrabObject = GetItemGrabIteractableObject();
                if (intemGrabObject != null)
                {
                    if (intemGrabObject.Interact())
                    {
                        SoundManager.Instance.CreateSound()
                            .WithSoundData(grabSoundData[Random.Range(0, grabSoundData.Count)])
                            .Play();
                    }
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
        if (rayController.FoundInteract) 
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
        if (inventoryUI.IsInventoryOpen == false && inventoryUI.ItemInUse == inventoryUI.HandItemUI && rayController.FoundInteract)
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