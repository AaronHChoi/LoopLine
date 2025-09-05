using System.Collections;
using Player;
using UnityEngine;
using DependencyInjection;
public class EventDialogueManager : Subject, IDependencyInjectable
{
    [SerializeField] float delayMonologue;
    public static System.Action<string> OnItemPicked;
    [Header("Dialogues")]
    public DialogueSO firstDialogueQuest;
    public DialogueSO quest;
    public DialogueSO personQuest;

    [SerializeField] ItemInteract trainPhotos;
    int personPhotoCount = 0;
    PhotoCapture photoCapture;
    PlayerStateController controller;
    //HashSet<string> receivedClueId = new HashSet<string>();
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    private void Start()
    {
        StartCoroutine(StartSceneMonologue(delayMonologue));
        StartCoroutine(EnableTakeTrainPhotos());
    }
    private void OnEnable()
    {
        DialogueUI.OnDialogueEndedById += HandleDialgoueFinished;
        OnItemPicked += HandleItemPicked;
        PhotoCapture.OnPhotoClueCaptured += HandlePhotoClueCaptured;
    }
    private void OnDisable()
    {
        DialogueUI.OnDialogueEndedById -= HandleDialgoueFinished;
        OnItemPicked -= HandleItemPicked;
        PhotoCapture.OnPhotoClueCaptured += HandlePhotoClueCaptured;
    }
    IEnumerator EnableTakeTrainPhotos()
    {
        yield return new WaitUntil(() => personQuest.Finished);
        trainPhotos.canBePicked = true;
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
        NotifyObservers(Events.TriggerMonologue);
        controller.CanUseNormalStateExecute = true;
    }
    private IEnumerator StartSceneMonologue(float delay, Events _event)
    {
        controller.CanUseNormalStateExecute = false;
        yield return new WaitForSeconds(delay);
        NotifyObservers(_event);
        NotifyObservers(Events.TriggerMonologue);
        controller.CanUseNormalStateExecute = true;
    }
    void SendOnlyEvent(Events _event)
    {
        NotifyObservers(_event);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        photoCapture = provider.PhotoContainer.PhotoCapture;
        controller = provider.PlayerContainer.PlayerStateController;
    }
}