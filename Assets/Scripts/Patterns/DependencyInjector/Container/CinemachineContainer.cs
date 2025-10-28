using Unity.Cinemachine;
using Unity.Cinemachine.Samples;

namespace DependencyInjection
{
    public class CinemachineContainer : BaseContainer
    {
        CinemachineCamera cinemachineCamera;
        public CinemachineCamera CinemachineCamera => cinemachineCamera ??= FindAndValidate<CinemachineCamera>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(() => FindAndValidate<CinemachinePOVExtension>());
        }
    }
}