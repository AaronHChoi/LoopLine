using UnityEngine;

public class FocusModeManager : MonoBehaviour
{
    [SerializeField] GameObject[] Normal;
    [SerializeField] GameObject[] Focus;

    public void ToggleColliders(bool isActive)
    {
        ToggleGameObjects(Normal, isActive);
        ToggleGameObjects(Focus, !isActive);
    }
    private void ToggleGameObjects(GameObject[] gameObjects, bool state)
    {
        foreach (var obj in gameObjects)
        {
            if (obj == null) continue;

            if (obj.TryGetComponent(out BoxCollider box))
                box.enabled = state;
        }
    }
}