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

    public Transform GetTransform()
    {
        return playerMovement.transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
public interface IPlayerController
{
    Transform GetTransform();
    GameObject GetGameObject();
    public PlayerModel PlayerModel { get; }
}