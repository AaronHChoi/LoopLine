using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class NoteBookManager : MonoBehaviour, INoteBookColliderToggle
{
    [SerializeField] public List<NPCID> ID_NPC = new List<NPCID>();

    public Transform playerTPPosition;
    public bool isNPCActive = false;

    [SerializeField] GameObject[] NooteBookOpen;

    public void ToggleColliders(bool isActive)
    {
        ToggleGameObjects(NooteBookOpen, !isActive);
    }
    private void ToggleGameObjects(GameObject[] gameObjects, bool state)
    {
        foreach (var obj in gameObjects)
        {
            if (obj == null) continue;

            if (obj.TryGetComponent(out BoxCollider box))
                box.enabled = state;
            if (obj.tag == "NoteBook")
            {
                obj.SetActive(state);
            }
        }
    }
}

public interface INoteBookColliderToggle
{
    void ToggleColliders(bool isActive);
}

