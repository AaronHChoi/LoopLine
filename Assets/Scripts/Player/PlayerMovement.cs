using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool canMove = true;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }
    public void HandleMovement(float _speedInputMovement, Vector2 _inputMovement, PlayerInputHandler _playerInputHandler, PlayerCamera _playerCamera, PlayerModel _playerModel, PlayerView _playerView)
    {
        if (!canMove) return;
        _inputMovement = _playerInputHandler.GetMoveAction().ReadValue<Vector2>();
        _speedInputMovement = _playerInputHandler.GetSprintAction().ReadValue<float>();

        Vector3 forward = _playerCamera.GetCameraTransform().forward;
        Vector3 right = _playerCamera.GetCameraTransform().right;

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

        _playerView.Move(moveDirection * Time.deltaTime);
    }
    public void RotateCharacterToCamera(PlayerCamera _playerCamera, PlayerModel _playerModel)
    {
        if (!canMove) return;

        float targetAngle = _playerCamera.GetCameraTransform().eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _playerModel.SpeedRotation);
    }
}
