using UnityEngine;

public class NPCDialogueSpeaker : DialogueSpeakerBase, IInteract
{ 
    [SerializeField] string interactText;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        if (npcType != NPCType.None)
        {
            if (NPCDialogueManager.Instance != null)
            {
                NPCDialogueManager.Instance.RegisterNPC(npcType, this);
                Debug.Log($"Registered NPC: {npcType}");
            }
            else
            {
                Debug.LogError($"NPCDialogueManager.Instance is null for NPC: {npcType}");
            }
        }
        else
        {
            Debug.LogWarning($"NPC {gameObject.name} has no NPCType assigned", gameObject);
        }
    }
    protected void OnDestroy()
    {
        if (npcType != NPCType.None)
        {
            NPCDialogueManager.Instance?.UnregisterNPC(npcType);
        }
    }
    public string GetInteractText()
    {
        if (interactText == null) return interactText = "";

        return interactText;
    }
    public void Interact()
    {
        if (!isShowingDialogue && playerStateController.IsInState(playerStateController.NormalState))
        {
            StartDialogueSequence();
        }
    }
}