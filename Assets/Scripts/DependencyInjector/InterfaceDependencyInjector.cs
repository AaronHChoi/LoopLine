using System.Collections.Generic;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class InterfaceDependencyInjector : MonoBehaviour, IDependencyInjectable
{
    public static InterfaceDependencyInjector Instance { get; private set; }
    
    private readonly Dictionary<System.Type, object> services = new();

    FocusModeManager focusModeManager;
    NoteBookManager noteBookManager;
    CinemachinePOVExtension cinemachinePOVExtension;
    PlayerInputHandler playerInputHandler;
    PlayerCamera playerCamera;
    PlayerView playerView;
    PlayerController playerController;
    DialogueManager dialogueManager;
    TimeManager timeManager;
    UIManager uiManager;
    SoundManager soundManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InjectDependencies(DependencyContainer.Instance);
        InitializeInterfaces();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        focusModeManager = provider.FocusModeManager;
        noteBookManager = provider.NoteBookManager;
        cinemachinePOVExtension = provider.CinemachinePOVExtension;
        playerInputHandler = provider.PlayerInputHandler;
        playerCamera = provider.PlayerCamera;
        playerView = provider.PlayerView;
        playerController = provider.PlayerController;
        dialogueManager = provider.DialogueManager;
        timeManager = provider.TimeManager;
        uiManager = provider.UIManager;
        soundManager = provider.SoundManager;
    }
    private void InitializeInterfaces()
    {
        Register<IColliderToggle>(focusModeManager);
        Register<INoteBookColliderToggle>(noteBookManager);
        Register<ICameraOrientation>(cinemachinePOVExtension);
        Register<IPlayerMovementInput>(playerInputHandler);
        Register<IPlayerCamera>(playerCamera);
        Register<IPlayerView>(playerView);
        Register<IPlayerController>(playerController);
        Register<IDialogueManager>(dialogueManager);
        Register<ISkipeable>(timeManager);
        Register<IUIManager>(uiManager);
    }
    public void Register<T>(T service)
    {
        services[typeof(T)] = service;
    }
    public T Resolve<T>()
    {
        if(services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }

        throw new System.Exception($"Service of type {typeof(T)} not registered");
    }
}