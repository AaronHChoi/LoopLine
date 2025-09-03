using System.Collections;
using Player;
using UnityEngine;

public class EventDialogueManager : Subject, IDependencyInjectable
{
    [SerializeField] float delayMonologue;
    public static System.Action<string> OnItemPicked;
    public DialogueSO quest;
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
    void HandlePhotoClueCaptured(string clueId)
    {
        if (!quest.Finished) return;

        StartCoroutine(WaitUntilCameraClosed(clueId));
    }
    IEnumerator WaitUntilCameraClosed(string clueId)
    {
        yield return new WaitUntil(() => !photoCapture.IsViewingPhoto && !CameraState.PolaroidIsActive);

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
            personPhotoCount++;
            if(personPhotoCount == 2)
            {
                StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_Two_Good_Photos_M));
            }
            if(personPhotoCount == 3)
            {
                StartCoroutine(StartSceneMonologue(delayMonologue, Events.With_Event_Photo));
            }
        }
    }
    void HandleItemPicked(string itemId)
    {
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
        photoCapture = provider.PhotoCapture;
        controller = provider.PlayerStateController;
    }
}