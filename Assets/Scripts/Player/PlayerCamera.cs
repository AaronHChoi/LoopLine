using Unity.Cinemachine;
using UnityEngine;
using DependencyInjection;
public class PlayerCamera : MonoBehaviour, IDependencyInjectable, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    Transform cameraTransform;

    CinemachineBasicMultiChannelPerlin m_Noise;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        cameraTransform = virtualCamera.transform;

        m_Noise = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        virtualCamera = provider.CinemachineContainer.CinemachineCamera;
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public void SetNoiseGains(float amplitudeGain, float frequencyGain, float smoothTime)
    {
        if (m_Noise != null)
        {
            m_Noise.AmplitudeGain = Mathf.Lerp(
                m_Noise.AmplitudeGain,
                amplitudeGain,
                Time.deltaTime * smoothTime
            );

            m_Noise.FrequencyGain = Mathf.Lerp(
                m_Noise.FrequencyGain,
                frequencyGain,
                Time.deltaTime * smoothTime
            );
        }
    }
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
    void SetNoiseGains(float amplitudeGain, float frequencyGain, float smoothTime);
}