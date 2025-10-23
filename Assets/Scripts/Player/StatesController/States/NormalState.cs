using System.Threading;
using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class NormalState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;

        public NormalState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera)
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
                controller.StateMachine.TransitionTo(controller.DevelopmentState);
            }
            if (input.PauseMenuModePressed())
            {
                //controller.UseEventPauseMenu();
                GameManager.Instance.screenManager.Push(EnumScreenName.Pause);
                controller.StateMachine.TransitionTo(controller.PauseMenuState);
            }
            if (input.FocusModePressed())
            {
                controller.UseEventFocusMode();
                controller.StateMachine.TransitionTo(controller.FocusModeState);
            }
            if (input.TeleportToMindplace())
            {
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