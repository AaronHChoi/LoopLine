using UnityEngine;

public class DoorHandler : MonoBehaviour
{ 
    Animator doorHandlerAnimator;
    SingleDoorInteract doorInteract;
    [SerializeField] bool leftSide;
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
        }
        else
        {
            doorHandlerAnimator.SetTrigger("Open");
        }
    }
    public void CloseAnimation()
    {
        if (leftSide)
        {
            doorHandlerAnimator.SetTrigger("Close2");
        }
        else
        {
            doorHandlerAnimator.SetTrigger("Close");
        }
    }
}