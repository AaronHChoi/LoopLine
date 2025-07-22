using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] Image photoDisplayArea;
    [SerializeField] GameObject photoFrame;

    [Header("FlashEffect")]
    [SerializeField] GameObject cameraFlash;
    [SerializeField] float flashTime;

    [Header("PhotoFade")]
    [SerializeField] Animator fadingAnimation;

    Texture2D screenCapture;
    bool viewvingPhoto;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(!viewvingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }
        }
    } 
    IEnumerator CapturePhoto()
    {
        viewvingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect (0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
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
    }
}