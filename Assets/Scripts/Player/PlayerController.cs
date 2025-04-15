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

    private Vector2 inputMovement;
    private Vector2 inputLook;
    private float lockedPanValue;
    private float lockedTiltValue;

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
        playerModel = new PlayerModel();
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        panTilt = FindFirstObjectByType<CinemachinePanTilt>();
    }
    private void Start()
    {
        cameraTransform = virtualCamera.transform;
    }
    private void Update()
    {
        HandleMovement();
        HandleLook();
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
    public void HandleLook()
    {
        if (panTilt == null) return;

        inputLook = lookAction.ReadValue<Vector2>();
        panTilt.PanAxis.Value += inputLook.x * playerModel.LookSensitivity * Time.deltaTime;
        panTilt.TiltAxis.Value += inputLook.y * playerModel.LookSensitivity * Time.deltaTime;

        panTilt.TiltAxis.Value = Mathf.Clamp(panTilt.TiltAxis.Value, -30f, 60f);

        float panRotation = panTilt.PanAxis.Value;
        transform.rotation = Quaternion.Euler(0, panRotation, 0);
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