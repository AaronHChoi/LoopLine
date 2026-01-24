using Unity.Cinemachine.Samples;
using Core.DependencyInjection;

namespace Gameplay.DependencyInjection
{
    public class CinemachineContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(() => FindAndValidate<CinemachinePOVExtension>());
        }
    }
}