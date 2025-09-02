using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;
    PlayerCamera playerCamera;
    PlayerFocusMode playerFocusMode;
    PlayerMindPlaceNoteBook playerMindPlaceNoteBook;
    PlayerMovement playerMovement;
    PlayerStateController playerStateController;

    public CharacterController characterController;
    public PlayerModel PlayerModel => playerModel;
    private void Awake()
    {
        playerCamera = GetComponent<PlayerCamera>();
        playerFocusMode = GetComponent<PlayerFocusMode>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMindPlaceNoteBook = GetComponent<PlayerMindPlaceNoteBook>();
        characterController = GetComponent<CharacterController>();
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
    public void SetCinemachineController(bool _enabled)
    {
        //Para cambiar los estados entre normal y dialogue

        playerStateController.ChangeState(_enabled ? playerStateController.NormalState : playerStateController.DialogueState);
    }
}
public interface IPlayerController
{
    void SetCinemachineController(bool _enabled);
}