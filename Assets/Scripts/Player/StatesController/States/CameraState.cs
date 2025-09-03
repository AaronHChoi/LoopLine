using InWorldUI;
using UI;
using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        PhotoCapture photo;
        PlayerInteraction interaction;
        PhotoMarker photoMarker;
        public CameraState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, 
            PhotoCapture photo, PlayerInteraction interaction,  PhotoMarker photoMarker)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
            this.interaction = interaction;
            this.photoMarker = photoMarker;
        }
        public void Enter()
        {
            interaction.SetInteractableDetection(false);
            movement.CanMove = true;
            photoMarker.enabled = true;
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
            photoMarker.enabled = false;
            Debug.Log("Exiting CameraState");
        }
    }
}