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
    private InputAction moveAction;
    public CinemachineCamera virtualCamera;
    public CinemachineCamera virtualCamera2;
    private Transform cameraTransform;
    private PlayerInput playerInput;
    [SerializeField] private CinemachinePOVExtension cinemachinePOVExtension;
    [SerializeField] FocusModeManager focusModeManager;

    private Vector2 inputMovement;
    [SerializeField] private float lockedPanValue;
    [SerializeField] private float lockedTiltValue;

    public Transform npcFocusTarget;
    public Transform head;
    public float cameraFocusSpeed = 5f;

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
        playerInput = GetComponent<PlayerInput>();
        virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        cinemachinePOVExtension = FindFirstObjectByType<CinemachinePOVExtension>();
        focusModeManager = FindFirstObjectByType<FocusModeManager>();
        playerModel = new PlayerModel();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
        cameraTransform = virtualCamera.transform;
    }
    private void Update()
    {
        HandleMovement();
        RotateCharacterToCamera();
        //Test
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleFocusMode();
            //main.MethodFocusCameraOnNPC();
        }

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

        if (cinemachinePOVExtension != null)
        {
            if (enabled)
            {
                SetPanAndTiltData();
            }
            else
            {
                GetPanAndTiltData();
            }
        }
    }
    public void SetPanAndTiltData()
    {
        cinemachinePOVExtension.SetPanAndTilt(lockedPanValue, lockedTiltValue);
    }
    public void GetPanAndTiltData()
    {
        (lockedPanValue, lockedTiltValue) = cinemachinePOVExtension.GetPanAndTilt();
        Debug.Log(lockedPanValue);
        Debug.Log(lockedTiltValue);
    }
}