using System.Collections.Generic;
using UnityEngine;
using System;

namespace DependencyInjection
{
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
            provider.PlayerContainer.RegisterServices(this);
            provider.CinemachineContainer.RegisterServices(this);
            provider.ManagerContainer.RegisterServices(this);
            provider.UIContainer.RegisterServices(this);
            provider.PhotoContainer.RegisterServices(this);
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
            if (services.TryGetValue(typeof(T), out var service))
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
}