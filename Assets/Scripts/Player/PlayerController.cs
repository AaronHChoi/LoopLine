using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;

    PlayerMovement playerMovement;

    public PlayerModel PlayerModel => playerModel;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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