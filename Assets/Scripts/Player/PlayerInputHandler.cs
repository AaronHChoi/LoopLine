using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IPolaroidCameraInput, IPlayerMovementInput
{
    PlayerInput playerInput;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction toggleCameraAction;
    InputAction takePhotoAction;
    InputAction exitPhotoAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        toggleCameraAction = playerInput.actions["ToggleCamera"];
        takePhotoAction = playerInput.actions["TakePhoto"];
    }
    public Vector2 GetInputMove()
    {
        return moveAction.ReadValue<Vector2>();
    }
    public bool IsSprinting()
    {
        return sprintAction.IsPressed();
    }
    public bool ToggleCameraPressed()
    {
        return toggleCameraAction.WasPerformedThisFrame();
    }
    public bool TakePhotoPressed()
    {
        return takePhotoAction.WasPerformedThisFrame();
    }
}
public interface IPlayerMovementInput
{
    Vector2 GetInputMove();
    bool IsSprinting();
}
public interface IPolaroidCameraInput
{
    bool ToggleCameraPressed();
    bool TakePhotoPressed();
}