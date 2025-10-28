using System;
using Player;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameStateController GameStateMachine {  get; private set; }

    public GameplayState GameplayState { get; private set; }
    public PauseState PauseState { get; private set; }

    GameStateMachine gameStateMachine {get; set;}

    public event Action<IGameState> OnGameStateChanged;

    [SerializeField] PlayerStateController playerStateController;

    private void Awake()
    {
        gameStateMachine = new GameStateMachine();

        GameplayState = new GameplayState(this, playerStateController);
        PauseState = new PauseState(this);

        gameStateMachine.Initialize(GameplayState);
    }
    private void Update()
    {
        gameStateMachine.Execute();
    }
    public void ChangeState(IGameState newState)
    {
        gameStateMachine.TransitionTo(newState);
        OnGameStateChanged?.Invoke(newState);
    }
}