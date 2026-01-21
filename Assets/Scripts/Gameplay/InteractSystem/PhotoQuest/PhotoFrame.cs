using DependencyInjection;
using UnityEngine;

public class PhotoFrame : MonoBehaviour, IInteract
{
    [Header("Settings")]
    [SerializeField] private string interactText = "Place Photo";
    [SerializeField] private PhotoQuestComponent correctPhoto;
    [SerializeField] private Events photoFrameEvent = Events.PodiumEmpty;
    [SerializeField] public PhotoQuestComponent currentPhoto;
    [SerializeField] public Transform SpawnPosition;
    public bool CorrectPhotoPlaced { get; private set; }
    private bool isFrameOccupied = false;

    private IInventoryUI inventoryUI;
    private IPhotoQuestManager photoQuestManager;
    private IPlayerInputHandler playerInputHandler;
    private IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        photoQuestManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoQuestManager>();
        playerInputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    public void Interact()
    {
        if (currentPhoto == null && inventoryUI.ItemInUse.id == inventoryUI.HandItemUI.id && !photoQuestManager.allFramesCorrect)
        {
            monologueSpeaker.StartMonologue(photoFrameEvent);
        }
        if (inventoryUI.ItemInUse.id != inventoryUI.HandItemUI.id && !photoQuestManager.allFramesCorrect)
        {
            var itemInUse = inventoryUI.ItemInUse.TryGetComponent<PhotoQuestComponent>(out var photoComponent) ? photoComponent : null;
            if (itemInUse == null) return;

            if (!isFrameOccupied)
            {
                PlacePhoto(itemInUse);
                photoQuestManager.CheckAllFrames();
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

        photoQuestManager.SetPhotoPosition(photo, this);

        isFrameOccupied = true;
        photo.isItemPlaced = true;
        photo.photoFrame = this;
        currentPhoto = photo;

        //if (photo == correctPhoto)
        //    CorrectPhotoPlaced = true;

        if (correctPhoto != null && photo.id == correctPhoto.id)
        {
            CorrectPhotoPlaced = true;
        }
        else
        {
            CorrectPhotoPlaced = false;
        }
    }
    public void AllCorrectPhotoPlaced()
    {
        correctPhoto.gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void RemovePhoto(PhotoQuestComponent photo)
    {
        //for (int i = 0; i < Photos.Count; i++)
        //{
        //    if (photo.id == Photos[i].id)
        //    {
        //        Photos[i].gameObject.SetActive(false);
        //        currentPhoto.Interact();
        //        Photos[i].gameObject.layer = LayerMask.NameToLayer("Default");
        //        break;
        //    }
        //}
        photoQuestManager.RemovePhoto(photo, this);
        currentPhoto = null;
        isFrameOccupied = false;
        CorrectPhotoPlaced = false;
    }

    public string GetInteractText() => interactText;
}
