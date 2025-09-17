using Player;

public class MonologueState : IState
{
    IPlayerStateController controller;
    IPlayerMovement movement;
    IPlayerInputHandler input;

    public MonologueState(IPlayerStateController controller, IPlayerMovement movement, IPlayerInputHandler input)
    {
        this.controller = controller;
        this.movement = movement;
        this.input = input;
    }
    public void Enter()
    {
        movement.CanMove = true;
    }

    public void Execute()
    {
        if (input.PassDialogPressed())
        {
            controller.UseEventDialogueNext();
        }
    }

    public void Exit()
    {
        movement.CanMove = false;
    }
}