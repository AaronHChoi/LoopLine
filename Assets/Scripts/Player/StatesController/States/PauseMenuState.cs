
using Player;
using UnityEngine;

namespace Player
{
    public class PauseMenuState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;
        ITimeProvider timeManager;
        public PauseMenuState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera, ITimeProvider timeManager)
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
            Debug.Log("Enter PauseManuState");
        }
        public void Execute()
        {
            if (input.PauseMenuModePressed())
            {
                GameManager.Instance.screenManager.Pop();
                controller.ChangeState(controller.NormalState);
            }
        }
        public void Exit()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            timeManager.PauseTime(false);
            Debug.Log("Exiting PauseManuState");
        }
    }

}
