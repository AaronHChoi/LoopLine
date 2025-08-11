using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IPlayerInputHandler
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
        exitPhotoAction = playerInput.actions["ExitPhoto"];
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
    public bool ExitPhotoPressed()
    {
        return exitPhotoAction.WasPerformedThisFrame();
    }
}
public interface IPlayerInputHandler
{
    Vector2 GetInputMove();
    bool IsSprinting();
    bool ToggleCameraPressed();
    bool TakePhotoPressed();
    bool ExitPhotoPressed();
}