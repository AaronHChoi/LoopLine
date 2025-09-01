using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        PhotoCapture photo;
        CinemachinePOVExtension playerCamera;

        public CameraState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, PhotoCapture photo, CinemachinePOVExtension playerCamera)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;

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
            playerCamera.CanLook = false;
            Debug.Log("Exiting CameraState");
        }
    }
}