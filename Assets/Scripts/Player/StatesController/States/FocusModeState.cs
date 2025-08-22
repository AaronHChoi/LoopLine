using UnityEngine;

namespace Player
{
    public class FocusModeState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public FocusModeState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = true;
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
            Debug.Log("Exiting FocusModeState");
        }
    }
}