using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    public static bool isCameraActiveGlobal { get; private set; } = false;

    [Header("Photo Taker")]
    [SerializeField] Image photoDisplayArea;
    [SerializeField] GameObject photoFrame;
    [SerializeField] GameObject cameraUI;

    [Header("FlashEffect")]
    [SerializeField] GameObject cameraFlash;
    [SerializeField] float flashTime;

    [Header("PhotoFade")]
    [SerializeField] Animator fadingAnimation;

    [Header("World Photo")]
    [SerializeField] List<Renderer> worldPhotoRenderers = new List<Renderer>();
    [SerializeField] float brightnessFactor = 2f;

    [Header("Sound")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;
    [SerializeField] AudioSource bgmAudio;

    Texture2D screenCapture;
    bool viewvingPhoto;
    bool cameraActive = false;
    bool isCurrentPhotoClue = false;

    int photoTaken = 0;
    [SerializeField] int maxPhotos = 5;

    IPlayerInputHandler playerInputHandler;
    private void Awake()
    {
        playerInputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
    }
    private void Start()
    {
        photoTaken = 0;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }
    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
            return;

        if (viewvingPhoto)
            return;

        if (playerInputHandler.ToggleCameraPressed())
        {
            cameraActive = !cameraActive;
            cameraUI.SetActive(cameraActive);

            isCameraActiveGlobal = cameraActive;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (cameraActive && !viewvingPhoto)
            {
                if (photoTaken < maxPhotos)
                {
                    StartCoroutine(CapturePhoto());
                }
                else
                {
                    SoundManager.Instance.CreateSound()
                        .WithSoundData(soundData2)
                        .Play();
                }
            }
            else if (viewvingPhoto)
            {
                RemovePhoto();
            }
        } 
    } 
    IEnumerator CapturePhoto()
    {
        cameraUI.SetActive(false);
        cameraActive = false;

        viewvingPhoto = true;

        yield return new WaitForEndOfFrame();

        isCurrentPhotoClue = CheckIfClue();

        Rect regionToRead = new Rect (0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        AdjustBrightness(screenCapture, brightnessFactor);

        ShowPhoto();

        SoundManager.Instance.CreateSound()
            .WithSoundData(soundData)
            .Play();

        ApplyPhotoToWorldObject();

        photoTaken++;
    }
    bool CheckIfClue()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if(hit.collider.GetComponent<PhotoClue>() != null)
            {
                return true;
            }
        }
        return false;
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
        if(photoTaken < worldPhotoRenderers.Count)
        {
            Texture2D photoCopy = new Texture2D(screenCapture.width, screenCapture.height, screenCapture.format, false);
            photoCopy.SetPixels(screenCapture.GetPixels());
            photoCopy.Apply();

            worldPhotoRenderers[photoTaken].material.mainTexture = photoCopy;

            string photoObjectName = "Photo" + photoTaken;
            GameObject photoObject = GameObject.Find(photoObjectName);

            if(photoObject == null)
            {
                Debug.LogWarning($"GameObject '{photoObjectName}' no encontrado.");
                return;
            }

            Photo photoScript = photoObject.GetComponent<Photo>();
            if(photoScript == null)
            {
                photoScript = photoObject.AddComponent<Photo>();
            }

            photoScript.SetPhoto(photoCopy, isCurrentPhotoClue);
        }
    }
    void AdjustBrightness(Texture2D texture, float brigtnessFactor)
    {
        Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= brigtnessFactor;
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }
}