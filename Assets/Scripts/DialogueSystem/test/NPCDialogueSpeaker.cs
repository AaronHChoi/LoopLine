using UnityEngine;

public class NPCDialogueSpeaker : DialogueSpeakerBase, IInteract
{ 
    string interactText;
    protected override void Awake()
    {
        base.Awake();

        if (!string.IsNullOrEmpty(id))
        {
            NPCDialogueManager.Instance?.RegisterNPC(id, this);
        }
        else
        {
            Debug.LogWarning($"NPC {gameObject.name} has no id assigned", gameObject);
        }
    }
    protected void OnDestroy()
    {
        if (!string.IsNullOrEmpty(id))
        {
            NPCDialogueManager.Instance?.UnregisterNPC(id);
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