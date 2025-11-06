using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        IPhotoCapture photo;
        ICameraOrientation playerCamera;
        IPlayerInteractMarkerPrompt interaction;
        ITogglePhotoDetection togglePhotoDetection;

        public CameraState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, 
            IPhotoCapture photo, ICameraOrientation playerCamera, IPlayerInteractMarkerPrompt interaction, 
            ITogglePhotoDetection togglePhotoDetection)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
            this.playerCamera = playerCamera;
            this.interaction = interaction;
            this.togglePhotoDetection = togglePhotoDetection;
        }
        public void Enter()
        {
            interaction.IsDetecting = false;
            movement.CanMove = true;

            playerCamera.CanLook = true;
            togglePhotoDetection.ToggleCollider(true);

            photo.SetCameraUIVisible(true);

            Debug.Log("Entering CameraState");
        }
        public void Execute()
        {
            if (input.ToggleCameraPressed())
            {
                controller.ChangeState(controller.NormalState);
            }
            if (input.TakePhotoPressed())
            {
                controller.UseEventTakePhoto();
            }
        }
        public void Exit()
        {
            interaction.IsDetecting = true;
            movement.CanMove = false;
            playerCamera.CanLook = false;
            togglePhotoDetection.ToggleCollider(false);
            photo.SetCameraUIVisible(false);
            Debug.Log("Exiting CameraState");
        }
    }
}