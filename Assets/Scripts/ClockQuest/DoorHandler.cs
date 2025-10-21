using UnityEngine;

public class DoorHandler : MonoBehaviour
{ 
    Animator doorHandlerAnimator;
    SingleDoorInteract doorInteract;
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
        doorHandlerAnimator.SetTrigger("Open");
    }
    public void CloseAnimation()
    {
        doorHandlerAnimator.SetTrigger("Close");
    }
}