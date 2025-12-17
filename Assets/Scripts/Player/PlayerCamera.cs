using Unity.Cinemachine;
using UnityEngine;
using DependencyInjection;
public class PlayerCamera : MonoBehaviour, IDependencyInjectable, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    CinemachineCameraOffset cameraOffset;
    Transform cameraTransform;

    private float timer = 0;
    private Vector3 currentOffset = Vector3.zero;

    //CinemachineBasicMultiChannelPerlin m_Noise;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        cameraTransform = virtualCamera.transform;

        cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();
        if (cameraOffset == null )
        {
            Debug.LogError("CinemachineCameraOffset missing component");
        }
        //m_Noise = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        virtualCamera = provider.CinemachineContainer.CinemachineCamera;
    }
    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public void ApplyHeadBob(float frequency, float amplitude, float horizontalMultiplier)
    {
        if (cameraOffset == null) return;

        timer += Time.deltaTime * frequency;

        float bobY = Mathf.Sin(timer) * amplitude;

        float bobX = Mathf.Cos(timer / 2) * amplitude * horizontalMultiplier;

        Vector3 targetOffset = new Vector3(bobX, bobY, 0);

        cameraOffset.Offset = Vector3.Lerp(cameraOffset.Offset, targetOffset, Time.deltaTime * 10f);
    }
    public void ResetHeadBob(float smoothTime)
    {
        if (cameraOffset == null) return;

        timer = 0; 

        cameraOffset.Offset = Vector3.Lerp(cameraOffset.Offset, Vector3.zero, Time.deltaTime * smoothTime);
    }
    //public void SetNoiseGains(float amplitudeGain, float frequencyGain, float smoothTime)
    //{
    //    if (m_Noise != null)
    //    {
    //        m_Noise.AmplitudeGain = Mathf.Lerp(
    //            m_Noise.AmplitudeGain,
    //            amplitudeGain,
    //            Time.deltaTime * smoothTime
    //        );

    //        m_Noise.FrequencyGain = Mathf.Lerp(
    //            m_Noise.FrequencyGain,
    //            frequencyGain,
    //            Time.deltaTime * smoothTime
    //        );
    //    }
    //}
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
    //void SetNoiseGains(float amplitudeGain, float frequencyGain, float smoothTime);
    void ApplyHeadBob(float frequency, float amplitude, float horizontalMultiplier);
    void ResetHeadBob(float smoothTime);
}