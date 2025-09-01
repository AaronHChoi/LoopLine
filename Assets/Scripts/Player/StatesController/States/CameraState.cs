using InWorldUI;
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

        public CameraState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, PhotoCapture photo, PlayerInteraction interaction)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
            this.interaction = interaction;
        }
        public void Enter()
        {
            interaction.SetInteractableDetection(false);
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
            interaction.SetInteractableDetection(true);
            movement.CanMove = false;
            Debug.Log("Exiting CameraState");
        }
    }
}