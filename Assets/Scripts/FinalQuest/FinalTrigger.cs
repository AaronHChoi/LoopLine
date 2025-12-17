using DependencyInjection;
using Player;
using UnityEngine;
using UnityEngine.Video;

public class FinalTrigger : MonoBehaviour
{
    [SerializeField] VideoClip succesCinematic;

    ICinematicManager cinematicManager;
    IPlayerStateController playerStateController;

    private void Awake()
    {
        cinematicManager = InterfaceDependencyInjector.Instance.Resolve<ICinematicManager>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);
        cinematicManager.PlayCinematic(succesCinematic);

    }
}