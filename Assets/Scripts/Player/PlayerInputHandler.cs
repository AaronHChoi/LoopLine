using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IPlayerInputHandler
{
    PlayerInput playerInput;

    InputAction moveAction;
    InputAction sprintAction;
    #region CAMERA
    InputAction toggleCameraAction;
    InputAction takePhotoAction;
    #endregion
    InputAction interact;
    InputAction passDialog;
    InputAction openInventory;
    InputAction developmentMode;
    InputAction pauseMenuMode;
    InputAction scrollInventory;
    InputAction grabItem;
    InputAction look;
    InputAction teleport;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        #region CAMERA
        toggleCameraAction = playerInput.actions["ToggleCamera"];
        takePhotoAction = playerInput.actions["TakePhoto"];
        #endregion
        interact = playerInput.actions["Interact1"];
        passDialog = playerInput.actions["PassDialog"];
        openInventory = playerInput.actions["OpenInventory"];
        developmentMode = playerInput.actions["DevelopmentMode"];
        pauseMenuMode = playerInput.actions["PauseMenuMode"];
        scrollInventory = playerInput.actions["ScrollInventory"];
        grabItem = playerInput.actions["GrabItem"];
        look = playerInput.actions["Look"];
        teleport = playerInput.actions["Teleport"];
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
    public bool Teleport()
    {
        return teleport.WasPerformedThisFrame();
    }
    public bool InteractPressed()
    {
        return interact.WasPerformedThisFrame();
    }
    public bool PassDialogPressed()
    {
        return passDialog.WasPerformedThisFrame();
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
    public bool PauseMenuModePressed()
    {
        return pauseMenuMode.WasPerformedThisFrame();
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
    Vector2 GetInputDelta();
    bool IsSprinting();
    bool InteractPressed();
    bool PassDialogPressed();
    bool OpenInventoryPressed();
    bool ToggleCameraPressed();
    bool TakePhotoPressed();
    bool DevelopmentModePressed();
    bool PauseMenuModePressed();
    bool GrabItemPressed();
    bool Teleport();
}