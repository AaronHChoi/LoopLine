using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UI;
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

    [Header("FlashEffect")]
    [SerializeField] GameObject cameraFlash;
    [SerializeField] float flashTime;
    [SerializeField] Animator fadingAnimation;

    [Header("World Photo")]
    [SerializeField] List<Renderer> worldPhotoRenderers = new List<Renderer>();
    [SerializeField] float brightnessFactor = 2f;
    [SerializeField] float brightnessFactorClue = 2f;

    [Header("Audio")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;
    [SerializeField] AudioSource bgmAudio;

    [Header("Cooldown")]
    float photoCooldown = 1.5f;
    float nextPhotoTime = 0f;

    Texture2D screenCapture;
    bool viewvingPhoto;
    public bool IsViewingPhoto {  get { return viewvingPhoto; } }

    bool cameraActive = false;
    bool isCurrentPhotoClue = false;

    [Header("UI Counter")]
    [SerializeField] TextMeshProUGUI photoCounterText;
    int photoTaken = 0;

    IPlayerStateController playerStateController;
    ITogglePhotoDetection photoDetectionZone;
    IPlayerMovement playerMovement;
    IGameSceneManager gameSceneManager;
    IBlackRoomManager blackRoomManager;
    ICameraOrientation cameraOrientation;

    public event Action<string> OnPhotoClueCaptured;
    #region MAGIC_METHODS
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        photoDetectionZone = InterfaceDependencyInjector.Instance.Resolve<ITogglePhotoDetection>();
        playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        blackRoomManager = InterfaceDependencyInjector.Instance.Resolve<IBlackRoomManager>();
        cameraOrientation = InterfaceDependencyInjector.Instance.Resolve<ICameraOrientation>();
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

        if (!viewvingPhoto)
        {
            playerMovement.CanMove = false;
            cameraOrientation.CanLook = false;
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
        else
        {
            playerMovement.CanMove = true;
            cameraOrientation.CanLook = true;
        }
    }
    IEnumerator CapturePhoto()
    {
        cameraUI.SetActive(false);
        viewvingPhoto = true;

        yield return new WaitForEndOfFrame();

        SoundManager.Instance.PlayQuickSound(soundData);

        string clueId = null;
          
        photoTaken++;
        UpdatePhotoCounter();
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