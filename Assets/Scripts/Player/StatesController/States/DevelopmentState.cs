using UnityEngine;
using Unity.Cinemachine.Samples;

namespace Player
{
    public class DevelopmentState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;
        ITimeProvider timeManager;
        public DevelopmentState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera, ITimeProvider timeManager)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCamera = playerCamera;
            this.timeManager = timeManager;
        }
        public void Enter()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            timeManager.PauseTime(true);
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
            timeManager.PauseTime(false);
            Debug.Log("Exiting DevelopmentState");
        }
    }
}