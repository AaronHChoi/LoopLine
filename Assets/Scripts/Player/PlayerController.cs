using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerView playerView;
    PlayerModel playerModel;
    PlayerInputHandler playerInputHandler;
    PlayerCamera playerCamera;
    PlayerFocusMode playerFocusMode;
    PlayerMovement playerMovement;
    private void Awake()
    {
        playerInputHandler = GetComponent<PlayerInputHandler>();
        playerCamera = GetComponent<PlayerCamera>();
        playerView = GetComponent<PlayerView>();
        playerFocusMode = GetComponent<PlayerFocusMode>();
        playerMovement = GetComponent<PlayerMovement>();

        playerModel = new PlayerModel();
    }
    private void Update()
    {
        playerMovement.HandleMovement(playerInputHandler.SpeedInputMovement, playerInputHandler.InputMovement, playerInputHandler, playerCamera, playerModel, playerView);
        playerMovement.RotateCharacterToCamera(playerCamera, playerModel);

        if (Input.GetKeyDown(KeyCode.V))
        {
            playerFocusMode.ToggleFocusMode(playerModel);
        }
    }
    public void SetCinemachineController(bool _enabled)
    {
        playerCamera.SetControllerEnabled(_enabled);
        playerMovement.CanMove = _enabled;
    }
}