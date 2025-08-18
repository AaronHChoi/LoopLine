using UnityEngine;
using System;

namespace Player
{
    public enum PlayerState
    {
        Normal,
        Dialogue,
        Camera,
        FocusMode,
        Development
    }
    public class PlayerStateController : MonoBehaviour, IDependencyInjectable
    {
        public PlayerState CurrentState { get; private set; } = PlayerState.Normal;

        public event Action<PlayerState> OnStateChanged;
        public event Action OnTakePhoto;
        public event Action OnInteract;
        public event Action OnDialogueNext;
        //public event Action OnDialogueSkip;
        public event Action OnOpenInventory;
        public event Action OnOpenDevelopment;

        PlayerInputHandler playerInputHandler;
        PlayerMovement playerMovement;
        PhotoCapture photoCapture;

        private void Awake()
        {
            InjectDependencies(DependencyContainer.Instance);
        }
        public void InjectDependencies(DependencyContainer provider)
        {
            playerMovement = provider.PlayerMovement;
            playerInputHandler = provider.PlayerInputHandler;
            photoCapture = provider.PhotoCapture;
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
                case PlayerState.Development:
                    HandleDevelopmentState();
                    break;
                case PlayerState.FocusMode:
                    HandleFocusModeState();
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

            if (playerInputHandler.ToggleCameraPressed())
            {
                SetState(PlayerState.Camera);
            }
            if (playerInputHandler.Interact())
            {
                OnInteract?.Invoke();
            }
            if (playerInputHandler.OpenInventory())
            {
                OnOpenInventory?.Invoke();
            }
            if (playerInputHandler.DevelopmentMode())
            {
                OnOpenDevelopment?.Invoke();
                SetState(PlayerState.Development);
            }
        }
        private void HandleDialogueState()
        {
            playerMovement.CanMove = false;

            if (playerInputHandler.PassDialog())
            {
                OnDialogueNext?.Invoke();
            }
            //if (playerInputHandler.SkipDialogueTyping())
            //{
            //    OnDialogueSkip?.Invoke();
            //}
        }
        private void HandleCameraState()
        {
            playerMovement.CanMove = true;

            if (playerInputHandler.ToggleCameraPressed())
            {
                if (!photoCapture.IsViewingPhoto)
                {
                    SetState(PlayerState.Normal);
                }
            }

            if (playerInputHandler.TakePhotoPressed())
            {
                OnTakePhoto?.Invoke();
            }
        }
        private void HandleFocusModeState()
        {

        }
        private void HandleDevelopmentState()
        {
            playerMovement.CanMove = false;

            if (playerInputHandler.DevelopmentMode())
            {
                OnOpenDevelopment?.Invoke();
                SetState(PlayerState.Normal);
            }
        }
    }
}