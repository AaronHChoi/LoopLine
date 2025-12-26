using UnityEngine;
using UnityEngine.Events;

public class OrderedInteraction : MonoBehaviour
{
    [SerializeField] int myOrderIndex;
    public UnityEvent OnValidateInteraction;

    public void TryInteract()
    {
        if (myOrderIndex == GameManager.Instance.currentPhotoIndex)
        {
            GameManager.Instance.currentPhotoIndex++;
            OnValidateInteraction?.Invoke();
        }
    }
}