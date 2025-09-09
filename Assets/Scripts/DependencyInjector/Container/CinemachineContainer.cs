using Unity.Cinemachine;
using Unity.Cinemachine.Samples;

namespace DependencyInjection
{
    public class CinemachineContainer : BaseContainer
    {
        CinemachineCamera cinemachineCamera;
        public CinemachineCamera CinemachineCamera => cinemachineCamera ??= FindAndValidate<CinemachineCamera>();

        CinemachinePOVExtension cinemachinePOVExtension;
        public CinemachinePOVExtension CinemachinePOVExtension => cinemachinePOVExtension ??= FindAndValidate<CinemachinePOVExtension>();

        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(CinemachinePOVExtension);
        }
    }
}