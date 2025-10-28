
using UnityEngine;

public class PauseState : IGameState
{
    GameStateController controller;

    public PauseState(GameStateController controller)
    {
        this.controller = controller;
    }
    public void Enter()
    {
        Time.timeScale = 0f;
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale = 1f;
            controller.ChangeState(controller.GameplayState);
        }
    }

    public void Exit()
    {
        Time.timeScale = 1f;
    }
}