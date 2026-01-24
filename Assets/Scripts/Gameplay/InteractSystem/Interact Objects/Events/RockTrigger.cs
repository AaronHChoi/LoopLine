using Gameplay.DependencyInjection;
using UnityEngine;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

public class RockTrigger : MonoBehaviour
{
    IPlayerInteract playerInteract;
    IPlayerInteractMarkerPrompt playerInteraction;

    //private float OriginalPlayerInteractRange;
    //private float OriginalPlayerInteractionRange;

    private void Start()
    {
        playerInteraction = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteractMarkerPrompt>();
        playerInteract = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteract>();
        //OriginalPlayerInteractRange = 2f;
        //OriginalPlayerInteractionRange = playerInteraction.interactRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //playerInteract.raycastDistance = 2.4f;
            //playerInteraction.interactRange = 2.6f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //playerInteract.raycastDistance = OriginalPlayerInteractRange;
            //playerInteraction.interactRange = OriginalPlayerInteractionRange;
        }
    }

}
