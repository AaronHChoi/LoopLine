using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    PlayerModel playerModel;
    PlayerCamera playerCamera;
    PlayerFocusMode playerFocusMode;
    PlayerMovement playerMovement;
    private void Awake()
    {
        playerCamera = GetComponent<PlayerCamera>();
        playerFocusMode = GetComponent<PlayerFocusMode>();
        playerMovement = GetComponent<PlayerMovement>();

        playerModel = new PlayerModel();
    }
    private void Update()
    {
        playerMovement.HandleMovement(playerModel);
        playerMovement.RotateCharacterToCamera(playerModel);
        HandleInput();
    }
    private void HandleInput()
    {
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
public interface IPlayerController
{
    void SetCinemachineController(bool _enabled);
}