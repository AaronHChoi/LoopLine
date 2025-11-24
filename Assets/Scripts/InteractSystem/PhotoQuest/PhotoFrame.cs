using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotoFrame : MonoBehaviour, IInteract
{
    [Header("Settings")]
    [SerializeField] private string interactText = "Place Photo";
    [SerializeField] private PhotoQuestComponent correctPhoto;
    [SerializeField] private PhotoQuestComponent currentPhoto;
    [SerializeField] private List<PhotoQuestComponent> Photos;

    public bool CorrectPhotoPlaced { get; private set; }
    private bool isFrameOccupied = false;

    private IInventoryUI inventoryUI;
    private IPhotoQuestManager photoQuestManager;
    private IPlayerInputHandler playerInputHandler;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
        playerInputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
    }

    private void Start()
    {
        StartCoroutine(UpdateNextFrame());
    }
    public void Interact()
    {
        if (inventoryUI.ItemInUse.id != inventoryUI.HandItemUI.id && !photoQuestManager.allFramesCorrect)
        {
            var itemInUse = inventoryUI.ItemInUse.TryGetComponent<PhotoQuestComponent>(out var photoComponent) ? photoComponent : null;
            if (itemInUse == null) return;

            if (!isFrameOccupied)
            {
                PlacePhoto(itemInUse);
            }
        }
        else if (currentPhoto != null && inventoryUI.ItemInUse.id == inventoryUI.HandItemUI.id && !photoQuestManager.allFramesCorrect)
        {
            RemovePhoto(currentPhoto);
        }

    }

    private void PlacePhoto(PhotoQuestComponent photo)
    {
        inventoryUI.RemoveInventorySlot(photo);

        for (int i = 0; i < Photos.Count; i++)
        {
            if (photo.id == Photos[i].id)
            {
                Photos[i].gameObject.SetActive(true);
                Photos[i].gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            }
        }

        isFrameOccupied = true;
        photo.isItemPlaced = true;
        photo.photoFrame = this;
        currentPhoto = photo;

        if (photo == correctPhoto)
            CorrectPhotoPlaced = true;
        photoQuestManager.CheckAllFrames();
    }

    public void AllCorrectPhotoPlaced()
    {
        correctPhoto.gameObject.layer = LayerMask.NameToLayer("Default");
    }
    public void RemovePhoto(PhotoQuestComponent photo)
    {
        for (int i = 0; i < Photos.Count; i++)
        {
            if (photo.id == Photos[i].id)
            {
                Photos[i].gameObject.SetActive(false);
                currentPhoto.Interact();
                Photos[i].gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            }
        }
        currentPhoto = null;
        isFrameOccupied = false;
        CorrectPhotoPlaced = false;
    }

    private IEnumerator UpdateNextFrame()
    {
        yield return null;
        for (int i = 0; i < Photos.Count; i++)
        {
            Photos[i].gameObject.SetActive(false);
        }
    }
    public string GetInteractText() => interactText;
}
