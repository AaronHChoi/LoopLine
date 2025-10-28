using System;

[Serializable]
public class GameStateMachine 
{
    public IGameState CurrentGameState { get; private set; }

    public void Initialize(IGameState startingState)
    {
        CurrentGameState = startingState;
        startingState.Enter();
    }
    public void TransitionTo(IGameState nextState)
    {
        CurrentGameState.Exit();
        CurrentGameState = nextState;
        CurrentGameState.Enter();
    }
    public void Execute()
    {
        if (CurrentGameState != null)
        {
            CurrentGameState.Execute();
        }
    }
}