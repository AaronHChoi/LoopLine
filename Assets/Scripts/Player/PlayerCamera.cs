using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IDependencyInjectable, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    Transform cameraTransform;
    [SerializeField] Transform cameraPOV;
    [SerializeField] LookAtNPC lookAtNPC;

    CinemachinePOVExtension cinemachinePOVExtension;

    float lockedPanValue;
    float lockedTiltValue;

    bool isLocked = false;

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
    private void LateUpdate()
    {
        if (isLocked && cinemachinePOVExtension != null)
        {
            cinemachinePOVExtension.SetPanAndTilt(lockedPanValue, lockedTiltValue);
        }
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public void SetControllerEnabled(bool _enabled)
    {
        if (_enabled)
        {
            virtualCamera.Follow = cameraPOV;
        }
        else
        {
            virtualCamera.Follow = null;
        }
        //if (cinemachinePOVExtension != null)
        //{
        //    if (_enabled)
        //    {
        //        cinemachinePOVExtension.SetPanAndTilt(lockedPanValue, lockedTiltValue);
        //    }
        //    else
        //    {
        //        (lockedPanValue, lockedTiltValue) = cinemachinePOVExtension.GetPanAndTilt();
        //    }
        //}
    }
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
}