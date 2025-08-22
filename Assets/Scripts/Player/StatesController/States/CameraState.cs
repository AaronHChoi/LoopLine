using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        PhotoCapture photo;

        public CameraState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, PhotoCapture photo)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
        }
        public void Enter()
        {
            movement.CanMove = true;
            Debug.Log("Entering CameraState");
        }
        public void Execute()
        {
            if (input.ToggleCameraPressed())
            {
                if (!photo.IsViewingPhoto)
                {
                    controller.ChangeState(controller.NormalState);
                }
            }
            if (input.TakePhotoPressed())
            {
                controller.UseEventTakePhoto();
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            Debug.Log("Exiting CameraState");
        }
    }
}