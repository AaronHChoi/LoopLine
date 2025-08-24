using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IPlayerController
{
    PlayerModel playerModel;
    PlayerCamera playerCamera;
    PlayerFocusMode playerFocusMode;
    PlayerMindPlaceNoteBook playerMindPlaceNoteBook;
    PlayerMovement playerMovement;
    PlayerStateController playerStateController;

    public CharacterController characterController;
    private void Awake()
    {
        playerCamera = GetComponent<PlayerCamera>();
        playerFocusMode = GetComponent<PlayerFocusMode>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMindPlaceNoteBook = GetComponent<PlayerMindPlaceNoteBook>();
        characterController = GetComponent<CharacterController>();
        playerStateController = GetComponent<PlayerStateController>();
        playerModel = new PlayerModel();
    }
    private void Update()
    {
        playerMovement.HandleMovement(playerModel);
        playerMovement.RotateCharacterToCamera(playerModel);
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
        playerFocusMode.ToggleFocusMode(playerModel);
    }
    public void SetCinemachineController(bool _enabled)
    {
        playerCamera.SetControllerEnabled(_enabled);

        playerStateController.ChangeState(_enabled ? playerStateController.NormalState : playerStateController.DialogueState);
    }
}
public interface IPlayerController
{
    void SetCinemachineController(bool _enabled);
}