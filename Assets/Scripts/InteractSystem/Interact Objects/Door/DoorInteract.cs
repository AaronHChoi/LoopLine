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

    private AudioSource openDoor;
    [SerializeField] private Vector3 doorLeftMovement = Vector3.forward;
    [SerializeField] private Vector3 doorRightMovement = Vector3.back;

    private Vector3 doorLeftPosOpen, doorRightPosOpen;
    private Vector3 doorLeftClosed, doorRightClosed;
    private bool isOpen;
    private bool isMoving;

    private float closeDelayInitial;
    private float closeTimer;

    private void Awake()
    {
        openDoor = GetComponent<AudioSource>();
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
    }

    public string GetInteractText() => doorText;

    public void Interact()
    {
        if (isMoving || isOpen) return;
        OpenDoors();
    }

    private void OpenDoors()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoors(true));
    }

    private void CloseDoors()
    {
        if (isMoving || !isOpen) return;
        StopAllCoroutines();
        StartCoroutine(MoveDoors(false));
    }

    private void Update()
    {
        if (isOpen && !isMoving)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f)
                CloseDoors();
        }
    }

    private IEnumerator MoveDoors(bool opening)
    {
        isMoving = true;
        if (openDoor) openDoor.Play();

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
        if (isOpen) closeTimer = closeDelayInitial;
    }
}
