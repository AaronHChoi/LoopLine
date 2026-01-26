using Core.DependencyInjection;
using DependencyInjection;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IPlayerCamera
{
    CinemachineCamera virtualCamera;
    CinemachineCameraOffset cameraOffset;
    Transform cameraTransform;

    private float timer = 0;

    private void Start()
    {
        try
        {
            virtualCamera = InterfaceDependencyInjector.Instance.Resolve<CinemachineCamera>();

            if (virtualCamera != null)
            {
                cameraTransform = virtualCamera.transform;
                cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[PlayerCamera] Error to resolve CinemachineCamera: {e.Message}");
        }

        if (cameraOffset == null)
        {
            Debug.LogError("CinemachineCameraOffset missing component");
        }
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
}
public interface IPlayerCamera
{
    Transform GetCameraTransform();
    void ApplyHeadBob(float frequency, float amplitude, float horizontalMultiplier);
    void ResetHeadBob(float smoothTime);
}