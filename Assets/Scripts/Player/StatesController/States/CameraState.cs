using InWorldUI;
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
        PlayerInteraction interaction;
        ITogglePhotoDetection togglePhotoDetection;
        public CameraState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, PhotoCapture photo, CinemachinePOVExtension playerCamera, PlayerInteraction interaction, ITogglePhotoDetection togglePhotoDetection)
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
            interaction.SetInteractableDetection(false);
            movement.CanMove = true;
            playerCamera.CanLook = true;
            togglePhotoDetection.ToggleCollider(true);
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
            interaction.SetInteractableDetection(true);
            movement.CanMove = false;
            playerCamera.CanLook = false;
            togglePhotoDetection.ToggleCollider(false);
            Debug.Log("Exiting CameraState");
        }
    }
}