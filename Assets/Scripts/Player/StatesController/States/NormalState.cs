using UnityEngine;

namespace Player
{
    public class NormalState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;
        IUIManager uiManager;
        public NormalState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera, IUIManager uiManager)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCamera = playerCamera;
            this.uiManager = uiManager;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            Debug.Log("Entering NormalState");
        }
        public void Execute()
        {
            if (GameManager.Instance.GetCondition(GameCondition.PolaroidTaken) && input.ToggleCameraPressed())
            {
                controller.StateMachine.TransitionTo(controller.CameraState);
            }
            if (input.InteractPressed())
            {
                controller.UseEventInteract();
            }
            if (input.GrabItemPressed())
            {
                controller.UseEventGrab();
            }
            if (input.OpenInventoryPressed())
            {
                controller.UseEventOpenInventory();
            }
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.StateMachine.TransitionTo(controller.DevelopmentState);
            }
            if (input.Teleport())
            {
                uiManager.HideCurrentPanel();
                controller.UseEventTeleport();
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting NormalState");
        }
    }
}