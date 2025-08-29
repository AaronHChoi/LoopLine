using UnityEngine;
using Unity.Cinemachine.Samples;

namespace Player
{
    public class DevelopmentState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        CinemachinePOVExtension playerCamera;
        public DevelopmentState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, CinemachinePOVExtension playerCamera)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting DevelopmentState");
        }
        public void Execute()
        {
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.ChangeState(controller.NormalState);
            }
        }
        public void Exit()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            Debug.Log("Exiting DevelopmentState");
        }
    }
}