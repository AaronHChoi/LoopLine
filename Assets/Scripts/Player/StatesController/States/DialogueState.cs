using UnityEngine;

namespace Player
{
    public class DialogueState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public DialogueState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = false;
            Debug.Log("Entering DialogueState");
        }
        public void Execute()
        {
            if (input.PassDialogPressed())
            {
                controller.UseEventDialogueNext();
            }
            //if (input.SkipDialogueTyping())
            //{
            //    OnDialogueSkip?.Invoke();
            //}
        }
        public void Exit()
        {
            movement.CanMove = true;
            Debug.Log("Exiting DialogueState");
        }
    }
}