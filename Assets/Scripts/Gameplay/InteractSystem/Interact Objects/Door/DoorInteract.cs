using DependencyInjection;
using System.Collections;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string doorText;
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;
    [SerializeField] private float doorSpeed;
    [SerializeField] private float doorDistance;
    [SerializeField] private float closeDoorsAfterTime;

    [SerializeField] private Vector3 doorLeftMovement = Vector3.forward;
    [SerializeField] private Vector3 doorRightMovement = Vector3.back;

    [SerializeField] DoorInteract connectedDoor;

    private Vector3 doorLeftPosOpen, doorRightPosOpen;
    private Vector3 doorLeftClosed, doorRightClosed;
    private bool isOpen;
    private bool isMoving;

    private float closeDelayInitial;
    private float closeTimer;

    [SerializeField] SoundData openDoorSound;

    [SerializeField] Animator doorLeftAnimator;
    [SerializeField] Animator doorRightAnimator;
    string openTrigger = "Open";
    string closeTrigger = "Close";

    [SerializeField] TeleportLoop tp;

    private Coroutine autoCloseCoroutine;

    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    void Start()
    {
        if (doorLeft != null)
        {
            doorLeftClosed = doorLeft.localPosition;
            doorLeftPosOpen = doorLeftClosed + (doorLeftMovement.normalized * doorDistance);
        }

        if (doorRight != null)
        {
            doorRightClosed = doorRight.localPosition;
            doorRightPosOpen = doorRightClosed + (doorRightMovement.normalized * doorDistance);
        }

        closeDelayInitial = closeDoorsAfterTime;
        closeTimer = closeDelayInitial;

        // Disable the parent's trigger collider if it exists (optional cleanup)
        Collider parentCollider = GetComponent<Collider>();
        if (parentCollider != null && parentCollider.isTrigger)
        {
            parentCollider.enabled = false;
        }
    }
    public string GetInteractText() => doorText;
    public void Interact()
    {
        if (GameManager.Instance.GetCondition(GameCondition.LOOP4))
        {
            monologueSpeaker.StartMonologue(Events.ClosedDoorsTrain);
            return;
        }

        if (isMoving || isOpen) return;

        if (tp != null)
        {
            tp.Teleport();

            if (connectedDoor != null)
            {
                connectedDoor.OpenDoors();
            }
        }

        if (connectedDoor == null)
        {
            OpenDoors();
        }
    }
    public void OpenDoors()
    {
        //StopAllCoroutines();
        //closeTimer = closeDelayInitial;
        StopActiveMovement();
        StartCoroutine(MoveDoors(true));
    }
    public void CloseDoors()
    {
        if (isMoving || !isOpen) return;
        //topAllCoroutines();
        StopActiveMovement();
        StartCoroutine(MoveDoors(false));
    }
    void StopActiveMovement()
    {
        StopAllCoroutines();
        autoCloseCoroutine = null;
        isMoving = false;
    }
    //private void Update()
    //{
    //    if (isOpen && !isMoving)
    //    {
    //        closeTimer -= Time.deltaTime;
    //        if (closeTimer <= 0f)
    //            CloseDoors();
    //    }
    //}
    private IEnumerator MoveDoors(bool opening)
    {
        isMoving = true;

        SoundManager.Instance.PlayQuickSound(openDoorSound);

        if (doorLeftAnimator != null)
            doorLeftAnimator.SetTrigger(opening ? openTrigger : closeTrigger);

        if (doorRightAnimator != null)
            doorRightAnimator.SetTrigger(opening ? openTrigger : closeTrigger);

        Vector3 leftTarget = opening ? doorLeftPosOpen : doorLeftClosed;
        Vector3 rightTarget = opening ? doorRightPosOpen : doorRightClosed;

        while (true)
        {
            bool doneLeft = true, doneRight = true;

            if (doorLeft != null)
            {
                doorLeft.localPosition = Vector3.Lerp(doorLeft.localPosition, leftTarget, Time.deltaTime * doorSpeed);
                doneLeft = Vector3.Distance(doorLeft.localPosition, leftTarget) < 0.01f;
            }

            if (doorRight != null)
            {
                doorRight.localPosition = Vector3.Lerp(doorRight.localPosition, rightTarget, Time.deltaTime * doorSpeed);
                doneRight = Vector3.Distance(doorRight.localPosition, rightTarget) < 0.01f;
            }

            if (doneLeft && doneRight) break;
            yield return null;
        }

        if (doorLeft != null) doorLeft.localPosition = opening ? doorLeftPosOpen : doorLeftClosed;
        if (doorRight != null) doorRight.localPosition = opening ? doorRightPosOpen : doorRightClosed;

        isOpen = opening;
        isMoving = false;

        if (isOpen)
        {
            autoCloseCoroutine = StartCoroutine(WaitAndClose());
        }

        //if (isOpen) closeTimer = closeDelayInitial;
    }
    IEnumerator WaitAndClose()
    {
        yield return new WaitForSeconds(closeDoorsAfterTime);
        CloseDoors();
    }
}