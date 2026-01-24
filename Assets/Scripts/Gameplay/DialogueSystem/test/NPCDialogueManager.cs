using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour, INPCDialogueManager
{
    public static NPCDialogueManager Instance { get; private set; }

    private Dictionary<NPCType, NPCDialogueSpeaker> npcSpeakers = new Dictionary<NPCType, NPCDialogueSpeaker>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RegisterNPC(NPCType npcType, NPCDialogueSpeaker speaker)
    {
        if (!npcSpeakers.ContainsKey(npcType))
        {
            npcSpeakers.Add(npcType, speaker);
            Debug.Log($"Registered NPC: {npcType}");
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcType} is already registered");
        }
    }
    public void UnregisterNPC(NPCType npcType)
    {
        if (npcSpeakers.ContainsKey(npcType))
        {
            npcSpeakers.Remove(npcType);
        }
    }
    public void HandleEventChange(NPCType targetNPC, Events newEvent)
    {
        if (targetNPC == NPCType.None)
        {
            foreach (var speaker in npcSpeakers.Values)
            {
                speaker.HandleEventChange(targetNPC, newEvent);
            }
        }
        else if (npcSpeakers.ContainsKey(targetNPC))
        {
            npcSpeakers[targetNPC].HandleEventChange(targetNPC, newEvent);
        }
        else
        {
            Debug.LogWarning($"NPC with ID {targetNPC} not found");
        }
    }
}
public interface INPCDialogueManager
{
    void HandleEventChange(NPCType targetNPC, Events newEvent);
}