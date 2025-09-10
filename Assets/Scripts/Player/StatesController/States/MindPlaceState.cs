
namespace Player
{
    public class MindPlaceState : IState
    {
        PlayerStateController controller;
        IPlayerInputHandler input;
        PlayerMovement movement;

        public MindPlaceState(PlayerStateController controller, IPlayerInputHandler input, PlayerMovement movement)
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