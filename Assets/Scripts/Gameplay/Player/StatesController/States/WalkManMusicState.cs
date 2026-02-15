using UnityEngine;

namespace Player
{
    public class WalkManMusicState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCamera;
        IWalkman walkman;

        public WalkManMusicState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, ICameraOrientation playerCamera, IWalkman walkman)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.walkman = walkman;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            walkman.SetWalkManUIVisible(true);

            Debug.Log("Entering WalkManMusicState");
        }

        public void Execute()
        {
            if (input.InteractPressed() && !walkman.isListeningAudioTape)
            {
                controller.UseEventInteract();
            }
            if (input.GrabItemPressed() && !walkman.isListeningAudioTape)
            {
                controller.UseEventGrab();
            }
            if (input.ToggleWalkmanPressed() && !walkman.isListeningAudioTape)
            {
                controller.ChangeState(controller.NormalState);
            }
        }

        public void Exit()
        {

            movement.CanMove = false;
            playerCamera.CanLook = false;

            walkman.SetWalkManUIVisible(false);

            Debug.Log("Exiting WalkManMusicState");
        }

    }
}
