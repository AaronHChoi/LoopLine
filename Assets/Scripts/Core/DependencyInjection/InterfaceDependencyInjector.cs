using System;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

namespace Core.DependencyInjection
{
    public class InterfaceDependencyInjector : Singleton<InterfaceDependencyInjector>
    {
        Dictionary<Type, Func<object>> factories = new();
        Dictionary<Type, object> instances = new();

        protected override void Awake()
        {
            base.Awake();
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