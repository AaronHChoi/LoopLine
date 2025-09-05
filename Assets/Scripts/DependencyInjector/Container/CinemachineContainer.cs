using Unity.Cinemachine;
using Unity.Cinemachine.Samples;

namespace DependencyInjection
{
    public class CinemachineContainer : BaseContainer
    {
        public CinemachineCamera CinemachineCamera { get; private set; }
        public CinemachinePOVExtension CinemachinePOVExtension { get; private set; }
        public void Initialize()
        {
            CinemachineCamera = FindAndValidate<CinemachineCamera>();
            CinemachinePOVExtension = FindAndValidate<CinemachinePOVExtension>();
        }
        public void RegisterServices(InterfaceDependencyInjector injector)
        {
            injector.Register<ICameraOrientation>(CinemachinePOVExtension);
        }
    }
}