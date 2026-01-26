using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using Core.DependencyInjection;

namespace DependencyInjection
{
    public class CinemachineContainer : BaseContainer
    {
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(() => FindAndValidate<CinemachinePOVExtension>());
            injector.Register<CinemachineCamera>(() => FindAndValidate<CinemachineCamera>());
        }
    }
}