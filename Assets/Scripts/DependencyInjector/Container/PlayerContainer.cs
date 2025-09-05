using InWorldUI;
using Player;

namespace DependencyInjection
{
    public class PlayerContainer : BaseContainer
    {
        public PlayerController PlayerController { get; private set; }
        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public PlayerCamera PlayerCamera { get; private set; }
        public PlayerView PlayerView { get; private set; }
        public PlayerMovement PlayerMovement { get; private set; }
        public PlayerStateController PlayerStateController { get; private set; }
        public PlayerInteract PlayerInteract { get; private set; }
        public PlayerInteraction PlayerInteraction { get; private set; }
        public PlayerInventorySystem PlayerInventorySystem { get; private set; }
        public void Initialize()
        {
            PlayerController = FindAndValidate<PlayerController>();
            PlayerInputHandler = FindAndValidate<PlayerInputHandler>();
            PlayerCamera = FindAndValidate<PlayerCamera>();
            PlayerView = FindAndValidate<PlayerView>();
            PlayerMovement = FindAndValidate<PlayerMovement>();
            PlayerStateController = FindAndValidate<PlayerStateController>();
            PlayerInteract = FindAndValidate<PlayerInteract>();
            PlayerInteraction = FindAndValidate<PlayerInteraction>();
            PlayerInventorySystem = FindAndValidate<PlayerInventorySystem>();
        }
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IPlayerCamera>(PlayerCamera);
            injector.Register<IPlayerController>(PlayerController);
            injector.Register<IPlayerView>(PlayerView);
            injector.Register<IPlayerInputHandler>(PlayerInputHandler);
        }
    }
}