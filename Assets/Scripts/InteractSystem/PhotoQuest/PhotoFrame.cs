using DependencyInjection;
using Unity.VisualScripting;
using UnityEngine;

public class PhotoFrame : MonoBehaviour, IInteract
{
    [Header("Settings")]
    [SerializeField] private string interactText = "Place Photo";
    [SerializeField] private PhotoQuestComponent correctPhoto;
    [SerializeField] private Transform photoSpawnPoint;
    [SerializeField] private Transform photoScalePlaced;

    public bool CorrectPhotoPlaced { get; private set; }
    private bool isFrameOccupied = false;

    private PhotoQuestComponent currentPhoto;
    private IInventoryUI inventoryUI;
    private IPhotoQuestManager photoQuestManager;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
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

        photoQuestManager.CheckAllFrames();
    }

    private void PlacePhoto(PhotoQuestComponent photo)
    {
        inventoryUI.RemoveInventorySlot(photo);
        photo.gameObject.transform.position = photoSpawnPoint.position;
        photo.gameObject.transform.rotation = photoSpawnPoint.rotation;
        photo.objectPrefab.transform.rotation = photoSpawnPoint.rotation;
        photo.gameObject.transform.localScale = photoScalePlaced.localScale;
        photo.objectPrefab.SetActive(true);
        photo.gameObject.SetActive(true);

        isFrameOccupied = true;
        photo.isItemPlaced = true;
        photo.photoFrame = this;
        currentPhoto = photo;

        if (photo == correctPhoto)
            CorrectPhotoPlaced = true;
    }

    public void AllCorrectPhotoPlaced()
    {
        correctPhoto.gameObject.layer = LayerMask.NameToLayer("Default");
    }
    public void RemovePhoto()
    {
        if (currentPhoto == null) return;
        currentPhoto = null;
        isFrameOccupied = false;
        CorrectPhotoPlaced = false;
    }

    public string GetInteractText() => interactText;
}
