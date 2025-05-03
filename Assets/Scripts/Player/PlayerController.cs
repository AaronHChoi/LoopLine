using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool CanListenToConversations => playerModel.CanListenToConversations;

    private PlayerView playerView;
    private PlayerModel playerModel;
    private InputAction moveAction;
    private CinemachineCamera virtualCamera;
    private Transform cameraTransform;
    private PlayerInput playerInput;
    [SerializeField] private CinemachinePOVExtension cinemachinePOVExtension;

    private Vector2 inputMovement;
    private float lockedPanValue;
    private float lockedTiltValue;

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
        playerInput = GetComponent<PlayerInput>();
        virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        cinemachinePOVExtension = FindFirstObjectByType<CinemachinePOVExtension>();
        playerModel = new PlayerModel();
    }
    private void Start()
    {
        moveAction = playerInput.actions["Move"];
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
            ToggleCanListening();
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
    public void ToggleCanListening()
    {
        playerModel.CanListenToConversations = !playerModel.CanListenToConversations;
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