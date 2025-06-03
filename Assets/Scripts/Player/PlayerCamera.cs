using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IDependencyInjectable
{
    CinemachineCamera virtualCamera;
    Transform cameraTransform;
    CinemachinePOVExtension cinemachinePOVExtension;

    float lockedPanValue;
    float lockedTiltValue;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        virtualCamera = provider.CinemachineCamera;
        cinemachinePOVExtension = provider.CinemachinePOVExtension;
    }
    private void Start()
    {
        cameraTransform = virtualCamera.transform;
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public void SetControllerEnabled(bool _enabled, bool _canMove)
    {
        _canMove = _enabled;
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