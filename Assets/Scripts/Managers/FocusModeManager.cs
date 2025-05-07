using UnityEngine;

public class FocusModeManager : MonoBehaviour
{
    public GameObject[] NPCs;
    public GameObject[] Focus;

    public void ToggleColliders(bool isActive)
    {
        ToggleGameObjects(NPCs, isActive);
        ToggleGameObjects(Focus, !isActive);
    }
    private void ToggleGameObjects(GameObject[] gameObjects, bool state)
    {
        foreach (var obj in gameObjects)
        {
            if (obj == null) continue;

            //if (obj.TryGetComponent(out DialogueSpeaker dial))
            //    dial.enabled = state;

            if (obj.TryGetComponent(out BoxCollider box))
                box.enabled = state;
        }
    }
}
