using UnityEngine;

namespace Player
{
    public class WalkManMusicState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        IPlayerInteractMarkerPrompt interaction;
        ICameraOrientation playerCamera;
        IWalkman walkman;

        public WalkManMusicState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, IPlayerInteractMarkerPrompt interaction, ICameraOrientation playerCamera, IWalkman walkman)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.interaction = interaction;
            this.walkman = walkman;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            if (walkman.isListeningAudioTape)
                interaction.IsDetecting = false;
            movement.CanMove = true;
            playerCamera.CanLook = true;
            walkman.SetWalkManUIVisible(true);

            Debug.Log("Entering WalkManMusicState");
        }

        public void Execute()
        {
            if (input.ToggleWalkmanPressed() && !walkman.isListeningAudioTape)
            {
                controller.ChangeState(controller.NormalState);
            }
        }

        public void Exit()
        {

            interaction.IsDetecting = true;
            movement.CanMove = false;
            playerCamera.CanLook = false;

            walkman.SetWalkManUIVisible(false);

            Debug.Log("Exiting WalkManMusicState");
        }

    }
}
