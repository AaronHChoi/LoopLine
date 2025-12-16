using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;

public class PhotoCapture : MonoBehaviour, IPhotoCapture
{
    [Header("Photo Taker")]
    [SerializeField] GameObject photoFrame;
    [SerializeField] GameObject cameraUI;
    [SerializeField] float delay;

    [Header("Audio")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;

    [Header("Cooldown")]
    float photoCooldown = 1.5f;
    float nextPhotoTime = 0f;

    IPlayerStateController playerStateController;
    IPolaroidUIAnimation uiAnimation;
    IPlayerInteract playerInteract;
    IMonologueSpeaker monologueSpeaker;

    #region MAGIC_METHODS
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiAnimation = InterfaceDependencyInjector.Instance.Resolve<IPolaroidUIAnimation>();
        playerInteract = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteract>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTakePhoto += HandleTakePhoto;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTakePhoto -= HandleTakePhoto;
        }
    }
    #endregion
    private void HandleTakePhoto()
    {
        if (Time.time < nextPhotoTime) return;

        StartCoroutine(CapturePhoto());
        nextPhotoTime = Time.time + photoCooldown;
    }
    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        SoundManager.Instance.PlayQuickSound(soundData);

        uiAnimation.PhotoUIAnimation();

        yield return new WaitForSeconds(delay);

        GameObject target = playerInteract.GetRaycastTarget();
        if (target != null && target.TryGetComponent(out RaycastActivator activator))
        {
            if (activator.SetChildrenActive(true))
            {
                Events eventToPlay = activator.monologueToTrigger;
                DelayUtility.Instance.Delay(activator.monologueDelay, () => monologueSpeaker.StartMonologue(eventToPlay));
            }
        }
    }
    public void SetCameraUIVisible(bool isVisible)
    {
        cameraUI.SetActive(isVisible);
    }
}
public interface IPhotoCapture
{
    void SetCameraUIVisible(bool isVisible);
}