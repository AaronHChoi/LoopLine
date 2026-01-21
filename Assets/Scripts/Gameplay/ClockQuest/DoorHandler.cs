using UnityEngine;

public class DoorHandler : MonoBehaviour
{ 
    Animator doorHandlerAnimator;
    SingleDoorInteract doorInteract;
    [SerializeField] bool leftSide;
    [SerializeField] EventsID openDoorSoundEventID;
    [SerializeField] EventsID closeDoorSoundEventID;

    private void Awake()
    {
        doorHandlerAnimator = GetComponent<Animator>();
        doorInteract = GetComponentInParent<SingleDoorInteract>();
    }
    private void OnEnable()
    {
        if (doorInteract != null)
        {
            doorInteract.OnDoorOpened += OpenAnimation;
            doorInteract.OnDoorClosed += CloseAnimation;
        }
    }
    private void OnDisable()
    {
        if (doorInteract != null)
        {
            doorInteract.OnDoorOpened -= OpenAnimation;
            doorInteract.OnDoorClosed -= CloseAnimation;
        }
    }
    public void OpenAnimation()
    {
        if (leftSide)
        {
            doorHandlerAnimator.SetTrigger("Open2");
            DelayUtility.Instance.Delay(0.75f, () => EventBus.Publish(new DoorEvent { SoundID = openDoorSoundEventID, ShouldPlay = true }));
        }
        else
        {
            doorHandlerAnimator.SetTrigger("Open");
            DelayUtility.Instance.Delay(0.75f, () => EventBus.Publish(new DoorEvent { SoundID = openDoorSoundEventID, ShouldPlay = true }));
        }
    }
    public void CloseAnimation()
    {
        if (leftSide)
        {
            doorHandlerAnimator.SetTrigger("Close2");
            EventBus.Publish(new DoorEvent { SoundID = closeDoorSoundEventID, ShouldPlay = true });
        }
        else
        {
            doorHandlerAnimator.SetTrigger("Close");
            EventBus.Publish(new DoorEvent { SoundID = closeDoorSoundEventID, ShouldPlay = true });
        }
    }
}