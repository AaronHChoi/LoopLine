using InWorldUI;
using UnityEngine;

public class RockTrigger : MonoBehaviour, IDependencyInjectable
{
    PlayerInteract playerInteract;
    PlayerInteraction playerInteraction;

    private float OriginalPlayerInteractRange;
    private float OriginalPlayerInteractionRange;
    void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }

    private void Start()
    {
        OriginalPlayerInteractRange = playerInteract.raycastDistance;
        OriginalPlayerInteractionRange = playerInteraction.interactRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInteract.raycastDistance = 2.4f;
            playerInteraction.interactRange = 2.6f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInteract.raycastDistance = OriginalPlayerInteractRange;
            playerInteraction.interactRange = OriginalPlayerInteractionRange;
        }
    }

    public void InjectDependencies(DependencyContainer provider)
    {
        playerInteract = provider.PlayerInteract;
        playerInteraction = provider.PlayerInteraction;
    }
}
