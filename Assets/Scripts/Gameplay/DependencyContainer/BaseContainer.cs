using UnityEngine;

namespace Core.DependencyInjection
{
    public abstract class BaseContainer
    {
        protected T FindAndValidate<T>() where T : MonoBehaviour
        {
            T instance = Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);

            if (instance == null)
            {
                Debug.LogError($"[DI] Service {typeof(T).Name} not found in scene!");
            }

            return instance;
        }
    }
}