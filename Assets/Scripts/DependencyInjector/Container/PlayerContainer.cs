using InWorldUI;
using Player;

namespace DependencyInjection
{
    public class PlayerContainer : BaseContainer
    {
        PlayerController playerController;
        public PlayerController PlayerController => playerController ??= FindAndValidate<PlayerController>();

        //PlayerInputHandler playerInputHandler;
        //public PlayerInputHandler PlayerInputHandler => playerInputHandler ??= FindAndValidate<PlayerInputHandler>();

        //PlayerCamera playerCamera;
        //public PlayerCamera PlayerCamera => playerCamera ??= FindAndValidate<PlayerCamera>();

        PlayerView playerView;
        public PlayerView PlayerView => playerView ??= FindAndValidate<PlayerView>();

        PlayerMovement playerMovement;
        public PlayerMovement PlayerMovement => playerMovement ??= FindAndValidate<PlayerMovement>();

        PlayerStateController playerStateController;
        public PlayerStateController PlayerStateController => playerStateController ??= FindAndValidate<PlayerStateController>();

        PlayerInteract playerInteract;
        public PlayerInteract PlayerInteract => playerInteract ??= FindAndValidate<PlayerInteract>();

        PlayerInteraction playerInteraction;
        public PlayerInteraction PlayerInteraction => playerInteraction ??= FindAndValidate<PlayerInteraction>();

        PlayerInventorySystem playerInventorySystem;
        public PlayerInventorySystem PlayerInventorySystem => playerInventorySystem ??= FindAndValidate<PlayerInventorySystem>();
        
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IPlayerCamera>(() => FindAndValidate<PlayerCamera>());
            injector.Register<IPlayerController>(() => PlayerController);
            injector.Register<IPlayerView>(() => PlayerView);
            injector.Register<IPlayerInputHandler>(() => FindAndValidate<PlayerInputHandler>());
        }
    }
}