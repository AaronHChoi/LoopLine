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
        if (playerStateController.IsInState(PlayerState.Normal))
        {
            playerMovement.HandleMovement(playerModel);
            playerMovement.RotateCharacterToCamera(playerModel);
        }
        
        HandleInput();
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerFocusMode.ToggleFocusMode(playerModel);
        }
        if (SceneManager.GetActiveScene().name == "05. MindPlace")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerMindPlaceNoteBook.ToggleNooteBook(playerModel); 
            }
        }
    }
    public void SetCinemachineController(bool _enabled)
    {
        playerCamera.SetControllerEnabled(_enabled);

        playerStateController.SetState(_enabled ? PlayerState.Normal : PlayerState.Dialogue);
    }
}
public interface IPlayerController
{
    void SetCinemachineController(bool _enabled);
}