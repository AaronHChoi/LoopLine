using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance { get; private set; }

    private Dictionary<string, NPCDialogueSpeaker> npcSpeakers = new Dictionary<string, NPCDialogueSpeaker>();

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
    public void RegisterNPC(string npcId, NPCDialogueSpeaker speaker)
    {
        if (!npcSpeakers.ContainsKey(npcId))
        {
            npcSpeakers.Add(npcId, speaker);
            Debug.Log($"Registered NPC: {npcId}");
        }
        else
        {
            Debug.LogWarning($"NPC with ID {npcId} is already registered");
        }
    }
    public void UnregisterNPC(string npcId)
    {
        if (npcSpeakers.ContainsKey(npcId))
        {
            npcSpeakers.Remove(npcId);
        }
    }
    public void HandleEventChange(string targetNPC, Events newEvent)
    {
        if (targetNPC == "ALL")
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