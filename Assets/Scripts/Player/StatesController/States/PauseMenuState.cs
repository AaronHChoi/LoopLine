
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
            Time.timeScale = 0f;
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
            Time.timeScale = 1f;
            Debug.Log("Exiting PauseManuState");
        }
    }

}
