using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class FocusModeState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        CinemachinePOVExtension playerCamera;

        public FocusModeState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, CinemachinePOVExtension playerCamera)
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
            Debug.Log("Entering FocusModeState");
        }
        public void Execute()
        {
            if (input.FocusModePressed())
            {
                controller.UseEventFocusMode();
                controller.ChangeState(controller.NormalState);
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting FocusModeState");
        }
    }
}