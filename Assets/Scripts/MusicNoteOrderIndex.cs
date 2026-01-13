using UnityEngine;
using UnityEngine.Events;

public class MusicNoteOrderIndex : MonoBehaviour
{
    public UnityEvent OnValidateInteraction;

    public void TryInteract()
    {

            OnValidateInteraction?.Invoke();

    }
}
