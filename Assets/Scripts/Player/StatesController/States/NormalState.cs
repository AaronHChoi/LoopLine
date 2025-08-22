using UnityEngine;

namespace Player
{
    public class NormalState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public NormalState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = true;
            Debug.Log("Entering NormalState");
        }
        public void Execute()
        {
            if (input.ToggleCameraPressed() && PlayerInventorySystem.Instance.ItemInUse.id == "Camera")
            {
                controller.stateMachine.TransitionTo(controller.CameraState);
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
                controller.stateMachine.TransitionTo(controller.InventoryState);
            }
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.stateMachine.TransitionTo(controller.DevelopmentState);
            }
            if (input.FocusModePressed())
            {
                controller.UseEventFocusMode();
                controller.stateMachine.TransitionTo(controller.FocusModeState);
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            Debug.Log("Exiting NormalState");
        }
    }
}