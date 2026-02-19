using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Player;
using Core.EventBus;
using Core.Utilities;
using Core.DependencyInjection;
using Core.Data;
using Core.UI;

public class FinalDoor : MonoBehaviour, IInteract
{
    [SerializeField] VideoClip succesCinematic;

    public event Action OnDoorOpened;
    public event Action OnDoorClosed;

    public bool isOpen = false;

    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private string doorText;
    [SerializeField] private bool IsRootatingDoor = true;
    [SerializeField] private float Speed = 1f;
    [SerializeField] float delayOpenDoorAnimation = 0.75f;
    [SerializeField] FinalDoor linkedDoor;

    private Vector3 playerPosition;

    [Header("Rotating Config")]
    [SerializeField] private float RotatingAmount = 90f;
    [SerializeField] private float ForwardDirection = 0f;
    private Vector3 StartRotation;
    private Vector3 Forward;
    private Coroutine AnimationCorutine;

    IPlayerController playerController;
    IInventoryUI inventoryUI;
    IPlayerStateController playerStateController;
    ICinematicManager cinematicManager;
    IFadeInOutController fadeInOutController;
    IFadeInOutController finalFade2;
    IMonologueSpeaker monologueSpeaker;
    IPlayerMovement playerMovement;
    IGameSceneManager gameSceneManager;
    IInventoryUI inventory;

    [SerializeField] GameObject doorHandler;
    [SerializeField] TutorialInteract correctKey;
    [SerializeField] bool active = false;

    [SerializeField] EventsID unlockDoorSoundEventID;
    [SerializeField] EventsID lockedDoorSoundEventID;
    [SerializeField] string keyString;

    [SerializeField] private UnityEvent OnUnlockDoorEvent;

    [SerializeField] bool inDoor;

    [Header("Cooldown Config")]
    [SerializeField] private float interactCooldown = 1.0f;
    private bool inCooldown = false;

    [Header("Persistence Settings")]
    [SerializeField] private bool usePersistence = true;
    [SerializeField] private GameCondition doorCondition;
    [SerializeField] private Events monologueToTrigger;

    private void Awake()
    {
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        StartRotation = doorGameObject.transform.rotation.eulerAngles;
        cinematicManager = InterfaceDependencyInjector.Instance.Resolve<ICinematicManager>();
        fadeInOutController = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.FinalFade);
        finalFade2 = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.FinalFade2);
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.FinalMonologue);
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
        inventory = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        Forward = doorGameObject.transform.forward; //this is because the forward of the door is orienteted to the right if the forwar chages chage this line
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.FinalQuestCompleted) && doorHandler != null)
        {
            active = true;
            doorHandler.SetActive(true);
        }
    }
    public void OpenDoor(Vector3 UserPosition)
    {
        if (!isOpen)
        {
            if (AnimationCorutine != null)
            {
                StopCoroutine(AnimationCorutine);
            }
            if (IsRootatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - doorGameObject.transform.position).normalized);
                AnimationCorutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }
    public void CloseDoor()
    {
        if (isOpen)
        {
            if (AnimationCorutine != null)
            {
                StopCoroutine(AnimationCorutine);
            }
            if (IsRootatingDoor)
            {
                AnimationCorutine = StartCoroutine(DoRotationClose());
            }
        }
    }
    public void Interact()
    {
        if (inCooldown)
            return;

        if (keyString != null)
        {
            if (inventoryUI.ItemInUse.id == keyString && !active)
            {
                StartCoroutine(CooldownRoutine());

                active = true;
                if (usePersistence)
                {
                    GameManager.Instance.SetCondition(doorCondition, true);
                }
                EventBus.Publish(new DoorEvent { SoundID = unlockDoorSoundEventID, ShouldPlay = true });
                OnUnlockDoorEvent?.Invoke();
                return;
            }
        }

        StartCoroutine(CooldownRoutine());

        if (!active)
        {
            EventBus.Publish(new DoorEvent
            {
                SoundID = lockedDoorSoundEventID,
                ShouldPlay = true
            });
            return;
        }
        if (active)
        {
            Vector3 playerPos = playerController.GetTransform().position;
            ToggleDoor(playerPos);
        }


        
    }

    private void ToggleDoor(Vector3 userPosition, bool fromLinkedDoor = false)
    {
        if (!isOpen)
        {
            OpenSequence(userPosition);
            //cinematicManager.PlayCinematic(succesCinematic, () =>
            //{
                 //playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);

            //    GameManager.Instance.SetGameConditions();
            //});
            
        }
        else
        {
            CloseSequence();
        }

        // Solo propaga UNA VEZ
        if (!fromLinkedDoor && linkedDoor != null)
        {
            linkedDoor.ToggleDoor(userPosition, true);
        }
    }
    IEnumerator CooldownRoutine()
    {
        inCooldown = true;
        yield return new WaitForSeconds(interactCooldown);
        inCooldown = false;
    }
    public string GetInteractText()
    {
        return doorText;
    }

    public void DoorUnlocked()
    {
        inventoryUI.RemoveInventorySlot(correctKey);
        keyString = "";
        active = true;
        StartCoroutine(PlayCinematic());
    }

    private IEnumerator PlayCinematic()
    {
        if (inventory.IsInventoryOpen)
        {
            playerStateController.UseEventOpenInventory();
        }
        playerMovement.CanMove = false;
        fadeInOutController.ForceFade(true);
        monologueSpeaker.StartMonologue(monologueToTrigger);
        yield return new WaitForSeconds(30f);
        fadeInOutController.ForceFade(false);
        yield return new WaitForSeconds(1f);
        finalFade2.ForceFade(true);
        yield return new WaitForSeconds(3f);
        finalFade2.ForceFade(false);
        playerMovement.CanMove = true;
        gameSceneManager.LoadNextScene("01. MainMenu");
    }
    private void OpenSequence(Vector3 userPosition)
    {
        OnDoorOpened?.Invoke();

        DelayUtility.Instance.Delay(delayOpenDoorAnimation, () => OpenDoor(userPosition));
    }
    private void CloseSequence()
    {
        OnDoorClosed?.Invoke();

        DelayUtility.Instance.Delay(0.6f, CloseDoor);
    }
    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = doorGameObject.transform.rotation;
        Quaternion endRotation;

        if (inDoor)
        {
            if (ForwardAmount >= ForwardDirection)
            {
                endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y - RotatingAmount, StartRotation.z));
            }
            else
            {
                endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y + RotatingAmount, StartRotation.z));
            }
        }
        else
        {
            if (ForwardAmount >= ForwardDirection)
            {
                endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y + RotatingAmount, StartRotation.z));
            }
            else
            {
                endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y - RotatingAmount, StartRotation.z));
            }
        }

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            doorGameObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = doorGameObject.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        isOpen = false;

        float time = 0;
        while (time < 1)
        {
            doorGameObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

}