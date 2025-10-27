using Player;
using UnityEngine;

public class GameplayState : IGameState
{
    GameStateController controller;
    PlayerStateController playerController;

    public GameplayState(GameStateController controller, PlayerStateController playerController)
    {
        this.controller = controller;
        this.playerController = playerController;
    }
    public void Enter()
    {
        playerController.enabled = true;
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            controller.ChangeState(controller.PauseState);
        }
    }

    public void Exit()
    {
        playerController.enabled = false;
    }
}
