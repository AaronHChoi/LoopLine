using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool canMove = true;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }
    IPlayerInputHandler playerInputHandler;
    IPlayerCamera playerCamera;
    IPlayerView playerView;
    private void Awake()
    {
        playerInputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>();
        playerView = InterfaceDependencyInjector.Instance.Resolve<IPlayerView>();
    }
    public void HandleMovement(PlayerModel _playerModel)
    {
        if (!canMove) return;

        Vector2 _inputMovement = playerInputHandler.GetMoveAction().ReadValue<Vector2>();
        float _speedInputMovement = playerInputHandler.GetSprintAction().ReadValue<float>();

        Vector3 forward = playerCamera.GetCameraTransform().forward;
        Vector3 right = playerCamera.GetCameraTransform().right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _inputMovement.y + right * _inputMovement.x;

        if (_speedInputMovement > 0.1f)
        {
            moveDirection *= _playerModel.SprintSpeed;
        }
        else
        {
            moveDirection *= _playerModel.Speed;
        }

        playerView.Move(moveDirection * Time.deltaTime);
    }
    public void RotateCharacterToCamera(PlayerModel _playerModel)
    {
        if (!canMove) return;

        float targetAngle = playerCamera.GetCameraTransform().eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _playerModel.SpeedRotation);
    }
}
