using System.Collections.Generic;
using UnityEngine;

public class InterfaceDependencyInjector : MonoBehaviour, IDependencyInjectable
{
    public static InterfaceDependencyInjector Instance { get; private set; }
    
    private readonly Dictionary<System.Type, object> services = new();

    FocusModeManager focusModeManager;
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
    }
    private void InitializeInterfaces()
    {
        Register<IColliderToggle>(focusModeManager);
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