using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IDependencyInjectable, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    Transform cameraTransform;

    ICameraOrientation cinemachinePOVExtension;

    float lockedPanValue;
    float lockedTiltValue;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        cinemachinePOVExtension = InterfaceDependencyInjector.Instance.Resolve<ICameraOrientation>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        virtualCamera = provider.CinemachineCamera;
    }
    private void Start()
    {
        cameraTransform = virtualCamera.transform;
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public void SetControllerEnabled(bool _enabled)
    {
        virtualCamera.enabled = _enabled;

        if (cinemachinePOVExtension != null)
        {
            if (_enabled)
            {
                cinemachinePOVExtension.SetPanAndTilt(lockedPanValue, lockedTiltValue);
            }
            else
            {
                (lockedPanValue, lockedTiltValue) = cinemachinePOVExtension.GetPanAndTilt();
            }
        }
    }
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
}