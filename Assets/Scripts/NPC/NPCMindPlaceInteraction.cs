using System.Collections;
using UnityEngine;

public class NPCMindPlaceInteraction : MonoBehaviour, IInteract, IDependencyInjectable
{
    [SerializeField] private NPCMindPlace npcMindPlace;
    [SerializeField] private string interactText = "";
    [SerializeField] private Transform ClairsRoomTpPosition;

    NoteBookManager noteBookManager;
    PlayerController playerController;

    [Header("TestSelection")]
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject interrogationLight;
    [SerializeField] GameObject clues;
    [SerializeField] bool cluesActivate;
    
    void Start()
    {
        npcMindPlace = GetComponent<NPCMindPlace>();
        InjectDependencies(DependencyContainer.Instance);
    }

    private void Update()
    {
        if (npcMindPlace.IsNPCInteracted)
        {
            gameObject.layer = LayerMask.NameToLayer("interact");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Interact()
    {   
        if (npcMindPlace.IsNPCInteracted)
        {
            foreach (NPCID npcID in noteBookManager.ID_NPC)
            {
                if (npcID.NPCIDValue == npcMindPlace.npcName)
                {
                    noteBookManager.TransitionInMindPlace();
                    StartCoroutine(WaitForSeconds(1));
                    npcID.gameObject.SetActive(true);
                    playerController.characterController.enabled = false;
                    noteBookManager.ToggleTrainFBX();
                    noteBookManager.ClairsRoom.transform.position = ClairsRoomTpPosition.position;
                    playerController.characterController.enabled = true;
                    canvas.SetActive(true);
                    interrogationLight.SetActive(true);
                    clues.SetActive(true);
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

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
