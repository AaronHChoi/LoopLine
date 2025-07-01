using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class NoteBookManager : MonoBehaviour, INoteBookColliderToggle
{
    [SerializeField] public List<NPCID> ID_NPC = new List<NPCID>();

    public Transform playerTPPosition;
    public bool isNPCActive = false;

    [SerializeField] GameObject[] NooteBookOpne;

    public void ToggleColliders(bool isActive)
    {
        ToggleGameObjects(NooteBookOpne, !isActive);
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

public interface INoteBookColliderToggle
{
    void ToggleColliders(bool isActive);
}

