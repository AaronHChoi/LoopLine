using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IPlayerInputHandler
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
    InputAction scrollInventory;
    InputAction grabItem;
    InputAction look;
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
        scrollInventory = playerInput.actions["ScrollInventory"];
        grabItem = playerInput.actions["GrabItem"];
        look = playerInput.actions["Look"];
    }
    public Vector2 GetInputMove()
    {
        return moveAction.ReadValue<Vector2>();
    }
    public Vector2 GetInputDelta()
    {
        return look.ReadValue<Vector2>();
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
    public float GetScrollValue()
    {
        return scrollInventory.ReadValue<Vector2>().y;
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
    public bool GrabItemPressed()
    {
        return grabItem.WasPerformedThisFrame();
    }
}
public interface IPlayerInputHandler
{
    float GetScrollValue();
    Vector2 GetInputMove();
    bool IsSprinting();
}