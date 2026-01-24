using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour, ICinematicManager
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject cinematicPanel;
    [SerializeField] RawImage displayImage;

    [SerializeField] float fadeDuration = 1f;

    Action onCinematicFinishedCallback;

    private void Start()
    {
        cinematicPanel.SetActive(false);
    }
    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }
    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }
    public void PlayCinematic(VideoClip clip, Action onComplete = null)
    {
        if (clip == null)
        {
            onComplete?.Invoke();
            return;
        }

        onCinematicFinishedCallback = onComplete;

        videoPlayer.clip = clip;

        if (videoPlayer.targetTexture != null)
        {
            videoPlayer.targetTexture.Release();
        }
        SetAlpha(0f);
        videoPlayer.Prepare();
    }
    void OnVideoPrepared(VideoPlayer source)
    {
        if (source.clip != null)
        {
            cinematicPanel.SetActive(true);
            source.Play();
            StartCoroutine(FadeRoutine(0f, 1f, null));
        }
    }
    void OnVideoFinished(VideoPlayer source)
    {
        StartCoroutine(FadeRoutine(1f, 0f, () =>
        {
            cinematicPanel.SetActive(false);

            onCinematicFinishedCallback?.Invoke();
            onCinematicFinishedCallback = null;
        }));
    }
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, Action onFadeComplete)
    {
        float timer = 0f;

        Color currentColor = displayImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);

            displayImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            yield return null;
        }

        displayImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);

        onFadeComplete?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        if (displayImage != null)
        {
            Color c = displayImage.color;
            displayImage.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
public interface ICinematicManager
{
    void PlayCinematic(VideoClip clip, Action onComplete = null);
}