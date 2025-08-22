
namespace Player
{
    public class MindPlaceState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public MindPlaceState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = true;
        }
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
        public void Exit()
        {
            movement.CanMove = false;
        }
    }
}