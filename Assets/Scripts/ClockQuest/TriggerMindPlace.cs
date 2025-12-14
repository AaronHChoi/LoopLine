using DependencyInjection;
using Player;
using UnityEngine;

public class TriggerMindPlace : MonoBehaviour
{
    [SerializeField] private Events MindPlaceEvent = Events.MindPlaceTrigger;

    private IMonologueSpeaker monologueSpeaker;
    private IPlayerStateController playerStateController;

    private void Awake()
    { 
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.GetCondition(GameCondition.ClockDoorOpen))
        {
            monologueSpeaker.StartMonologue(MindPlaceEvent);
            Debug.Log("Mind Place Triggered");
        }
        else if(other.CompareTag("Player") && GameManager.Instance.GetCondition(GameCondition.ClockDoorOpen))
        {
            playerStateController.UseEventTeleport();
            Debug.Log("Teleporting to Mind Place");
        }
    }
}
