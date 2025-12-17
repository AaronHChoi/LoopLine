using UnityEngine;

public class CinematicState : IState
{
    IPlayerMovement movement;
    ICameraOrientation playerCamera;

    public CinematicState(IPlayerMovement movement, ICameraOrientation playerCamera)
    {
        this.movement = movement;
        this.playerCamera = playerCamera;
    }
    public void Enter()
    {
        movement.CanMove = false;
        playerCamera.CanLook = false;
        Debug.Log("Entering CinematicState");
    }
    public void Execute()
    {

    }
    public void Exit()
    {
        movement.CanMove = true;
        playerCamera.CanLook = true;
        Debug.Log("Exiting CinematicState");
    }
}