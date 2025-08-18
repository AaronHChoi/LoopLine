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
    }
    public Vector2 GetInputMove()
    {
        return moveAction.ReadValue<Vector2>();
    }
    public bool IsSprinting()
    {
        return sprintAction.IsPressed();
    }
    public bool Interact()
    {
        return interact.WasPerformedThisFrame();
    }
    public bool PassDialog()
    {
        return passDialog.WasPerformedThisFrame();
    }
    //public bool SkipDialogueTyping()
    //{
    //    return skipDialogue.WasPerformedThisFrame();
    //}
    public bool OpenInventory()
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
    public bool DevelopmentMode()
    {
        return developmentMode.WasPerformedThisFrame();
    }
}
public interface IPlayerMovementInput
{
    Vector2 GetInputMove();
    bool IsSprinting();
}