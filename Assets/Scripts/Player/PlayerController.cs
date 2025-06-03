
using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool FocusMode => playerModel.FocusMode;

    private PlayerView playerView;
    private PlayerModel playerModel;
  
    public Transform HeadPosition;
    private Vector2 inputMovement;
    private float speedInputMovement;
    
    PlayerInputHandler playerInputHandler;
    PlayerCamera playerCamera;
    PlayerFocusMode playerFocusMode;
    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        playerCamera = GetComponent<PlayerCamera>();
        playerView = GetComponent<PlayerView>();
        playerFocusMode = GetComponent<PlayerFocusMode>();

        playerModel = new PlayerModel();
    }
    private void Update()
    {
        HandleMovement();
        RotateCharacterToCamera();

        //Test
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerFocusMode.ToggleFocusMode(playerModel);
        }
    }
    public void HandleMovement()
    {
        if (!CanMove) return;
        inputMovement = playerInputHandler.GetMoveAction().ReadValue<Vector2>();
        speedInputMovement = playerInputHandler.GetSprintAction().ReadValue<float>();

        Vector3 forward = playerCamera.GetCameraTransform().forward;
        Vector3 right = playerCamera.GetCameraTransform().right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * inputMovement.y + right * inputMovement.x;

        if (speedInputMovement > 0.1f)
        {
            moveDirection *= playerModel.SprintSpeed;         
        }
        else
        {   
            moveDirection *= playerModel.Speed;
        }

        playerView.Move(moveDirection * Time.deltaTime);
    }
    private void RotateCharacterToCamera()
    {
        if (!CanMove) return;

        float targetAngle = playerCamera.GetCameraTransform().eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerModel.SpeedRotation);
    }
    public void SetCinemachineController(bool _enabled)
    {
        playerCamera.SetControllerEnabled(_enabled, CanMove);
    }
}