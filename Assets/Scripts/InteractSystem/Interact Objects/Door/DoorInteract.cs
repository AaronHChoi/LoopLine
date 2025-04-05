using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string doorText = "open door";
    [SerializeField] private Transform doorLeft, doorRight;
    [SerializeField] private float doorSpeed = 2f;
    [SerializeField] private float doorDistance = 2f;

    [SerializeField] private Vector3 doorLeftMovement = Vector3.forward;
    [SerializeField] private Vector3 doorRightMovement = Vector3.back;

    private Vector3 doorLeftPosOpen, doorRightPosOpen;
    private Vector3 doorLeftClosed, doorRightClosed;
    private bool isOpen = false;
    void Start()
    {
        doorLeftClosed = doorLeft.position;
        doorRightClosed = doorRight.position;

        doorLeftPosOpen = doorLeftClosed + doorLeftMovement * doorDistance;
        doorRightPosOpen = doorRightClosed + doorRightMovement * doorDistance;
    }

    public string GetInteractText()
    {
        return doorText;
    }

    public void Interact()
    {
        ToggleDoors();  
    }

    private void ToggleDoors()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoors(isOpen ? doorLeftClosed : doorLeftPosOpen, isOpen ? doorRightClosed : doorRightPosOpen));
        isOpen = !isOpen;
    }

    private System.Collections.IEnumerator MoveDoors(Vector3 leftTarget, Vector3 rightTarget)
    {
        while (Vector3.Distance(doorLeft.position, leftTarget) > 0.01f || Vector3.Distance(doorRight.position, rightTarget) > 0.01f)
        {
            doorLeft.position = Vector3.Lerp(doorLeft.position, leftTarget, Time.deltaTime * doorSpeed);
            doorRight.position = Vector3.Lerp(doorRight.position, rightTarget, Time.deltaTime * doorSpeed);
            yield return null;
        }

        doorLeft.position = leftTarget;
        doorRight.position = rightTarget;
    }
}
