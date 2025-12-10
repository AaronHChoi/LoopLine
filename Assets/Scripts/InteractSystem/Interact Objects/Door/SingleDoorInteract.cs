using DependencyInjection;
using System;
using System.Collections;
using UnityEngine;

public class SingleDoorInteract : MonoBehaviour, IInteract
{
    public event Action OnDoorOpened;
    public event Action OnDoorClosed;
    public event Action OnPhotoQuestOpenDoor;

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
    [SerializeField] bool active = false;

    [SerializeField] EventsID soundEventID;

    private void Awake()
    {
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        StartRotation = doorGameObject.transform.rotation.eulerAngles;
        Forward = doorGameObject.transform.forward; //this is because the forward of the door is orienteted to the right if the forwar chages chage this line
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.PhotoDoorOpen) && doorHandler != null)
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
                EventBus.Publish(new UnlockDoorEvent { SoundID = EventsID.OpenDoor, ShouldPlay = true });
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
        if (inventoryUI.ItemInUse.id == "Key" && !active)
        {
            active = true;
            EventBus.Publish(new UnlockDoorEvent { SoundID = soundEventID, ShouldPlay = true });
            //OnPhotoQuestOpenDoor?.Invoke();
            return;
        }

        if (active)
        {
            if (!isOpen)
            {
                playerPosition = playerController.GetTransform().position;

                OpenSequence(playerPosition);
            }
            else
            {
                CloseSequence();
            }
        }
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

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y + RotatingAmount, StartRotation.z));
        }
        else 
        {
            endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y - RotatingAmount, StartRotation.z));
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