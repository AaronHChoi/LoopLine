using DependencyInjection;
using System.Collections;
using UnityEngine;

public class SingleDoorInteract : MonoBehaviour, IInteract
{
    public bool isOpen = false;

    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private string doorText;
    [SerializeField] private bool IsRootatingDoor = true;
    [SerializeField] private float Speed = 1f;
    [SerializeField] private float AutoCloseDelay = 3f;

    private Vector3 playerPosition;

    [Header("Rotating Config")]
    [SerializeField] private float RotatingAmount = 90f;
    [SerializeField] private float ForwardDirection = 0f;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCorutine;
    private Coroutine AutoCloseCoroutine;

    IPlayerController playerController;

    private void Awake()
    {
        playerController = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        doorGameObject = transform.parent.gameObject;
        StartRotation = transform.rotation.eulerAngles;
        Forward = doorGameObject.transform.forward; //this is because the forward of the door is orienteted to the right if the forwar chages chage this line
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
                Debug.Log(dot);
                //if (AutoCloseCoroutine != null)
                //    StopCoroutine(AutoCloseCoroutine);

                //AutoCloseCoroutine = StartCoroutine(CloseDoorAfterTime(AutoCloseDelay));
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
        if (!isOpen)
        {   
            playerPosition = playerController.GetTransform().position;
            OpenDoor(playerPosition);
            Debug.Log(playerPosition);
        }
        else
        {
            CloseDoor();
        }
    }
    public string GetInteractText()
    {
        return doorText;
    }
    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = doorGameObject.transform.rotation;
        Quaternion endRotation;

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotatingAmount, 0));
        }
        else 
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotatingAmount, 0));
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
    private IEnumerator CloseDoorAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isOpen)
        {
            CloseDoor();
        }
    }
}