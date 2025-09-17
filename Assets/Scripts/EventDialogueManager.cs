using System;
using System.Collections;
using DependencyInjection;
using Player;
using UnityEngine;

public class EventDialogueManager : Subject, IEventDialogueManager
{
    [SerializeField] float delayMonologue;
    public Action<string> OnItemPicked {  get;  set; }
    [Header("Dialogues")]
    public DialogueSO firstDialogueQuest;
    public DialogueSO quest;
    public DialogueSO personQuest;

    [SerializeField] ItemInteract trainPhotos;
    int personPhotoCount = 0;

    IPhotoCapture photoCapture;
    IPlayerStateController controller;
    IDialogueUI dialogueUI;

    private void Awake()
    {
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        dialogueUI = InterfaceDependencyInjector.Instance.Resolve<IDialogueUI>();
        photoCapture = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
    }
    private void Start()
    {
        StartCoroutine(StartSceneMonologue(delayMonologue));
        StartCoroutine(EnableTakeTrainPhotos());
    }
    private void OnEnable()
    {
        dialogueUI.OnDialogueEndedById += HandleDialgoueFinished;
        OnItemPicked += HandleItemPicked;
        photoCapture.OnPhotoClueCaptured += HandlePhotoClueCaptured;
    }
    private void OnDisable()
    {
        dialogueUI.OnDialogueEndedById -= HandleDialgoueFinished;
        OnItemPicked -= HandleItemPicked;
        photoCapture.OnPhotoClueCaptured -= HandlePhotoClueCaptured;
    }
    IEnumerator EnableTakeTrainPhotos()
    {
        yield return new WaitUntil(() => personQuest.Finished);
        trainPhotos.canBePicked = true;
        GameManager.Instance.CameraGirlPhoto = true;
    }
    void HandlePhotoClueCaptured(string clueId)
    {
        if (!quest.Finished) return;

        StartCoroutine(WaitUntilCameraClosed(clueId));
    }
    IEnumerator WaitUntilCameraClosed(string clueId)
    {
        yield return new WaitUntil(() => !photoCapture.IsViewingPhoto && !CameraState.PolaroidIsActive);

        personPhotoCount++;
        bool isFirstTimeForThisClue = false;

        if (clueId == null)
        {
            if (!isFirstTimeForThisClue)
            {
                SendOnlyEvent(Events.With_Any_Good_Photos);
            }
            isFirstTimeForThisClue = true;
        }
        else if (clueId == "Event")
        {
            StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_Event_Photo));
        }
        else if (clueId == "Person")
        {
            StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_Some_Good_Photos));
            
            if(personPhotoCount == 2)
            {
                StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_Two_Good_Photos_M));
            }
            if(personPhotoCount == 3)
            {
                StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_All_Good_Photos));
            }
        }
    }
    void HandleItemPicked(string itemId)
    {
        if (firstDialogueQuest.Finished) return;

        if(itemId == "Camera")
        {
            SendOnlyEvent(Events.With_Camera);
        }
    }
    void HandleDialgoueFinished(DialogueSO dialogue)
    {
        if (dialogue.events != Events.None)
        {
            StartCoroutine(StartSceneMonologue(delayMonologue, dialogue.events));
        }
    }
    private IEnumerator StartSceneMonologue(float delay)
    {
        controller.CanUseNormalStateExecute = false;
        yield return new WaitForSeconds(delay);
        NotifyObservers(Events.TriggerMonologue, "Player");
        controller.CanUseNormalStateExecute = true;
    }
    private IEnumerator StartSceneMonologue(float delay, Events _event)
    {
        controller.CanUseNormalStateExecute = false;
        yield return new WaitForSeconds(delay);
        NotifyObservers(_event);
        NotifyObservers(_event, "Player");
        NotifyObservers(Events.TriggerMonologue, "Player");
        controller.CanUseNormalStateExecute = true;
    }
    void SendOnlyEvent(Events _event)
    {
        NotifyObservers(_event);
    }
}

public interface IEventDialogueManager
{
    Action<string> OnItemPicked {  get; set; }
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
}