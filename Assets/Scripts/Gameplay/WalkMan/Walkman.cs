using System.Collections;
using UnityEngine;
using Player;
using Core.Utilities;
using Core.Audio;
using Core.DependencyInjection;

public class Walkman : MonoBehaviour, IWalkman
{

    [Header("WalkMan")]
    //[SerializeField] GameObject photoFrame;
    [SerializeField] GameObject walkmanUI;
    [SerializeField] float delay;
    [SerializeField] public bool isListeningAudioTape { get; set; }

    [Header("Audio")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;


    IPlayerStateController playerStateController;
    IPolaroidUIAnimation uiAnimation; /* Remplazar por WalkMan */
    IPlayerInteract playerInteract;
    IMonologueSpeaker monologueSpeaker;
    ISoundManager soundManager;

    #region MAGIC_METHODS
    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiAnimation = InterfaceDependencyInjector.Instance.Resolve<IPolaroidUIAnimation>();
        playerInteract = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteract>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTakeAudioTape += HandleListenMusic;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTakeAudioTape -= HandleListenMusic;
        }
    }
    #endregion

    private void HandleListenMusic()
    {
        if (isListeningAudioTape) return;

        StartCoroutine(ListenAudioTape());
    }

    IEnumerator ListenAudioTape()
    {
        yield return new WaitForEndOfFrame();

        //uiAnimation.PhotoUIAnimation();

        yield return new WaitForSeconds(delay);

        GameObject target = playerInteract.GetRaycastTarget();

        if (target != null && target.TryGetComponent(out IAudioTape tapeTarget))
        {
            target.gameObject.SetActive(false);

            soundManager.CreateSound()
           .WithSoundData(tapeTarget.GetSoundData())
           .Play();

            isListeningAudioTape = true;
            monologueSpeaker.StartMonologue(tapeTarget.GetMonologueToTrigger());

            yield return new WaitForSeconds(tapeTarget.GetSoundData().clip.length);

            isListeningAudioTape = false;
            target.gameObject.SetActive(true);

        }

    }

    public void SetWalkManUIVisible(bool isVisible)
    {
        walkmanUI.SetActive(isVisible);
    }
}
public interface IWalkman
{
    bool isListeningAudioTape { get; }
    void SetWalkManUIVisible(bool isVisible);
}
