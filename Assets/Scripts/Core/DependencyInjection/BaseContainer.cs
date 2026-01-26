using UnityEngine;

namespace Core.DependencyInjection
{
    public abstract class BaseContainer
    {
        protected T FindAndValidate<T>() where T : MonoBehaviour
        {
            T instance = Object.FindFirstObjectByType<T>();

            return instance;
        }
    }
}