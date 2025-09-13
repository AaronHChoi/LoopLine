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


    IPlayerController controller;
    IPlayerInputHandler input;
    IPlayerCamera playerCamera;
    IPlayerView playerView;
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

        if (input.IsSprinting())
        {
            moveDirection *= controller.PlayerModel.SprintSpeed;
        }
        else
        {
            moveDirection *= controller.PlayerModel.Speed;
        }

        playerView.Move(moveDirection * Time.deltaTime);
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