using DependencyInjection;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SingleDoorInteract : MonoBehaviour, IInteract
{
    public event Action OnDoorOpened;
    public event Action OnDoorClosed;

    public bool isOpen = false;

    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private string doorText;
    [SerializeField] private bool IsRootatingDoor = true;
    [SerializeField] private float Speed = 1f;
    [SerializeField] float delayOpenDoorAnimation = 0.75f;

    private Vector3 playerPosition;

    [Header("Rotating Config")]
    [SerializeField] private float RotatingAmount = 90f;
    [SerializeField] private float ForwardDirection = 0f;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCorutine;

    IPlayerController playerController;
    IInventoryUI inventoryUI;

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

    private void Awake()
    {
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        StartRotation = doorGameObject.transform.rotation.eulerAngles;
        Forward = doorGameObject.transform.forward; //this is because the forward of the door is orienteted to the right if the forwar chages chage this line
    }
    private void Start()
    {
        if (usePersistence)
        {
            if (GameManager.Instance.GetCondition(doorCondition) && doorHandler != null)
            {
                active = true;
            }
        }

        // Disable the parent's trigger collider if it exists (use child colliders instead)
        Collider parentCollider = GetComponent<Collider>();
        if (parentCollider != null && parentCollider.isTrigger)
        {
            parentCollider.enabled = false;
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
        {
            return;
        }

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

        if (active)
        {
            StartCoroutine(CooldownRoutine());

            if (!isOpen)
            {
                playerPosition = playerController.GetTransform().position;
                OnUnlockDoorEvent?.Invoke();
                OpenSequence(playerPosition);
            }
            else
            {
                CloseSequence();
            }
        }
        else
        {
            EventBus.Publish(new DoorEvent { SoundID = lockedDoorSoundEventID, ShouldPlay = true });
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
        while(time < 1)
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