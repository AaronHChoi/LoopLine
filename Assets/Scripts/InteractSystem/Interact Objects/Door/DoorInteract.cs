using System.Collections;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string doorText;
    [SerializeField] private Transform doorLeft, doorRight;
    [SerializeField] private float doorSpeed;
    [SerializeField] private float doorDistance;
    [SerializeField] private float closeDoorsAfterTime;

    private AudioSource openDoor;
    [SerializeField] private Vector3 doorLeftMovement = Vector3.forward;
    [SerializeField] private Vector3 doorRightMovement = Vector3.back;

    private Vector3 doorLeftPosOpen, doorRightPosOpen;
    private Vector3 doorLeftClosed, doorRightClosed;

    private bool isOpen = false;
    private bool isMoving = false;

    private float closeDelayInitial;
    private float closeTimer;

    private void Awake()
    {
        openDoor = GetComponent<AudioSource>();
    }

    void Start()
    {
        doorLeftClosed = doorLeft.position;
        doorRightClosed = doorRight.position;

        doorLeftPosOpen = doorLeftClosed + doorLeftMovement * doorDistance;
        doorRightPosOpen = doorRightClosed + doorRightMovement * doorDistance;

        closeDelayInitial = closeDoorsAfterTime;
        closeTimer = closeDelayInitial;
    }

    public string GetInteractText()
    {
        return doorText;
    }

    public void Interact()
    {
        if (isMoving || isOpen) return;

        OpenDoors();
    }

    private void OpenDoors()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoors(doorLeftPosOpen, doorRightPosOpen, true));
    }

    private void CloseDoors()
    {
        if (isMoving || !isOpen) return;
        StopAllCoroutines();
        StartCoroutine(MoveDoors(doorLeftClosed, doorRightClosed, false));
    }

    private void Update()
    {
        if (isOpen && !isMoving)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f)
            {
                CloseDoors();
            }
        }
    }

    private IEnumerator MoveDoors(Vector3 leftTarget, Vector3 rightTarget, bool resultIsOpen)
    {
        isMoving = true;
        if (openDoor) openDoor.Play();

        while (Vector3.Distance(doorLeft.position, leftTarget) > 0.01f || Vector3.Distance(doorRight.position, rightTarget) > 0.01f)
        {
            doorLeft.position = Vector3.Lerp(doorLeft.position, leftTarget, Time.deltaTime * doorSpeed);
            doorRight.position = Vector3.Lerp(doorRight.position, rightTarget, Time.deltaTime * doorSpeed);
            yield return null;
        }

        doorLeft.position = leftTarget;
        doorRight.position = rightTarget;

        isOpen = resultIsOpen;
        isMoving = false;

        if (isOpen)
        {
            closeTimer = closeDelayInitial;
        }
    }
}
