using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IPlayerMovementInput
{
    PlayerInput playerInput;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction toggleCameraAction;
    InputAction takePhotoAction;
    InputAction interact;
    InputAction passDialog;
    //InputAction skipDialogue;
    InputAction openInventory;
    InputAction developmentMode;
    InputAction focusMode;
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
        interact = playerInput.actions["Interact1"];
        passDialog = playerInput.actions["PassDialog"];
        //skipDialogue = playerInput.actions["SkipDialogueTyping"];
        openInventory = playerInput.actions["OpenInventory"];
        developmentMode = playerInput.actions["DevelopmentMode"];
        focusMode = playerInput.actions["FocusMode"];
    }
    public Vector2 GetInputMove()
    {
        return moveAction.ReadValue<Vector2>();
    }
    public bool IsSprinting()
    {
        return sprintAction.IsPressed();
    }
    public bool InteractPressed()
    {
        return interact.WasPerformedThisFrame();
    }
    public bool PassDialogPressed()
    {
        return passDialog.WasPerformedThisFrame();
    }
    //public bool SkipDialogueTyping()
    //{
    //    return skipDialogue.WasPerformedThisFrame();
    //}
    public bool FocusModePressed()
    {
        return focusMode.WasPerformedThisFrame();
    }
    public bool OpenInventoryPressed()
    {
        return openInventory.WasPerformedThisFrame();
    }
    public bool ToggleCameraPressed()
    {
        return toggleCameraAction.WasPerformedThisFrame();
    }
    public bool TakePhotoPressed()
    {
        return takePhotoAction.WasPerformedThisFrame();
    }
    public bool DevelopmentModePressed()
    {
        return developmentMode.WasPerformedThisFrame();
    }
}
public interface IPlayerMovementInput
{
    Vector2 GetInputMove();
    bool IsSprinting();
}