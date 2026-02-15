using Core.Data;
using Core.DependencyInjection;
using Player;
using UnityEngine;

public class TriggerCondition : MonoBehaviour
{
    [SerializeField] private Events Event;
    [SerializeField] private GameCondition requiredCondition = GameCondition.None;

    private IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.GetCondition(requiredCondition))
        {
            monologueSpeaker.StartMonologue(Event);
        }
    }
}
