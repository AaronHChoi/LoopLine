using Player;
using UnityEngine;
using Core.Data;

public class CinematicState : IState
{
    IPlayerMovement movement;
    ICameraOrientation playerCamera;
    IPlayerInputHandler input;
    IPlayerStateController controller;

    public CinematicState(IPlayerMovement movement, ICameraOrientation playerCamera, IPlayerInputHandler input, IPlayerStateController controller)
    {
        this.movement = movement;
        this.playerCamera = playerCamera;
        this.input = input;
        this.controller = controller;
    }
    public void Enter()
    {
        movement.CanMove = false;
        playerCamera.CanLook = false;
        Debug.Log("Entering CinematicState");
    }
    public void Execute()
    {
        if (input.SkipCinematicInteract() && GameManager.Instance.GetCondition(GameCondition.IsCinematicActivated))
        {
            controller.UseEventSkipCinematic();
        }
    }
    public void Exit()
    {
        movement.CanMove = true;
        playerCamera.CanLook = true;
        Debug.Log("Exiting CinematicState");
    }
}