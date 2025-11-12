using DependencyInjection;
using Player;
using System;
using UnityEngine;

public class GameStateController : MonoBehaviour, IGameStateController
{
    public GameStateController GameStateMachine {  get; private set; }

    IPlayerInputHandler inputHandler;

    public GameplayState GameplayState { get; private set; }
    public PauseState PauseState { get; private set; }

    GameStateMachine gameStateMachine {get; set;}

    public event Action<IGameState> OnGameStateChanged;
    public event Action OnPauseMenu;

    [SerializeField] PlayerStateController playerStateController;

    private void Awake()
    {
        inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        gameStateMachine = new GameStateMachine();

        GameplayState = new GameplayState(this, playerStateController, inputHandler);
        PauseState = new PauseState(this, inputHandler);

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

    public void UseEventPauseMenu()
    {
        OnPauseMenu?.Invoke();
    }
}

public interface IGameStateController
{
    public event Action OnPauseMenu;
    public GameplayState GameplayState{ get; }
    void ChangeState(IGameState newState);
    void UseEventPauseMenu();
}