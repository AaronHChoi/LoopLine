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
}
