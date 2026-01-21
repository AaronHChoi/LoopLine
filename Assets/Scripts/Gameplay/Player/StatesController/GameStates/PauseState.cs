
using UnityEngine;

public class PauseState : IGameState
{
    GameStateController controller;
    IPlayerInputHandler input;

    public PauseState(GameStateController controller, IPlayerInputHandler input)
    {
        this.controller = controller;
        this.input = input;
    }
    public void Enter()
    {
        Time.timeScale = 0f;
    }

    public void Execute()
    {
        if (input.PauseMenuModePressed())
        {
            Time.timeScale = 1f;
            controller.UseEventPauseMenu();
            controller.ChangeState(controller.GameplayState);
        }

    }

    public void Exit()
    {
        Time.timeScale = 1f;
    }
}