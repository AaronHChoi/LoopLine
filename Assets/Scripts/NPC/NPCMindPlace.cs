using UnityEngine;

public class NPCMindPlace : MonoBehaviour
{
    [SerializeField] bool isNPCInteracted;
    [SerializeField] public string npcName;
    //[SerializeField] string name;

    public bool IsNPCInteracted
    {
        get => isNPCInteracted;
        set => isNPCInteracted = value;
    }
}
