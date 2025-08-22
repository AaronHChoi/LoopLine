
namespace Player
{
    public class DevelopmentState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public DevelopmentState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = false;
        }
        public void Execute()
        {
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.ChangeState(controller.NormalState);
            }
        }
        public void Exit()
        {
            movement.CanMove = true;
        }
    }
}