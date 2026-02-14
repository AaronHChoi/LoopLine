using UnityEngine;

namespace Player
{
    public class WalkManMusicState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        IPlayerInteractMarkerPrompt interaction;
        IWalkman walkman;

        public WalkManMusicState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement, IPlayerInteractMarkerPrompt interaction, IWalkman walkman)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.interaction = interaction;
            this.walkman = walkman;
        }
        public void Enter()
        {
            interaction.IsDetecting = false;
            movement.CanMove = true;

            walkman.SetWalkManUIVisible(true);

            Debug.Log("Entering WalkManMusicState");
        }

        public void Execute()
        {
            if (input.ToggleWalkmanPressed())
            {
                controller.ChangeState(controller.NormalState);
            }
        }

        public void Exit()
        {
            interaction.IsDetecting = true;
            movement.CanMove = false;

            walkman.SetWalkManUIVisible(false);

            Debug.Log("Exiting WalkManMusicState");
        }

    }
}
