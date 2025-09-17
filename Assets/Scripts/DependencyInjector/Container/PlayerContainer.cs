using InWorldUI;
using Player;

namespace DependencyInjection
{
    public class PlayerContainer : BaseContainer
    {
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