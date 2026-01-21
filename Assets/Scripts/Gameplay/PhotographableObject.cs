using UnityEngine;
using UnityEngine.Events;

public class PhotographableObject : MonoBehaviour, IPhotographable
{
    [SerializeField] UnityEvent OnPhotoTaken;
    public void ProceesPhoto()
    {
        OnPhotoTaken?.Invoke();
    }
}