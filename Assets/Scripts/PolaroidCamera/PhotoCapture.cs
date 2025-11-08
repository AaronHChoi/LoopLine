using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
using TMPro;
public class PhotoCapture : MonoBehaviour, IPhotoCapture
{
    [Header("Photo Taker")]
    [SerializeField] Image photoDisplayArea;
    [SerializeField] GameObject photoFrame;
    [SerializeField] GameObject cameraUI;
    [SerializeField] int maxPhotos = 5;

    [Header("Audio")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;
    [SerializeField] AudioSource bgmAudio;

    [Header("Cooldown")]
    float photoCooldown = 1.5f;
    float nextPhotoTime = 0f;

    [Header("UI Counter")]
    [SerializeField] TextMeshProUGUI photoCounterText;
    int photoTaken = 0;

    IPlayerStateController playerStateController;
    IPolaroidUIAnimation uiAnimation;

    public event Action<string> OnPhotoClueCaptured;
    #region MAGIC_METHODS
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiAnimation = InterfaceDependencyInjector.Instance.Resolve<IPolaroidUIAnimation>();
    }
    private void Start()
    {
        photoTaken = 0;
        UpdatePhotoCounter();
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

        if (photoTaken < maxPhotos)
        {
            StartCoroutine(CapturePhoto());
            nextPhotoTime = Time.time + photoCooldown;
        }
        else
        {
            SoundManager.Instance.PlayQuickSound(soundData2);
            nextPhotoTime = Time.time + photoCooldown;
        }
    }
    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        SoundManager.Instance.PlayQuickSound(soundData);

        photoTaken++;
        UpdatePhotoCounter();

        uiAnimation.PhotoUIAnimation();
    }
    void UpdatePhotoCounter()
    {
        int remainingPhotos = maxPhotos - photoTaken;
        if (photoCounterText != null)
        {
            photoCounterText.text = $"{remainingPhotos} / {maxPhotos}";
        }
        else
        {
            Debug.LogWarning("photoCounterText is not assigned in the inspector.");
        }
    }
    public void SetCameraUIVisible(bool isVisible)
    {
        cameraUI.SetActive(isVisible);
    }
}

public interface IPhotoCapture
{
    event Action<string> OnPhotoClueCaptured;
    void SetCameraUIVisible(bool isVisible);
}