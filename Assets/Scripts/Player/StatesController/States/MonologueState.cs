using Player;

public class MonologueState : IState
{
    IPlayerStateController controller;
    IPlayerMovement movement;
    IPlayerInputHandler input;
    ICameraOrientation playerCamera;

    public MonologueState(IPlayerStateController controller, IPlayerMovement movement, IPlayerInputHandler input, ICameraOrientation playerCamera)
    {
        this.controller = controller;
        this.movement = movement;
        this.input = input;
        this.playerCamera = playerCamera;
    }
    public void Enter()
    {
        movement.CanMove = true;
        playerCamera.CanLook = true;
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
        playerCamera.CanLook = false;
    }
}