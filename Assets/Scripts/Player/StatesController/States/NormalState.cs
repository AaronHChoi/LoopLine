using System.Threading;
using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class NormalState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        CinemachinePOVExtension playerCamera;

        public NormalState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, CinemachinePOVExtension playerCamera)
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
            Debug.Log("Entering NormalState");
        }
        public void Execute()
        {
            if (!controller.CanUseNormalStateExecute) return;

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
            playerCamera.CanLook = false;
            Debug.Log("Exiting NormalState");
        }
    }
}