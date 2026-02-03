using Player;
using UnityEngine;
using Core.DependencyInjection;

public class TriggerMindPlace : MonoBehaviour
{
    [SerializeField] private Events MindPlaceEvent = Events.MindPlaceTrigger;
    [SerializeField] private GameCondition requiredCondition = GameCondition.None;

    private IMonologueSpeaker monologueSpeaker;
    private IPlayerStateController playerStateController;

    private void Awake()
    { 
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.GetCondition(requiredCondition))
        {
            monologueSpeaker.StartMonologue(MindPlaceEvent);
            Debug.Log("Mind Place Triggered");
        }
        else if(other.CompareTag("Player") && GameManager.Instance.GetCondition(requiredCondition))
        {
            playerStateController.UseEventTeleport();
            Debug.Log("Teleporting to Mind Place");
        }
    }
}
