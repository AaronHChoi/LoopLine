using InWorldUI;
using UI;
using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        public static bool PolaroidIsActive { get; private set; }
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        IPhotoCapture photo;
        ICameraOrientation playerCamera;
        IPlayerInteractMarkerPrompt interaction;
        ITogglePhotoDetection togglePhotoDetection;
        GameObject polaroidItem;
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

            if (polaroidItem == null)
            {
                polaroidItem = GameObject.FindWithTag("PolaroidItem");
            }
            polaroidItem.SetActive(false);

            PolaroidIsActive = true;
            Debug.Log("Entering CameraState");
        }
        public void Execute()
        {
            if (input.ToggleCameraPressed())
            {
                if (!photo.IsViewingPhoto)
                {
                    controller.ChangeState(controller.ObjectInHandState);
                }
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
            polaroidItem.SetActive(true);
            PolaroidIsActive = false;
            Debug.Log("Exiting CameraState");
        }
    }
}