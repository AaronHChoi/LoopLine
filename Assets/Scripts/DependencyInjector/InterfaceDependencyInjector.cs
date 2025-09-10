using System.Collections.Generic;
using UnityEngine;
using System;

namespace DependencyInjection
{
    public class InterfaceDependencyInjector : MonoBehaviour, IDependencyInjectable
    {
        public static InterfaceDependencyInjector Instance { get; private set; }

        Dictionary<Type, Func<object>> factories = new();
        Dictionary<Type, object> instances = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InjectDependencies(DependencyContainer.Instance);
        }
        public void InjectDependencies(DependencyContainer provider)//para eliminar
        {
            provider.PlayerContainer.RegisterServices(this);
            provider.CinemachineContainer.RegisterServices(this);
            provider.ManagerContainer.RegisterServices(this);
            provider.UIContainer.RegisterServices(this);
            provider.PhotoContainer.RegisterServices(this);
        }
        public void Register<T>(Func<T> factory)
        {
            if (factory == null)
            {
                Debug.LogWarning($"[Injector] Tried to register null for {typeof(T)}");
                return;
            }
            factories[typeof(T)] = () => factory();
        }
        public T Resolve<T>()
        {
            var type = typeof(T);

            if(!instances.TryGetValue(type, out var instance))
            {
                if (!factories.TryGetValue(type, out var factory))
                    throw new Exception($"No service registered for {type}");

                instance = factory();
                instances[type] = instance;
            }
            return (T)instance;
        }
    }
}