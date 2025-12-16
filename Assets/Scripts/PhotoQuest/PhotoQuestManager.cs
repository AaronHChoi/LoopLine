using DependencyInjection;
using System;
using System.Collections;
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
    [SerializeField] Events QuestCompleteEvent;
    [SerializeField] TutorialInteract Key;


    [SerializeField] private List<PhotoFrame> frames;
    [SerializeField] private List<PhotoQuestComponent> Photos;

    [SerializeField] List<PhotoActivationEntry> photoActivations;
    public bool allFramesCorrect { get; private set; } = false;

    IInventoryUI inventoryUI;
    IGameSceneManager gameSceneManager;
    IMonologueSpeaker monologueSpeaker;
    IFinalQuestManager finalQuestManager;


    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        finalQuestManager = InterfaceDependencyInjector.Instance.Resolve<IFinalQuestManager>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.PhotoDoorOpen))
        {
            doorHandler.gameObject.SetActive(false);
        }
        StartCoroutine(UpdateNextFrame());
        UpdatePhotoActivationStates();
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PhotoQuestComplete();
        }
    }
#endif
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

    public void SetPhotoPosition(PhotoQuestComponent photo, PhotoFrame frame)
    {
        for (int i = 0; i < Photos.Count; i++)
        {
            if (photo.id == Photos[i].id)
            {
                Photos[i].gameObject.transform.position = frame.SpawnPosition.position;
                Photos[i].gameObject.SetActive(true);
                Photos[i].gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            }
        }
    }

    public void RemovePhoto(PhotoQuestComponent photo, PhotoFrame frame)
    {
        for (int i = 0; i < Photos.Count; i++)
        {
            if (photo.id == Photos[i].id)
            {
                Photos[i].gameObject.SetActive(false);
                frame.currentPhoto.Interact();
                Photos[i].gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            }
        }
    }
    private void OnQuestCompleted()
    {
        PhotoQuestComplete();
        GameManager.Instance.SetCondition(GameCondition.WordGroup2, true);
        finalQuestManager.UpdateWordsActivation();
        Debug.Log("Todas las fotos colocadas correctamente");       
        allFramesCorrect = true;
        foreach (var frame in frames)
        {
            frame.AllCorrectPhotoPlaced();
        }
    }
    public void OpenDoorPhotoQuest()
    {
        inventoryUI.RemoveInventorySlot(doorHandler);        
        GameManager.Instance.SetCondition(GameCondition.PhotoDoorOpen, true);
        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
        gameSceneManager.SetInitialLoop(true);
    }
    private void PhotoQuestComplete()
    { 
        Key.Interact();
        OnPhotoQuestFinished?.Invoke();
        GameManager.Instance.SetCondition(GameCondition.IsPhotoQuestComplete, true);
    }
    private IEnumerator UpdateNextFrame()
    {
        yield return null;
        for (int i = 0; i < Photos.Count; i++)
        {
            Photos[i].gameObject.SetActive(false);
        }
    }
}
public interface IPhotoQuestManager
{
    void CheckAllFrames();
    bool allFramesCorrect { get; }

    event Action OnPhotoQuestFinished;
    void SetPhotoPosition(PhotoQuestComponent photo, PhotoFrame frame);
    void RemovePhoto(PhotoQuestComponent photo, PhotoFrame frame);
}