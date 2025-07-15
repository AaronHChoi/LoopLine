using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FocusModeManager : MonoBehaviour, IColliderToggle
{
    [SerializeField] GameObject[] Normal;
    [SerializeField] public GameObject[] Focus;

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

public interface IColliderToggle
{
    void ToggleColliders(bool isActive);
}