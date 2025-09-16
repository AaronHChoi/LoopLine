using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class DialogueState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;

        public DialogueState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera)
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
            playerCamera.CanLook = true;
            Debug.Log("Exiting DialogueState");
        }
    }
}