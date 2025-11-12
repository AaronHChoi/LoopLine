using DependencyInjection;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhotoActivationEntry
{
    public GameCondition condition;
    public GameObject photo;
}
public class PhotoQuestManager : MonoBehaviour, IPhotoQuestManager
{
    public event Action OnPhotoQuestFinished;

    [SerializeField] SingleDoorInteract doorInteract;

    [SerializeField] ItemInteract doorHandler;

    [SerializeField] private List<PhotoFrame> frames;

    [SerializeField] List<PhotoActivationEntry> photoActivations;
    public bool allFramesCorrect { get; private set; } = false;

    IInventoryUI inventoryUI;

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

        UpdatePhotoActivationStates();
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
    void UpdatePhotoActivationStates()
    {
        if (photoActivations == null) return;

        foreach (var entry in photoActivations)
        {
            bool isConditionMet = GameManager.Instance.GetCondition(entry.condition);

            if (entry.photo != null)
            {
                entry.photo.SetActive(isConditionMet);
            }
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
        PhotoQuestComplete();

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
    private void PhotoQuestComplete()
    {
        OnPhotoQuestFinished?.Invoke();
        GameManager.Instance.SetCondition(GameCondition.IsPhotoQuestComplete, true);
    }
}
public interface IPhotoQuestManager
{
    void CheckAllFrames();
    bool allFramesCorrect { get; }

    event Action OnPhotoQuestFinished;
}