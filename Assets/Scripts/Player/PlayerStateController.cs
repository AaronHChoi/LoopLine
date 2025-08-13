using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Normal,
        Dialogue,
        Camera
    }
    public class PlayerStateController : MonoBehaviour, IDependencyInjectable
    {
        public PlayerState CurrentState { get; private set; } = PlayerState.Normal;

        public event System.Action<PlayerState> OnStateChanged;

        IPolaroidCameraInput playerPolaroidCameraInput;
        PlayerMovement playerMovement;

        private void Awake()
        {
            playerPolaroidCameraInput = InterfaceDependencyInjector.Instance.Resolve<IPolaroidCameraInput>();
            InjectDependencies(DependencyContainer.Instance);
        }
        public void InjectDependencies(DependencyContainer provider)
        {
            playerMovement = provider.PlayerMovement;
        }
        private void Update()
        {
            switch(CurrentState)
            {
                case PlayerState.Normal:
                    HandleNormalState();
                    break;
                case PlayerState.Dialogue:
                    HandleDialogueState();
                    break;
                case PlayerState.Camera:
                    HandleCameraState();
                    break;
            }
        }
        public void SetState(PlayerState newState)
        {
            CurrentState = newState;
            Debug.Log($"Player State changed to: {newState}");
            OnStateChanged?.Invoke(newState);
        }
        public bool IsInState(PlayerState state)
        {
            return CurrentState == state;
        }
        private void HandleNormalState()
        {
            playerMovement.CanMove = true;

            if (playerPolaroidCameraInput.ToggleCameraPressed())
            {
                EnterCameraState();
            }
        }
        private void HandleDialogueState()
        {
            playerMovement.CanMove = false;
        }
        private void HandleCameraState()
        {
            if (playerPolaroidCameraInput.ToggleCameraPressed())
            {
                ExitCameraState();
            }
        }
        private void EnterCameraState()
        {
            SetState(PlayerState.Camera);
        }
        private void ExitCameraState()
        {
            SetState(PlayerState.Normal);
        }
    }
}