using UnityEngine;

public class NPCID: MonoBehaviour
{
    [SerializeField] string npcID;

    public string NPCIDValue
    {
        get { return npcID; }

        private set { value = npcID; }
    }

}
