using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class ObjectInHandState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;

        public ObjectInHandState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            Debug.Log("Entering ObjectInHandState");
        }
        public void Execute()
        {
            if (input.ToggleCameraPressed() && controller.IsInState(controller.ObjectInHandState) && InventoryUI.Instance.ItemInUse.id == "Camera")
            {
                controller.ChangeState(controller.CameraState);
            }
            if (input.OpenInventoryPressed())
            {
                controller.UseEventOpenInventory();
            }
            if (input.InteractPressed())
            {
                controller.UseEventInteract();
            }
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.StateMachine.TransitionTo(controller.DevelopmentState);
            }
            if (input.Teleport())
            {
                controller.UseEventTeleport();
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting ObjectInHandState");
        }
    }
}