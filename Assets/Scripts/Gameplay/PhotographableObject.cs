using UnityEngine;
using UnityEngine.Events;

public class PhotographableObject : MonoBehaviour, IPhotographable
{
    [SerializeField] UnityEvent OnPhotoTaken;
    public bool HasBeenProcessed { get; }

    public void ProceesPhoto()
    {
        OnPhotoTaken?.Invoke();
    }
}