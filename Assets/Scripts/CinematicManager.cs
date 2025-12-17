using System;
using UnityEngine;
using UnityEngine.Video;

public class CinematicManager : MonoBehaviour, ICinematicManager
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject cinematicPanel;

    Action onCinematicFinishedCallback;

    private void Start()
    {
        cinematicPanel.SetActive(false);
    }
    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }
    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
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
        cinematicPanel.SetActive(true);
        videoPlayer.Play();
    }
    void OnVideoFinished(VideoPlayer source)
    {
        cinematicPanel.SetActive(false);

        onCinematicFinishedCallback?.Invoke();
        onCinematicFinishedCallback = null;
    }
}
public interface ICinematicManager
{
    void PlayCinematic(VideoClip clip, Action onComplete = null);
}