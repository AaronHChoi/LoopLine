using InWorldUI;
using Player;

namespace DependencyInjection
{
    public class PlayerContainer : BaseContainer
    {
        //PlayerController playerController;
        //public PlayerController PlayerController => playerController ??= FindAndValidate<PlayerController>();

        //PlayerInputHandler playerInputHandler;
        //public PlayerInputHandler PlayerInputHandler => playerInputHandler ??= FindAndValidate<PlayerInputHandler>();

        //PlayerCamera playerCamera;
        //public PlayerCamera PlayerCamera => playerCamera ??= FindAndValidate<PlayerCamera>();

        //PlayerView playerView;
        //public PlayerView PlayerView => playerView ??= FindAndValidate<PlayerView>();

        //IPlayerMovement playerMovement;
        //public PlayerMovement PlayerMovement => playerMovement ??= FindAndValidate<PlayerMovement>();

        //PlayerStateController playerStateController;
        //public PlayerStateController PlayerStateController => playerStateController ??= FindAndValidate<PlayerStateController>();

        //PlayerInteract playerInteract;
        //public PlayerInteract PlayerInteract => playerInteract ??= FindAndValidate<PlayerInteract>();

        //PlayerInteractMarkerPrompt playerInteraction;
        //public PlayerInteractMarkerPrompt PlayerInteraction => playerInteraction ??= FindAndValidate<PlayerInteractMarkerPrompt>();

        //PlayerInventorySystem playerInventorySystem;
        //public PlayerInventorySystem PlayerInventorySystem => playerInventorySystem ??= FindAndValidate<PlayerInventorySystem>();
        
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IPlayerInteractMarkerPrompt>(() => FindAndValidate<PlayerInteractMarkerPrompt>());
            injector.Register<IPlayerInteract>(() => FindAndValidate<PlayerInteract>());
            injector.Register<IPlayerMovement>(() => FindAndValidate<PlayerMovement>());
            injector.Register<IPlayerCamera>(() => FindAndValidate<PlayerCamera>());
            injector.Register<IPlayerController>(() => FindAndValidate<PlayerController>());
            injector.Register<IPlayerView>(() => FindAndValidate<PlayerView>());
            injector.Register<IPlayerInputHandler>(() => FindAndValidate<PlayerInputHandler>());
            injector.Register<IPlayerStateController>(() => FindAndValidate<PlayerStateController>());
        }
    }
}