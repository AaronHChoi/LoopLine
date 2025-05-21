using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class NPCInteractionColor : MonoBehaviour
{
    public List<NPCMindPlace> NPCs = new List<NPCMindPlace>();
    public Gradient interactedGradient;
    public Gradient defaultGradient;

    private void Start()
    {
        UpdateNPCStates();
        CheckNPCInteracted();
    }
    public void CheckNPCInteracted()
    {
        foreach (NPCMindPlace npc in NPCs)
        {
            VisualEffect vfx = npc.GetComponent<VisualEffect>();

            if (npc.isNPCInteracted)
            {
                vfx.SetGradient("Color", interactedGradient);
            }
            else
            {
                vfx.SetGradient("Color", defaultGradient);
            }
        }
    }
    public void UpdateNPCStates()
    {
        Dictionary<string, bool> npcBoolStates = GameManager.Instance.GetNPCBoolStates();

        Debug.Log(npcBoolStates);

        foreach (NPCMindPlace npc in NPCs)
        {
            if(npcBoolStates.TryGetValue(npc.name, out bool state))
            {
                Debug.Log(npc.name);
                npc.isNPCInteracted = state;
            }
        }
    }
}
