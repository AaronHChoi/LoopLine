using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;

    private PlayerView playerView;
    private PlayerModel playerModel;
    private InputAction moveAction;
    private CinemachineCamera virtualCamera;
    private Transform cameraTransform; 

    private Vector2 inputMovement;

    private void Awake()
    {
        playerView = GetComponent<PlayerView>();
        playerModel = new PlayerModel();
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        cameraTransform = virtualCamera.transform;
    }
    private void Update()
    {
        HandleMovement();
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
    public void SetControllerEnabled(bool enabled)
    {
        CanMove = enabled;
        virtualCamera.enabled = enabled;
    }
}
