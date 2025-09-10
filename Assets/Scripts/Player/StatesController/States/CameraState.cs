using InWorldUI;
using UI;
using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class CameraState : IState
    {
        public static bool PolaroidIsActive { get; private set; }
        PlayerStateController controller;
        IPlayerInputHandler input;
        PlayerMovement movement;
        PhotoCapture photo;
        CinemachinePOVExtension playerCamera;
        PlayerInteraction interaction;
        PhotoMarker photoMarker;
        ITogglePhotoDetection togglePhotoDetection;
        GameObject polaroidItem;
        public CameraState(PlayerStateController controller, IPlayerInputHandler input, PlayerMovement movement, 
            PhotoCapture photo, CinemachinePOVExtension playerCamera, PlayerInteraction interaction, 
            ITogglePhotoDetection togglePhotoDetection, PhotoMarker photoMarker)
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
            interaction.SetInteractableDetection(false);
            movement.CanMove = true;
            photoMarker.enabled = true;
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
            interaction.SetInteractableDetection(true);
            movement.CanMove = false;
            photoMarker.enabled = false;
            playerCamera.CanLook = false;
            togglePhotoDetection.ToggleCollider(false);
            polaroidItem.SetActive(true);
            PolaroidIsActive = false;
            Debug.Log("Exiting CameraState");
        }
    }
}