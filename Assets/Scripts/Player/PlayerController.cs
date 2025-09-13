using DependencyInjection;
using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;

    IPlayerMovement playerMovement;

    public PlayerModel PlayerModel => playerModel;

    private void Awake()
    {
        playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
    }
    private void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.RotateCharacterToCamera();
    }
}
public interface IPlayerController
{
    public PlayerModel PlayerModel { get; }
}