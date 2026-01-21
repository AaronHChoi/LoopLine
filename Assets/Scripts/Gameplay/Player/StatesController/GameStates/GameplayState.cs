using Player;
using UnityEngine;

public class GameplayState : IGameState
{
    GameStateController controller;
    PlayerStateController playerController;

    IPlayerInputHandler input;

    public GameplayState(GameStateController controller, PlayerStateController playerController, IPlayerInputHandler input)
    {
        this.controller = controller;
        this.playerController = playerController;
        this.input = input;
    }
    public void Enter()
    {
        playerController.enabled = true;
    }

    public void Execute()
    {
        if (input.PauseMenuModePressed())
        {
            controller.UseEventPauseMenu();
            controller.ChangeState(controller.PauseState);
        }
    }

    public void Exit()
    {
        playerController.enabled = false;
    }
}
