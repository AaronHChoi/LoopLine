using UI;
using Core.DependencyInjection;
namespace DependencyInjection
{
    public class WalkmanContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<IAudioTape>(() => FindAndValidate<AudioTape>());
            injector.Register<IWalkman>(() => FindAndValidate<Walkman>());
            injector.Register<IWalkmanItem>(() => FindAndValidate<WalkManItem>());

        }
    }
}
