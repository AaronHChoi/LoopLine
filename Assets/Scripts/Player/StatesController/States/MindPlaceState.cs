
namespace Player
{
    public class MindPlaceState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;

        public MindPlaceState(IPlayerStateController controller, IPlayerInputHandler input, IPlayerMovement movement)
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