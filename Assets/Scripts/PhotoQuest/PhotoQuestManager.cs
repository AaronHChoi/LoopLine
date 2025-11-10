using DependencyInjection;
using System.Collections.Generic;
using UnityEngine;

public class PhotoQuestManager : MonoBehaviour, IPhotoQuestManager
{
    IInventoryUI inventoryUI;
    [SerializeField] SingleDoorInteract doorInteract;

    [SerializeField] ItemInteract doorHandler;

    [SerializeField] private List<PhotoFrame> frames;
    public bool allFramesCorrect { get; private set; } = false;
    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.PhotoDoorOpen))
        {
            doorHandler.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (doorInteract  != null)
        {
            doorInteract.OnPhotoQuestOpenDoor += OpenDoorPhotoQuest;
        }
    }
    private void OnDisable()
    {
        if (doorInteract != null)
        {
            doorInteract.OnPhotoQuestOpenDoor -= OpenDoorPhotoQuest;
        }
    }

    public void CheckAllFrames()
    {
        foreach (var frame in frames)
        {
            if (!frame.CorrectPhotoPlaced)
                return; 
        }

        OnQuestCompleted();
    }

    private void OnQuestCompleted()
    {
        Debug.Log("Todas las fotos colocadas correctamente");       
        allFramesCorrect = true;
        foreach (var frame in frames)
        {
            frame.AllCorrectPhotoPlaced();
        }
    }
    private void OpenDoorPhotoQuest()
    {
        inventoryUI.RemoveInventorySlot(doorHandler);
        GameManager.Instance.SetCondition(GameCondition.PhotoDoorOpen, true);
    }
}

public interface IPhotoQuestManager
{
    void CheckAllFrames();
    bool allFramesCorrect { get; }
    private void PhotoQuestComplete()
    {
        GameManager.Instance.SetCondition(GameCondition.IsPhotoQuestComplete, true);
    }
}