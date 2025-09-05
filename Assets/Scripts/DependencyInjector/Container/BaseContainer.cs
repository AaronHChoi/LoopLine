using UnityEngine;

namespace DependencyInjection
{
    public abstract class BaseContainer
    {
        protected T FindAndValidate<T>() where T : MonoBehaviour
        {
            T instance = Object.FindFirstObjectByType<T>();

            if (instance == null)
                Debug.LogError($"[PlayerContainer] Missing required component of type {typeof(T)}");

            return instance;
        }
    }
}