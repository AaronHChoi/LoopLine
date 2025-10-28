using UnityEngine;
using UnityEngine.Rendering;
using Unity.Cinemachine;

public class SceneTransitionController : MonoBehaviour, ISceneTransitionController
{
    [Header("Transition Settings")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float fovDuration = 0.5f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Camera & Effects")]
    [SerializeField] private Volume transitionVolume;
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineImpulseSource impulseSource; // assign with Noise Signal
    [SerializeField] private float startFOV = 60f;
    [SerializeField] private float endFOV = 120f;
    [SerializeField] private float fadeTargetWeight = 1f;

    private Coroutine currentTransition = null;
    private bool isActive = false;

    private void Start()
    {
        if (transitionVolume)
            transitionVolume.weight = 0f;

        if (mainCamera)
            mainCamera.Lens.FieldOfView = startFOV;
    }
    public void StartTransition(bool forward)
    {
        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(TransitionSequence(forward));
    }
    private System.Collections.IEnumerator TransitionSequence(bool forward)
    {
        // Start rumble immediately
        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        // Set initial FOV
        if (mainCamera != null)
            mainCamera.Lens.FieldOfView = forward ? startFOV : endFOV;

        // Start FOV warp and fade in parallel
        Coroutine fovCoroutine = StartCoroutine(FOVWarpCoroutine(
            mainCamera.Lens.FieldOfView,
            forward ? endFOV : startFOV,
            fovDuration));

        Coroutine fadeCoroutine = StartCoroutine(FadeVolumeCoroutine(
            transitionVolume.weight,
            forward ? fadeTargetWeight : 0f,
            fadeDuration));

        // Wait until both coroutines finish
        yield return fovCoroutine;
        yield return fadeCoroutine;
    }
    private System.Collections.IEnumerator FOVWarpCoroutine(float fromFOV, float toFOV, float duration)
    {
        if (mainCamera == null || duration <= 0f)
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            mainCamera.Lens.FieldOfView = Mathf.Lerp(fromFOV, toFOV, elapsed / duration);
            yield return null;
        }
        mainCamera.Lens.FieldOfView = toFOV;
    }
    private System.Collections.IEnumerator FadeVolumeCoroutine(float fromWeight, float toWeight, float duration)
    {
        if (transitionVolume == null || duration <= 0f)
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transitionVolume.weight = Mathf.Lerp(fromWeight, toWeight, elapsed / duration);
            yield return null;
        }
        transitionVolume.weight = toWeight;
    }
}
public interface ISceneTransitionController
{
    void StartTransition(bool forward);
}