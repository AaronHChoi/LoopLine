using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UI;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;
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

    int photoTaken = 0;

    IPlayerStateController playerStateController;
    IPhotoMarkerManager photoMarkerManager;
    ITogglePhotoDetection photoDetectionZone;
    public event Action<string> OnPhotoClueCaptured;
    #region MAGIC_METHODS
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        photoDetectionZone = InterfaceDependencyInjector.Instance.Resolve<ITogglePhotoDetection>();
        photoMarkerManager = InterfaceDependencyInjector.Instance.Resolve<IPhotoMarkerManager>();
    }
    private void Start()
    {
        photoTaken = 0;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnStateChanged += HandlePlayerStateChanged;
            playerStateController.OnTakePhoto += HandleTakePhoto;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnStateChanged -= HandlePlayerStateChanged;
            playerStateController.OnTakePhoto -= HandleTakePhoto;
        }
        CleanupTextures();
    }
    #endregion
    void CleanupTextures()
    {
        if(screenCapture != null)
        {
            Destroy(screenCapture);
            screenCapture = null;
        }
    }
    private void HandleTakePhoto()
    {
        if (Time.time < nextPhotoTime) return;

        if (!viewvingPhoto)
        {
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
            RemovePhoto();
        }
    }
    private void HandlePlayerStateChanged(IState newState)
    {
        if (newState == playerStateController.ObjectInHandState)
        {
            cameraActive = false;
        }
        else if (newState == playerStateController.CameraState)
        {
            cameraActive = true;
        }
        cameraUI.SetActive(cameraActive);
    }
    IEnumerator CapturePhoto()
    {
        cameraUI.SetActive(false);
        cameraActive = false;
        viewvingPhoto = true;
        
        photoMarkerManager.ShowMarker(); 

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect (0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        AdjustBrightness(screenCapture, brightnessFactor);

        ShowPhoto();

        SoundManager.Instance.PlayQuickSound(soundData);

        string clueId = null;

        if (photoDetectionZone.CheckIfAnyClue())
        {
            ApplyPhotoToWorldObject();
        }
        else
        {
            OnPhotoClueCaptured?.Invoke(clueId);
        }
        photoTaken++;
        
        photoMarkerManager.HideMarker();    
    }
    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        StartCoroutine(CameraFlashEffect());
        fadingAnimation.Play("PhotoFade");
    }
    IEnumerator CameraFlashEffect()
    {
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }
    void RemovePhoto()
    {
        viewvingPhoto = false;
        photoFrame.SetActive(false);
        cameraUI.SetActive(true);
        cameraActive = true;
    }
    void ApplyPhotoToWorldObject()
    {
        if (photoTaken < worldPhotoRenderers.Count)
        {
            Texture2D photoCopy = new Texture2D(screenCapture.width, screenCapture.height, TextureFormat.RGBA32, true, true);
            photoCopy.SetPixels(screenCapture.GetPixels());
            photoCopy.Apply();

            AdjustBrightness(photoCopy, brightnessFactorClue);

            photoCopy.wrapMode = TextureWrapMode.Clamp;
            photoCopy.filterMode = FilterMode.Bilinear;

            worldPhotoRenderers[photoTaken].material.mainTexture = photoCopy;

            string photoObjectName = "Photo" + photoTaken;
            GameObject photoObject = GameObject.Find(photoObjectName);

            if (photoObject == null)
            {
                Debug.LogWarning($"GameObject '{photoObjectName}' no encontrado.");
                return;
            }

            Photo photoScript = photoObject.GetComponent<Photo>();
            if (photoScript == null)
            {
                photoScript = photoObject.AddComponent<Photo>();
            }

            PhotoClue detectedClue = photoDetectionZone.GetClue();

            string clueId = "";
            if (detectedClue != null)
            {
                isCurrentPhotoClue = true;
                clueId = detectedClue.Type.ToString();
            }
            else
            {
                isCurrentPhotoClue = false;
            }

            photoScript.SetPhoto(photoCopy, isCurrentPhotoClue, clueId);

            if (isCurrentPhotoClue && !string.IsNullOrEmpty(clueId))
            {
                OnPhotoClueCaptured?.Invoke(clueId);
            }
        }
    }
    void AdjustBrightness(Texture2D texture, float brigtnessFactor)
    {
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = pixels[i].gamma;
            pixels[i] *= brigtnessFactor;
            pixels[i] = pixels[i].linear;
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }
}

public interface IPhotoCapture
{
    event Action<string> OnPhotoClueCaptured;
    bool IsViewingPhoto {  get; }
}