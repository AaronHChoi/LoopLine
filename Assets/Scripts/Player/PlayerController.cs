using System.Collections;
using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool FocusMode => playerModel.FocusMode;

    private PlayerView playerView;
    private PlayerModel playerModel;
  
    private CinemachineCamera virtualCamera;
    private Transform cameraTransform;
    [SerializeField] private CinemachinePOVExtension cinemachinePOVExtension;
    [SerializeField] FocusModeManager focusModeManager;

    public Transform HeadPosition;
    private Vector2 inputMovement;
    private float speedInputMovement;
    private float lockedPanValue;
    private float lockedTiltValue;

    PlayerInputHandler playerInputHandler;

    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();

        playerView = GetComponent<PlayerView>();
        virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        cinemachinePOVExtension = FindFirstObjectByType<CinemachinePOVExtension>();
        focusModeManager = FindFirstObjectByType<FocusModeManager>();
        playerModel = new PlayerModel();
    }
    private void Start()
    {
        cameraTransform = virtualCamera.transform;
        //cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        HandleMovement();
        RotateCharacterToCamera();

        //Test
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleFocusMode();
        }
    }
    public void HandleMovement()
    {
        if (!CanMove) return;
        inputMovement = playerInputHandler.GetMoveAction().ReadValue<Vector2>();
        speedInputMovement = playerInputHandler.GetSprintAction().ReadValue<float>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

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

        float targetAngle = cameraTransform.eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerModel.SpeedRotation);
    }
    public void ToggleFocusMode()
    {
        playerModel.FocusMode = !playerModel.FocusMode;

        focusModeManager.ToggleColliders(playerModel.FocusMode);

        Debug.Log(playerModel.FocusMode);
    }
    public void SetControllerEnabled(bool enabled)
    {
        CanMove = enabled;
        virtualCamera.enabled = enabled;

        if(cinemachinePOVExtension != null)
        {
            if (enabled)
            {
                cinemachinePOVExtension.SetPanAndTilt(lockedPanValue, lockedTiltValue);
            }
            else
            {
                (lockedPanValue, lockedTiltValue) = cinemachinePOVExtension.GetPanAndTilt();
            }
        }
    }
}