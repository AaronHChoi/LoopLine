using Unity.Cinemachine;
using UnityEngine;
using DependencyInjection;
public class PlayerCamera : MonoBehaviour, IDependencyInjectable, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    Transform cameraTransform;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        virtualCamera = provider.CinemachineContainer.CinemachineCamera;
    }
    private void Start()
    {
        cameraTransform = virtualCamera.transform;
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
}