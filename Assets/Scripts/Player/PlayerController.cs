using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;

    private PlayerView playerView;
    private PlayerModel playerModel;
    private InputAction moveAction;
    private InputAction lookAction;
    private CinemachineCamera virtualCamera;
    private Transform cameraTransform;
    private CinemachinePanTilt panTilt;
    private PlayerInput playerInput;

    private Vector2 inputMovement;
    private Vector2 inputLook;
    private float lockedPanValue;
    private float lockedTiltValue;

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
        playerInput = GetComponent<PlayerInput>();
        virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        panTilt = FindFirstObjectByType<CinemachinePanTilt>();
        playerModel = new PlayerModel();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        cameraTransform = virtualCamera.transform;
        //cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        HandleMovement();
        RotateCharacterToCamera();
    }
    public void HandleMovement()
    {
        if (!CanMove) return;
        inputMovement = moveAction.ReadValue<Vector2>();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * inputMovement.y + right * inputMovement.x;
        moveDirection *= playerModel.Speed;

        playerView.Move(moveDirection * Time.deltaTime);
    }
    private void RotateCharacterToCamera()
    {
        if (!CanMove) return;

        float targetAngle = cameraTransform.eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerModel.SpeedRotation);

    }
    public void SetControllerEnabled(bool enabled)
    {
        CanMove = enabled;
        virtualCamera.enabled = enabled;

        if(panTilt != null)
        {
            if (enabled)
            {
                panTilt.PanAxis.Value = lockedPanValue;
                panTilt.TiltAxis.Value = lockedTiltValue;
            }
            else
            {
                lockedPanValue = panTilt.PanAxis.Value;
                lockedTiltValue = panTilt.TiltAxis.Value;
            }
        }
    }
}