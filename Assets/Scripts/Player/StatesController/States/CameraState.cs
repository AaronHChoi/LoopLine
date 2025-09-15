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
        IPhotoMarker photoMarker;
        ITogglePhotoDetection togglePhotoDetection;
        GameObject polaroidItem;
        public CameraState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, 
            IPhotoCapture photo, ICameraOrientation playerCamera, IPlayerInteractMarkerPrompt interaction, 
            ITogglePhotoDetection togglePhotoDetection, IPhotoMarker photoMarker)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.photo = photo;
            this.playerCamera = playerCamera;
            this.interaction = interaction;
            this.photoMarker = photoMarker;
            this.togglePhotoDetection = togglePhotoDetection;
        }
        public void Enter()
        {
            interaction.IsDetecting = false;
            movement.CanMove = true;
            photoMarker.SetGameObjectEnable(true);
            playerCamera.CanLook = true;
            togglePhotoDetection.ToggleCollider(true);
            if (polaroidItem == null)
                polaroidItem = GameObject.FindWithTag("PolaroidItem");
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
            photoMarker.SetGameObjectEnable(false);
            playerCamera.CanLook = false;
            togglePhotoDetection.ToggleCollider(false);
            polaroidItem.SetActive(true);
            PolaroidIsActive = false;
            Debug.Log("Exiting CameraState");
        }
    }
}