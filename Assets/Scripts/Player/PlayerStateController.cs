using UnityEngine;

namespace Player
{
    public class PlayerStateController : MonoBehaviour, IDependencyInjectable
    {
        public enum PlayerState
        {
            Normal,
            Dialogue,
            Camera
        }

        public PlayerState CurrentState { get; private set; } = PlayerState.Normal;

        IPlayerInputHandler playerInputHandler;
        PlayerMovement playerMovement;

        private void Awake()
        {
            playerInputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
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
                    HandleDialgueState();
                    break;
            }
        }
        private void HandleNormalState()
        {
            playerMovement.CanMove = true;
        }
        private void HandleDialgueState()
        {
        }
        private void HandleCameraState()
        {

        }
    }
}