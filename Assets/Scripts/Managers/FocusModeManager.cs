using UnityEngine;

public class FocusModeManager : MonoBehaviour
{
    public BoxCollider[] NPCColliders;
    public BoxCollider[] FocusColliders;

    public void ToggleColliders(bool isActive)
    {
        foreach (var collider in NPCColliders)
        {
            collider.enabled = isActive;
        }
        foreach (var collider in FocusColliders)
        {
            collider.enabled = !isActive;
        }
    }
}
