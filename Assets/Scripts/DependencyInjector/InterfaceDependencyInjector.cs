using System.Collections.Generic;
using UnityEngine;
using System;

public class InterfaceDependencyInjector : MonoBehaviour, IDependencyInjectable
{
    public static InterfaceDependencyInjector Instance { get; private set; }
    
    private readonly Dictionary<Type, object> services = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InjectDependencies(DependencyContainer.Instance);

        ValidateRegistrations();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        Register<IColliderToggle>(provider.FocusModeManager);
        Register<INoteBookColliderToggle>(provider.NoteBookManager);
        Register<ICameraOrientation>(provider.CinemachinePOVExtension);
        Register<IPlayerMovementInput>(provider.PlayerInputHandler);
        Register<IPlayerCamera>(provider.PlayerCamera);
        Register<IPlayerView>(provider.PlayerView);
        Register<IPlayerController>(provider.PlayerController);
        Register<IDialogueManager>(provider.DialogueManager);
        Register<ITimeProvider>(provider.TimeManager);
        Register<IUIManager>(provider.UIManager);
        Register<ITogglePhotoDetection>(provider.PhotoDetectionZone);
    }
    public void Register<T>(T service)
    {
        if (service == null)
        {
            Debug.LogWarning($"[Injector] Tried to register null for {typeof(T)}");
            return;
        }
        services[typeof(T)] = service;
    }
    public T Resolve<T>()
    {
        if(services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }

        throw new Exception($"Service of type {typeof(T)} not registered");
    }
    void ValidateRegistrations()
    {
        Type[] required =
        {
            typeof(IColliderToggle),
        };

        foreach (var type in required)
        {
            if (!services.ContainsKey(type))
                Debug.LogError($"[Injector] Missing required service: {type}");
        }
    }
}