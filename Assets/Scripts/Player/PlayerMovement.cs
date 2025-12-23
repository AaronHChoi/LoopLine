using UnityEngine;
using DependencyInjection;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    bool canMove = true;
    public bool CanMove 
    { 
        get => canMove; 
        set => canMove = value; 
    }
    public float walkStepInterval = 0.6f; 
    public float runStepInterval = 0.35f;

    Vector3 lastPosition;
    float distanceAccumulated;

    [Header("Footstep Settings")]
    [SerializeField] float walkStepDistance = 1.8f;
    [SerializeField] float runStepDistance = 2.2f; 
    [SerializeField] float minVelocityThreshold = 0.5f;

    IPlayerController controller;
    IPlayerInputHandler input;
    IPlayerCamera playerCamera;
    IPlayerView playerView;

    readonly PlayerStepEvent stepEvent = new PlayerStepEvent();

    private void Awake()
    {
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        input = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>();
        playerView = InterfaceDependencyInjector.Instance.Resolve<IPlayerView>();
    }
    public void HandleMovement()
    {
        if (!canMove) return;

        Vector2 inputMovement = input.GetInputMove();

        Vector3 forward = playerCamera.GetCameraTransform().forward;
        Vector3 right = playerCamera.GetCameraTransform().right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * inputMovement.y + right * inputMovement.x;

        if (transform.position.y != controller.PlayerModel.YAxisLocation)
        {
            moveDirection.y = (controller.PlayerModel.YAxisLocation - transform.position.y) * 0.9f;
        }

        float currentSpeed = input.IsSprinting() ? controller.PlayerModel.SprintSpeed : controller.PlayerModel.Speed;
        Vector3 velocity = moveDirection * currentSpeed;

        playerView.Move(velocity * Time.deltaTime);

        Vector3 currentPos = transform.position;
        Vector3 horizontalDisplacement = new Vector3(currentPos.x - lastPosition.x, 0, currentPos.z - lastPosition.z);
        float distanceMovedThisFrame = horizontalDisplacement.magnitude;

        float realVelocity = distanceMovedThisFrame / Time.deltaTime;

        if (inputMovement.magnitude > 0.1f && realVelocity > 0.5f)
        {
            distanceAccumulated += distanceMovedThisFrame;

            float threshold = input.IsSprinting() ? runStepInterval : walkStepInterval;

            if (distanceAccumulated >= threshold)
            {
                EventBus.Publish(stepEvent);
                distanceAccumulated = 0f; // Reset
            }
        }
        else
        {
            distanceAccumulated = Mathf.MoveTowards(distanceAccumulated, 0, Time.deltaTime);
        }

        lastPosition = transform.position;
    }
    public void RotateCharacterToCamera()
    {
        if (!canMove) return;

        float targetAngle = playerCamera.GetCameraTransform().eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * controller.PlayerModel.SpeedRotation);
    }
}
public interface IPlayerMovement
{
    Transform transform { get; }
    public bool CanMove { get; set; }
    void HandleMovement();
    void RotateCharacterToCamera();
}