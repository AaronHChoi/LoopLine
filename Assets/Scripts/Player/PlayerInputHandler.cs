using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
    }
    public InputAction GetMoveAction()
    {
        return moveAction;
    }
    public InputAction GetSprintAction()
    {
        return sprintAction;
    }
}
