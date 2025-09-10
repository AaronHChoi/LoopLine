using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;

    PlayerFocusMode playerFocusMode;
    PlayerMovement playerMovement;
    PlayerStateController playerStateController;

    public PlayerModel PlayerModel => playerModel;

    private void Awake()
    {
        playerFocusMode = GetComponent<PlayerFocusMode>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStateController = GetComponent<PlayerStateController>();
    }
    private void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.RotateCharacterToCamera();
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnFocusMode += HandleFocusMode;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnFocusMode -= HandleFocusMode;
        }
    }
    private void HandleFocusMode()
    {
        playerFocusMode.ToggleFocusMode();
    }
}
public interface IPlayerController
{
    public PlayerModel PlayerModel { get; }
}