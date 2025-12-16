using Player;
using UnityEngine;

public class PuzzleState : IState
{
    IPlayerMovement movement;
    IPlayerInputHandler inputHandler;
    IPlayerStateController controller;
    ICameraOrientation playerCamera;

    public PuzzleState(IPlayerMovement movement, IPlayerInputHandler inputHandler, IPlayerStateController controller, ICameraOrientation playerCamera)
    {
        this.controller = controller;
        this.movement = movement;
        this.inputHandler = inputHandler;
        this.playerCamera = playerCamera;
    }
    public void Enter()
    {
        movement.CanMove = false;
        playerCamera.CanLook = false;
        Debug.Log("Entering PuzzleState");
    }

    public void Execute()
    {
        if (inputHandler.PuzzleInteract())
        {
            controller.UseEventPuzzleInteract();
        }
        if (inputHandler.PuzzleLeftInteract())
        {
            controller.UseEventPuzzleLeftInteract();
        }
        if (inputHandler.PuzzleRightInteract())
        {
            controller.UseEventPuzzleRightInteract();
        }
        if (inputHandler.InteractPressed())
        {
            controller.UseEventInteract();
        }
    }

    public void Exit()
    {
        movement.CanMove = true;
        playerCamera.CanLook = true;
        Debug.Log("Exiting PuzzleState");
    }
}
