using UnityEngine;

public class NPCMindPlaceInteraction : MonoBehaviour, IInteract, IDependencyInjectable
{
    [SerializeField] private NPCMindPlace npcMindPlace;
    [SerializeField] private string interactText = "";

    NoteBookManager noteBookManager;
    PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcMindPlace = GetComponent<NPCMindPlace>();
        InjectDependencies(DependencyContainer.Instance);
    }

    public void Interact()
    {   
        if (npcMindPlace.IsNPCInteracted)
        {
            foreach (NPCID npcID in noteBookManager.ID_NPC)
            {
                Debug.Log('1');
                if (npcID.NPCIDValue == npcMindPlace.npcName)
                {
                    Debug.Log('2');
                    npcID.gameObject.SetActive(true);
                    playerController.characterController.enabled = false;
                    playerController.transform.position = noteBookManager.playerTPPosition.position;
                    playerController.characterController.enabled = true;
                }
                else
                {
                    npcID.gameObject.SetActive(false);
                }
            }
        }
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public void InjectDependencies(DependencyContainer provider)
    {
        noteBookManager = provider.NoteBookManager;
        playerController = provider.PlayerController;
    }
}
