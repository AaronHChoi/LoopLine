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
        Inventory,
        MindPlace,
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
        public event Action OnFocusMode;
        public event Action OnScrollInventory;

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
                case PlayerState.MindPlace:
                    HandleMindPlaceState();
                    break;
                case PlayerState.Inventory:
                    HandleInventoryState();
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

            if (playerInputHandler.ToggleCameraPressed() && PlayerInventorySystem.Instance.ItemInUse.id == "Camera")
            {
                SetState(PlayerState.Camera);
            }
            if (playerInputHandler.InteractPressed())
            {
                OnInteract?.Invoke();
            }
            if (playerInputHandler.OpenInventoryPressed())
            {
                OnOpenInventory?.Invoke();
                SetState(PlayerState.Inventory);
            }
            if (playerInputHandler.DevelopmentModePressed())
            {
                OnOpenDevelopment?.Invoke();
                SetState(PlayerState.Development);
            }
            if (playerInputHandler.FocusModePressed())
            {
                OnFocusMode?.Invoke();
                SetState(PlayerState.FocusMode);
            }
        }
        private void HandleDialogueState()
        {
            playerMovement.CanMove = false;

            if (playerInputHandler.PassDialogPressed())
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
            playerMovement.CanMove = true;

            if (playerInputHandler.FocusModePressed())
            {
                OnFocusMode?.Invoke();
                SetState(PlayerState.Normal);
            }
        }
        private void HandleDevelopmentState()
        {
            playerMovement.CanMove = false;

            if (playerInputHandler.DevelopmentModePressed())
            {
                OnOpenDevelopment?.Invoke();
                SetState(PlayerState.Normal);
            }
        }
        private void HandleMindPlaceState()
        {
            playerMovement.CanMove = true;
        }
        private void HandleInventoryState()
        {
            playerMovement.CanMove = true;

            if (playerInputHandler.OpenInventoryPressed())
            {
                OnOpenInventory?.Invoke();
                SetState(PlayerState.Normal);
            }
            if (playerInputHandler.InteractPressed())
            {
                OnInteract?.Invoke();
            }
        }
    }
}