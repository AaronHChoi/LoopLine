using Unity.Cinemachine.Samples;
using Core.DependencyInjection;

namespace Gameplay.DependencyContainer
{
    public class CinemachineContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(() => FindAndValidate<CinemachinePOVExtension>());
        }
    }
}